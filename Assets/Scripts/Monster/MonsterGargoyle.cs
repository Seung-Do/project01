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

    //몬스터의 상태를 정하는 코루틴
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
            //자체적으로 쿨타임을 가져 반복적으로 상태가 변화는것을 방지
            if (cool)
            {
                print("쿨타임");
                state = State.IDLE;
                yield return new WaitForSeconds(1f);
                cool = false;
            }
            //플레이어가 공격거리 안에 들어왔을 때
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
            //주변 몬스터가 플레이어를 발견했을 때
            else if (isFindPlayer || viewRange < dist && !isAction)
            {
                state = State.TRACE;
            }
            else
                state = State.IDLE;
        }
    }
    //상태에 따른 행동을 실행하는 코루틴
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


    //시야각에 플레이어가 있나 없는 체크하는 메서드
    bool isTracePlayer()
    {
        bool FindPlayer = false;
        //설정된 반경만큼 OverlapSphere 메소드를 활용하여 플레이어 탐지
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange, 1 << playerLayer);

        //설정된 반경안에 플레이어가 탐지된다면
        if (colls.Length >= 1)
        {
            Vector3 dir = (GameManager.Instance.playerTr.position - transform.position).normalized;
            //적의 시야각에 플레이어가 존재하는지 판단
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5)
            {
                FindPlayer = true;
            }
        }
        return FindPlayer;
    }

    //커스텀에디터에 사용될 기즈모설정
    public Vector3 CirclePoint(float angle)
    {
        //angle을 트랜트폼의 y에 맞게 설정해줌
        angle += transform.eulerAngles.y;

        //그냥 앵글만 곱하면 수의 오류가 일어나므로 pi/2(Radion)을 곱해줌
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    //플레이어가 숨었는지 체크하는 메서드
    public bool ViewPlayer()
    {
        bool Find = false;
        RaycastHit hit;

        Vector3 dir = (GameManager.Instance.playerTr.position - transform.position).normalized;
        //플레이어를 향해 레이캐스트
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            //찾으면 true 못찾으면 false반환
            Find = hit.collider.CompareTag("PLAYER");
        }
        return Find;
    }
    //상태가 TRACE일때 플레이어방향으로 이동하는 메서드
    IEnumerator TracePlayer()
    {
        //상태가 TRACE일때만 반복
        while (state == State.TRACE)
        {
            /* Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
             moveDirection.Normalize(); // 방향을 정규화
             Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);*/
            nav.SetDestination(GameManager.Instance.playerTr.position);

            yield return Time.deltaTime;
        }
    }
    //근접공격 범위안에 플레이어가 있는지 판단
    //애니메이션 이벤트에서 호출
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist < attackDist + 0.5f)
        {
            //플레이어가 데미지 받는 메서드
            print("플레이어 데미지" + damage);
        }
    }
    //플레이어를 발견하고 추격할 때 주변에 있는 몬스터에게 알리는 메서드
    void FindPlayer()
    {
        //발견반경 * 1.5만큼 범위내의 몬스터들 찾기
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange * 1.5f, 1 << enemyLayer);

        //있을 경우 inFindPlayer를 true로 변경
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
    //갑자기 시야각에서 플레이어가 사라지면 idle로 변경되던 문제를 해결하기위한 메서드
    //한번이라도 공격시작하면 chaseMaxTime만큼 시야각에서 벗어나도 추격함
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
                     moveDirection.Normalize(); // 방향을 정규화
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
        moveDirection.Normalize(); // 방향을 정규화
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    //거리가 멀어져도 지속적으로 추격하는 문제 해결 메서드
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

    //IDamage인터페이스 상속 메서드
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
    //애니메이션이벤트에서 hit애니메이션 끝날때 호출
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
    //애니메이션 이벤트로 호출
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
