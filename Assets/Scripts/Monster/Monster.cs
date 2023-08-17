using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;

    public MonsterShield shield;

    float hp;
    float speed;
    float damage;

    bool isChase;
    bool isDead;
    public bool isFindPlayer;

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
        DEAD,
        HIT
    }
    public State state = State.IDLE;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        playerLayer = LayerMask.NameToLayer("PLAYER");
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        StartCoroutine(Action());
    }
    private void OnEnable()
    {
        isDead = false;
        isChase = false;
        hp = data.Health;
        speed = data.Speed;
        damage = data.Damage;
        attackDist = data.AttackDistance;
        viewRange = data.ViewRange;
        TraceTime = 0;
        Type = data.MonsterType;
    }

    void Update()
    {
        StartCoroutine(CheckState());
        chaseTime += Time.deltaTime;
        chase();
        StopTrace();
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        //�þ߿� �÷��̾ ������ �ʾҰų�
        //�ֺ��� �÷��̾ �����ϴ� ���Ͱ� ���� ��
        if (viewRange >= dist)
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
        else if (isFindPlayer)
        {
            state = State.TRACE;
        }
        //hp�� 0���ϰ� �Ǿ��� ��
        else if (hp <= 0)
        {
            state = State.DEAD;
        }
        else
            state = State.IDLE;

        yield return wait;
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
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Idle");
                    break;
                case State.TRACE:
                    anim.SetBool("Run", true);
                    switch (Type)
                    {
                        case 4:
                            shield.ShieldDash();
                            StartCoroutine(TracePlayer());
                            break;
                        default:
                            StartCoroutine(TracePlayer());
                            break;
                    }
                    FindPlayer();
                    break;
                case State.ATTACK:
                    isFindPlayer = false;
                    chaseTime = 0f;
                    isChase = true;
                    anim.SetBool("Run", false);
                    rb.velocity = Vector3.zero;
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
            rb.velocity = moveDirection * speed;

            yield return Time.deltaTime;
        }
    }
    //�������� �����ȿ� �÷��̾ �ִ��� �Ǵ�
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        if (dist < attackDist + 0.5f)
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
                Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
                moveDirection.Normalize(); // ������ ����ȭ
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
                rb.velocity = moveDirection * speed;
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
        if (Type == 4)
        {
            shield.ShieldAttack();
            return;
        }
        else if (Type == 5 || Type == 6)
        {
            anim.SetTrigger("Attack");
            return;
        }
        else
        {
            int ran = Random.Range(0, 4);

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
    }
}
