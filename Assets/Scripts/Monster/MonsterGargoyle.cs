using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MonsterGargoyle : MonoBehaviour, IDamage
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    CapsuleCollider coll;
    public GameObject firePos;
    public ParticleSystem MetoerCircle;

    public float hp;
    float speed;
    float damage;
    public float move;
    float moveSpeed;

    Vector3 originColl;

    bool isFly;
    bool isHit;
    bool isChase;
    bool isDead;
    public bool isFindPlayer;
    public bool isFreeze;

    int playerLayer;
    int enemyLayer;

    float chaseTime = 10f;
    float chaseMaxTime = 5f;
    public float TraceTime = 0f;
    float TraceMaxTime = 5f;

    float attackDist;

    public float viewRange;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD,
        HIT,
        CAST
    }
    public State state = State.IDLE;

    void Awake()
    {
        coll = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        playerLayer = LayerMask.NameToLayer("PLAYER");
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        StartCoroutine(Action());
        StartCoroutine(CheckState());
        originColl = coll.center;
    }
    private void OnEnable()
    {
        isFly = false;
        move = 0;
        isDead = false;
        isHit = false;
        isChase = false;
        hp = data.Health;
        speed = data.Speed;
        damage = data.Damage;
        TraceTime = 0;
        anim.SetBool("Dead", false);
        isFreeze = false;

        attackDist = data.AttackDistance;
        viewRange = data.ViewRange;
        anim.SetBool("Fly", false);
        anim.SetBool("Ground", true);
    }

    void Update()
    {
        chaseTime += Time.deltaTime;
        chase();
        StopTrace();
        moveSpeed = move * speed;

        if (isFreeze)
            rb.velocity = Vector3.zero;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return wait;

            float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

            if (hp < data.Health / 2 && !isFly)
            {
                attackDist = data.AttackDistance + 5;
                viewRange = data.ViewRange + 5;
                anim.SetTrigger("DoFly");
                isFly = true;
                yield return new WaitForSeconds(1);
            }

            //hp�� 0���ϰ� �Ǿ��� ��
            if (hp <= 0)
            {
                state = State.DEAD;
                isDead = true;
            }
            //�þ߿� �÷��̾ ������ �ʾҰų�
            //�ֺ��� �÷��̾ �����ϴ� ���Ͱ� ���� ��
            else if (viewRange >= dist)
            {
                //�þ߿� �÷��̾ ������ ��
                if (isTracePlayer())
                {
                    //�÷��̾ ���̸�
                    if (ViewPlayer())
                    {
                        //�÷��̾ ���ݰŸ� �ȿ� ������ ��
                        if (attackDist >= dist)
                        {
                            if (3 >= dist)
                            {
                                state = State.ATTACK;
                            }
                            else
                            {
                                state = State.CAST;
                                yield return new WaitForSeconds(6f);
                            }
                        }
                        else
                            state = State.TRACE;
                    }
                    //�÷��̾ �Ⱥ��̸�
                    else
                        state = State.IDLE;

                }
                else
                    state = State.IDLE;

            }
            //�ֺ� ���Ͱ� �÷��̾ �߰����� ��
            else if (isFindPlayer)
            {
                state = State.TRACE;
            }
            //������ �޾��� ��
            else if (isHit)
            {
                state = State.HIT;
            }
            else
                state = State.IDLE;
        }
    }

    //���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ
    IEnumerator Action()
    {
        while (!isDead)
        {
            yield return wait;
            switch (state)
            {
                case State.IDLE:
                    StartCoroutine(Idle());
                    anim.SetFloat("Move", move);
                    break;
                case State.TRACE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    StartCoroutine(TracePlayer());
                    FindPlayer();
                    break;
                case State.CAST:
                    anim.SetTrigger("Cast");
                    rb.velocity = Vector3.zero;
                    AttackLook();
                    break;
                case State.ATTACK:
                    if (isFly)
                        StartCoroutine(Back());
                    isFindPlayer = false;
                    chaseTime = 0f;
                    isChase = true;
                    rb.velocity = Vector3.zero;
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
                    AttackLook();
                    AttackAnim();
                    break;
                case State.HIT:
                    anim.SetTrigger("Hit");
                    break;
                case State.DEAD:
                    anim.SetBool("Dead", true);
                    break;
            }
        }
    }


    //�þ߰��� �÷��̾ �ֳ� ���� üũ�ϴ� �޼���
    bool isTracePlayer()
    {
        bool FindPlayer = false;
        //������ �ݰ游ŭ OverlapSphere �޼ҵ带 Ȱ���Ͽ� �÷��̾� Ž��
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange, 1 << playerLayer);

        //������ �ݰ�ȿ� �÷��̾ Ž���ȴٸ�
        if (colls.Length == 1)
        {
            Vector3 dir = (GameManager.Instance.testPlayer.transform.position - transform.position).normalized;
            //���� �þ߰��� �÷��̾ �����ϴ��� �Ǵ�
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5)
            {
                FindPlayer = true;
            }
        }
        return FindPlayer;
    }

    //Ŀ���ҿ����Ϳ� ���� �������
    public Vector3 CirclePoint(float angle)
    {
        //angle�� Ʈ��Ʈ���� y�� �°� ��������
        angle += transform.eulerAngles.y;

        //�׳� �ޱ۸� ���ϸ� ���� ������ �Ͼ�Ƿ� pi/2(Radion)�� ������
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    //�÷��̾ �������� üũ�ϴ� �޼���
    public bool ViewPlayer()
    {
        bool Find = false;
        RaycastHit hit;

        Vector3 dir = (GameManager.Instance.testPlayer.transform.position - transform.position).normalized;
        //�÷��̾ ���� ����ĳ��Ʈ
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            //ã���� true ��ã���� false��ȯ
            Find = hit.collider.CompareTag("PLAYER");
        }
        return Find;
    }
    //���°� TRACE�϶� �÷��̾�������� �̵��ϴ� �޼���
    IEnumerator TracePlayer()
    {
        //���°� TRACE�϶��� �ݺ�
        while (state == State.TRACE)
        {
            Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
            moveDirection.Normalize(); // ������ ����ȭ
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            rb.velocity = moveDirection * moveSpeed;

            yield return Time.deltaTime;
        }
    }
    //�������� �����ȿ� �÷��̾ �ִ��� �Ǵ�
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        if (dist < 3)
        {
            //�÷��̾ ������ �޴� �޼���
            print("�÷��̾� ������" + damage);
        }
    }
    //�÷��̾ �߰��ϰ� �߰��� �� �ֺ��� �ִ� ���Ϳ��� �˸��� �޼���
    void FindPlayer()
    {
        //�߰߹ݰ� * 1.5��ŭ �������� ���͵� ã��
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange * 1.5f, 1 << enemyLayer);

        //���� ��� inFindPlayer�� true�� ����
        if (colls.Length >= 1)
        {
            foreach (var coll in colls)
            {
                Monster monster = coll.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.isFindPlayer = true;
                    monster.TraceTime = 0f;
                }
            }
        }
    }
    //���ڱ� �þ߰����� �÷��̾ ������� idle�� ����Ǵ� ������ �ذ��ϱ����� �޼���
    //�ѹ��̶� ���ݽ����ϸ� chaseMaxTime��ŭ �þ߰����� ����� �߰���
    void chase()
    {
        if (isChase && state != State.ATTACK)
        {
            if (chaseTime <= chaseMaxTime)
            {
                float dist = Vector3.Distance(transform.position, GameManager.Instance.testPlayer.transform.position);

                if (dist < 3)
                {
                    Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
                    moveDirection.Normalize(); // ������ ����ȭ
                    Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
                    rb.velocity = moveDirection * moveSpeed;
                }
                else
                    return;
            }
            else
                isChase = false;
        }
    }
    void AttackLook()
    {
        Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
        moveDirection.Normalize(); // ������ ����ȭ
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

    }

    //�Ÿ��� �־����� ���������� �߰��ϴ� ���� �ذ� �޼���
    void StopTrace()
    {
        if (isFindPlayer)
        {
            TraceTime += Time.deltaTime;

            if (TraceTime > TraceMaxTime)
            {
                TraceTime = 0f;
                isFindPlayer = false;
            }
        }
    }
    void AttackAnim()
    {
        int ran = Random.Range(0, 2);

        switch (ran)
        {
            case 0:
                anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
        }
    }

    //IDamage�������̽� ��� �޼���
    public void getDamage(int damage)
    {
        isHit = true;
        //������ �޴� ���� �ۼ�
    }
    //�ִϸ��̼��̺�Ʈ���� hit�ִϸ��̼� ������ ȣ��
    public void Hit()
    {
        isHit = false;
    }

    IEnumerator Move()
    {
        if (move >= 1)
        {
            move = 1;
            yield break;
        }
        while (move <= 1)
        {
            move += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    IEnumerator Idle()
    {
        if (move <= 0)
        {
            move = 0;
            yield break;
        }

        while (move >= 0)
        {
            move -= Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    IEnumerator Back()
    {
        float dist = Vector3.Distance(transform.position, GameManager.Instance.testPlayer.transform.position);

        if (dist < 5)
        {
            if (move <= -1)
            {
                move = -1;
                yield break;
            }
            while (move >= -1)
            {
                move -= Time.deltaTime;
                yield return Time.deltaTime;
            }
        }
    }
    public void Fly()
    {
        anim.SetBool("Ground", false);
        anim.SetBool("Fly", true);
    }
    public void Ground()
    {
        anim.SetBool("Ground", true);
        anim.SetBool("Fly", false);
    }

    public void ColliderUp()
    {
        coll.center = new Vector3(originColl.z, originColl.y + 1, originColl.x);
        isFly = true;
    }
    public IEnumerator Cast1()
    {
        GameObject light = GameManager.Instance.poolManager[0].Get(2);
        light.transform.position = firePos.transform.position;
        yield return new WaitForSeconds(1.3f);
        MonsterGargoyleBullet bullet = light.GetComponent<MonsterGargoyleBullet>();
        bullet.lookPlayer();
        bullet.Throw = true;
    }
    public IEnumerator Cast2()
    {
        MetoerCircle.Play();
        yield return new WaitForSeconds(1f);
    }
    public void meteor()
    {
        GameObject meteor = GameManager.Instance.poolManager[0].Get(3);
        meteor.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x + Random.Range(-1f, 1f), GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z + Random.Range(-1f, 1f));
    }
    public void Freeze()
    {
        StartCoroutine(OffFreeze());
    }
    IEnumerator OffFreeze()
    {
        yield return new WaitForSeconds(5f);
        isFreeze = false;
    }
}
