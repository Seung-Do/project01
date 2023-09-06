using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterGargoyle : MonoBehaviour, IDamage
{
    public MonsterData data;
    [SerializeField] SpawnManager spawnManager;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    CapsuleCollider coll;
    BoxCollider deadColl;
    NavMeshAgent nav;
    public GameObject firePos;
    public ParticleSystem MetoerCircle;

    public float hp;
    float speed;
    float damage;
    public float move;

    Vector3 originColl;

    bool isFly;
    bool canCast;
    bool isChase;
    bool isDead;
    bool isAttack;
    bool cool;
    bool isStart;
    [SerializeField] bool isAction;
    public bool isFindPlayer;
    public bool isFreeze;

    int playerLayer;
    int enemyLayer;

    float chaseTime = 10f;
    float chaseMaxTime = 5f;
    public float TraceTime = 0f;
    float TraceMaxTime = 5f;
    float CastTime = 0f;
    float CastMaxTime = 5f;
    float ActionTime = 0f;
    float ActionMaxTime = 5f;

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
        deadColl = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        playerLayer = LayerMask.NameToLayer("PLAYER");
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        nav = GetComponent<NavMeshAgent>();
        originColl = coll.center;
    }
    private void OnEnable()
    {
        isFly = false;
        move = 0;
        isStart = false;
        isAttack = false;
        canCast = true;
        isAction = false;
        isDead = false;
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
        if(!isStart && spawnManager.spawnList.Count == 0)
        {
            StartCoroutine(StartAnim());
        }

        if(!isStart)
            return;
         
        if (hp <= 0)
        {
            state = State.DEAD;
            nav.speed = 0;
            return;
        }

        //chaseTime += Time.deltaTime;
        //chase();
        //StopTrace();

        if (isAttack && !isFreeze)
            AttackLook();
    }

    void FixedUpdate()
    {
        CastTime += Time.fixedDeltaTime;
        ActionTime += Time.fixedDeltaTime;

        if (CastTime > CastMaxTime)
            canCast = true;
        if (isAction)
            if (ActionTime > ActionMaxTime)
                isAction = false;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return wait;

            float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

            if (hp < data.Health / 2 && !isFly && !isAction)
            {
                //attackDist = data.AttackDistance + 1;
                viewRange = data.ViewRange + 5;
                anim.SetTrigger("DoFly");
                yield return new WaitForSeconds(1);
            }
;
            //��ü������ ��Ÿ���� ���� �ݺ������� ���°� ��ȭ�°��� ����
            if (cool)
            {
                print("��Ÿ��");
                state = State.IDLE;
                yield return new WaitForSeconds(1f);
                cool = false;
            }
            //�÷��̾ ���ݰŸ� �ȿ� ������ ��
            if (viewRange >= dist)
            {
                if (attackDist >= dist && !isAction)
                {
                    state = State.ATTACK;
                }
                else if (attackDist < dist && canCast && !isAction)
                {
                    state = State.CAST;
                }
                else
                    state = State.TRACE;
            }
            //�ֺ� ���Ͱ� �÷��̾ �߰����� ��
            else if (isFindPlayer || viewRange < dist && !isAction)
            {
                state = State.TRACE;
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
                    nav.speed = move * speed;
                    break;
                case State.TRACE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    nav.speed = move * speed;
                    StartCoroutine(TracePlayer());
                    FindPlayer();
                    break;
                case State.CAST:
                    anim.SetTrigger("Cast");
                    nav.speed = 0;
                    canCast = false;
                    CastTime = 0;
                    break;
                case State.ATTACK:
                    /*if (isFly)
                        StartCoroutine(Back());*/
                    isFindPlayer = false;
                    chaseTime = 0f;
                    isChase = true;
                    nav.speed = 0;
                    //transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
                    AttackAnim();
                    break;
                case State.DEAD:
                    isDead = true;
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

        Vector3 dir = (GameManager.Instance.playerTr.position - transform.position).normalized;
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
            /* Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
             moveDirection.Normalize(); // ������ ����ȭ
             Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);*/
            nav.SetDestination(GameManager.Instance.playerTr.position);

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
                float dist = Vector3.Distance(transform.position, GameManager.Instance.playerTr.position);

                if (dist < 3)
                {
                    /* Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
                     moveDirection.Normalize(); // ������ ����ȭ
                     Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);*/
                    nav.SetDestination(GameManager.Instance.playerTr.position);
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
        if(!isStart) return;

        if (hp <= 0)
            return;

        if (state == State.IDLE)
            isFindPlayer = true;

        hp -= damage;
        if (hp > 0)
            anim.SetTrigger("Hit");
        else
            StartCoroutine(Death());
    }
    //�ִϸ��̼��̺�Ʈ���� hit�ִϸ��̼� ������ ȣ��
    public void Hit()
    {
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
        float dist = Vector3.Distance(transform.position, GameManager.Instance.playerTr.position);

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
        Vector3 pos = new Vector3(GameManager.Instance.playerTr.position.x + Random.Range(-1f, 1f), GameManager.Instance.playerTr.position.y / 2, GameManager.Instance.playerTr.position.z + Random.Range(-1f, 1f));
        GameObject Circle = GameManager.Instance.poolManager[0].Get(4);
        Circle.transform.position = pos;
        yield return new WaitForSeconds(2.5f);
        GameObject meteor = GameManager.Instance.poolManager[0].Get(3);
        meteor.transform.position = pos;
    }
    //�ִϸ��̼� �̺�Ʈ�� ȣ��
    public void WaitOn()
    {
        isAction = true;
    }
    public void WaitOff()
    {
        isAction = false;
        cool = true;
    }
    public void AttackOn()
    {
        isAttack = true;
        ActionTime = 0;
    }
    public void AttackOff()
    {
        isAttack = false;
    }
    IEnumerator Death()
    {
        nav.speed = 0;
        isDead = true;
        anim.SetBool("Dead", true);
        state = State.DEAD;
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
    public void StartState()
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
        isStart = true;
    }

    IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(1);
        isStart = true;
        anim.SetTrigger("Start");
    }
}
