using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Witch : MonoBehaviour, IDamage, IFreeze
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;
    public SpawnManager spawn;

    [SerializeField]
    float hp;
    float speed;
    float move;
    float moveSpeed;

    bool isChase;
    bool isDead;
    bool isGetHit;
    public bool isFindPlayer;
    public bool isFreeze;

    int playerLayer;
    int enemyLayer;

    float chaseTime = 10f;
    float chaseMaxTime = 5f;
    public float TraceTime = 0f;
    float TraceMaxTime = 5f;

    float attackDist;
    float dist;

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
    }
    private void OnEnable()
    {
        move = 0;
        isGetHit = false;
        isDead = false;
        isChase = false;
        hp = data.Health;
        speed = data.Speed;
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
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        StopTrace();
        StopMove();
        moveSpeed = move * speed;

        if (isFreeze)
            rb.velocity = Vector3.zero;

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
            //�¾��� ��
            if (isGetHit)
            {
                state = State.TRACE;
                isChase = true;
                isGetHit = false;
            }  
            //�þ߿� �÷��̾ ������ �ʾҰų�
            //�ֺ��� �÷��̾ �����ϴ� ���Ͱ� ���� ��
           else if (viewRange >= dist)
            {
                //�þ߿� �÷��̾ ������ ��
                if (isTracePlayer())
                {
                    //print("�þ߿� �÷��̾ ����");
                    //�÷��̾ ���̸�
                    if (ViewPlayer())
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
                    StartCoroutine(TracePlayer());
                    FindPlayer();
                    break;
                case State.ATTACK:
                    isFindPlayer = false;
                    isGetHit = false;
                    anim.SetFloat("Move", 0);
                    chaseTime = 0f;
                    isChase = true;
                    rb.velocity = Vector3.zero;
                    AttackAnim();
                    break;
                case State.DEAD:
                    anim.SetBool("Dead", true);
                    move = 0;
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
            rb.velocity = moveDirection * moveSpeed;

            yield return Time.deltaTime;
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
                rb.velocity = moveDirection * moveSpeed;
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
        anim.SetTrigger("Attack");
    }

    //IDamage�������̽� ��� �޼���
    public void getDamage(int damage)
    {
        if (hp <= 0)
            return;

        if (state == State.IDLE)
            isGetHit = true;

        hp -= damage;
        if (hp > 0)
            anim.SetTrigger("Hit");
        else
            anim.SetBool("Dead", true);
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
    IEnumerator Death()
    {
        isFindPlayer = false;
        isChase = false;
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
    void StopMove()
    {
        if(dist < attackDist / 2)
        {
            rb.velocity = Vector3.zero;
            move = 0;
        }
    }
    public void IFreeze()
    {
        if (isFreeze) isFreeze = false;
        else isFreeze = true;
    }
}
