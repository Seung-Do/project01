using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamage, IFreeze
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;
    NavMeshAgent nav;
    public SpawnManager spawn;

    public MonsterShield shield;

    [SerializeField]
    float hp;
    float speed;
    int damage;
    float move;
    //float moveSpeed;

    bool isChase;
    bool isDead;
    bool isAction;
    public bool isFindPlayer;
    public bool isFreeze;

    int playerLayer;
    int enemyLayer;

    float chaseTime = 10f;
    float chaseMaxTime = 5f;
    public float TraceTime = 0f;
    float TraceMaxTime = 5f;

    float attackDist;

    int Type;

    public float viewRange;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }
    public State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        playerLayer = LayerMask.NameToLayer("PLAYER");
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        coll = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        move = 0;
        isDead = false;
        isAction = false;
        isChase = false;
        hp = data.Health;
        speed = data.Speed;
        damage = data.Damage;
        attackDist = data.AttackDistance;
        viewRange = data.ViewRange;
        TraceTime = 0;
        Type = data.MonsterType;
        anim.SetBool("Dead", false);
        isFreeze = false;
        StartCoroutine(Action());
        StartCoroutine(CheckState());
        rb.isKinematic = false;
        coll.enabled = true;
    }

    void Update()
    {
        if (isDead) return;
        chaseTime += Time.deltaTime;
        chase();
        StopTrace();
        //moveSpeed = move * speed;

        if (isFreeze)
        {
            nav.isStopped = true;
            isAction = true;
        }
        else
        {
            nav.isStopped = false;
            isAction = false;
        }

        if (isChase || isFindPlayer)
            AttackLook();
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        yield return wait;
        while (!isDead)
        {
            //hp�� 0���ϰ� �Ǿ��� ��
            if (hp <= 0)
            {
                state = State.DEAD;
                print("����");
                yield break;
            }

            float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

            //�þ߿� �÷��̾ ������ �ʾҰų�
            //�ֺ��� �÷��̾ �����ϴ� ���Ͱ� ���� ��
            if (viewRange >= dist)
            {
                // print("�ֺ��� �÷��̾ ����");
                //�þ߿� �÷��̾ ������ ��
                if (isTracePlayer())
                {
                    //print("�þ߿� �÷��̾ ����");
                    //�÷��̾ ���̸�
                    if (ViewPlayer() && !isAction)
                    {
                        // print("�÷��̾ ����");
                        //�÷��̾ ���ݰŸ� �ȿ� ������ ��
                        if (attackDist >= dist)
                        {
                            state = State.ATTACK;
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
            else if (isFindPlayer && !isAction)
            {
                state = State.TRACE;
            }

            else
                state = State.IDLE;

            yield return wait;
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
                    nav.isStopped = false;
                    switch (Type)
                    {
                        case 4:
                            shield.ShieldDash();
                            break;
                    }
                    StartCoroutine(TracePlayer());
                    FindPlayer();
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;
                case State.ATTACK:
                    isFindPlayer = false;
                    chaseTime = 0f;
                    isChase = true;
                    nav.isStopped = true;
                    AttackAnim();
                    break;
                case State.DEAD:
                    //anim.SetBool("Dead", true);
                    isDead = true;
                    StartCoroutine(Death());
                    yield break;
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
        if (colls.Length >= 1)
        {
            Vector3 dir = (GameManager.Instance.playerTr.position - transform.position).normalized;
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

        Vector3 player = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y + 0.5f, GameManager.Instance.playerTr.position.z);
        Vector3 dir = (player - transform.position).normalized;
        //�÷��̾ ���� ����ĳ��Ʈ
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            // print(hit.collider.gameObject.name);
            //ã���� true ��ã���� false��ȯ
            Find = hit.collider.CompareTag("PLAYER");
        }
        return Find;
    }
    //���°� TRACE�϶� �÷��̾�������� �̵��ϴ� �޼���
    IEnumerator TracePlayer()
    {
        //���°� TRACE�϶��� �ݺ�
        while (state == State.TRACE && !isFreeze)
        {
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
            moveDirection.Normalize(); // ������ ����ȭ
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            //rb.velocity = moveDirection * moveSpeed;

            yield return Time.deltaTime;
        }
    }
    //�������� �����ȿ� �÷��̾ �ִ��� �Ǵ�
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist < attackDist + 0.5f)
        {
            //�÷��̾ ������ �޴� �޼���
            print("�÷��̾� ������" + damage);
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }
    //�÷��̾ �߰��ϰ� �߰��� �� �ֺ��� �ִ� ���Ϳ��� �˸��� �޼���
    void FindPlayer()
    {
        //�߰߹ݰ� * 1.5��ŭ �������� ���͵� ã��
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange * 0.7f, 1 << enemyLayer);

        //���� ��� inFindPlayer�� true�� ����
        if (colls.Length >= 1)
        {
            foreach (var coll in colls)
            {
                Monster monster = coll.GetComponent<Monster>();
                Monster_Witch witch = coll.GetComponent<Monster_Witch>();

                if (monster != null)
                {
                    monster.isFindPlayer = true;
                    monster.TraceTime = 0f;
                }
                else if (witch != null)
                {
                    witch.isFindPlayer = true;
                    witch.TraceTime = 0f;
                }
            }
        }
    }
    //���ڱ� �þ߰����� �÷��̾ ������� idle�� ����Ǵ� ������ �ذ��ϱ����� �޼���
    //�ѹ��̶� ���ݽ����ϸ� chaseMaxTime��ŭ �þ߰����� ����� �߰���
    void chase()
    {
        if (isChase && state != State.ATTACK && !isFreeze)
        {
            if (chaseTime <= chaseMaxTime)
            {
                Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
                moveDirection.Normalize(); // ������ ����ȭ
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
                //rb.velocity = moveDirection * moveSpeed;
            }
            else
                isChase = false;
        }
    }
    void AttackLook()
    {
        Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
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
        if (Type == 4)
        {
            shield.ShieldAttack();
        }
        else if (Type == 5 || Type == 6 || Type == 10)
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            randomAttackAnim();
        }
    }

    //IDamage�������̽� ��� �޼���
    public void getDamage(int damage)
    {
        if (hp <= 0)
            return;
        if (state == State.IDLE)
            state = State.TRACE;

        hp -= damage;
        if (hp > 0 && !isFreeze)
            StartCoroutine(getHit());
        else if (hp <= 0)
            anim.SetBool("Dead", true);

    }

    void randomAttackAnim()
    {
        int ran = Random.Range(0, 4);
        if(Type == 8)
            ran = Random.Range(0, 3); 

        switch (ran)
        {
            case 0:
                anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
            case 2:
                anim.SetTrigger("Attack3");
                break;
            case 3:
                anim.SetTrigger("Attack4");
                break;
        }
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
        nav.speed = move * speed;
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
    IEnumerator Death()
    {
        nav.speed = 0; 
        isFindPlayer = false;
        isChase = false;
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
    }

    public void IFreeze()
    {
        if(isFreeze) isFreeze = false;
        else isFreeze = true;
    }
    IEnumerator getHit()
    {
        anim.SetTrigger("Hit");
        nav.isStopped = true;
        yield return new WaitForSeconds(1);
        nav.isStopped = false;
    }
    public void Hit()
    {
        //�ִϸ��̼� �̺�Ʈ �޼��� ������
    }
}
