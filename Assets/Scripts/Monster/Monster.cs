using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Monster : FreezableMonster, IDamage
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
    //public bool isFreeze;

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


    //몬스터의 상태를 정하는 코루틴
    IEnumerator CheckState()
    {
        yield return wait;
        while (!isDead)
        {
            //hp가 0이하가 되었을 때
            if (hp <= 0)
            {
                state = State.DEAD;
                print("죽음");
                yield break;
            }

            float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

            //시야에 플레이어가 들어오지 않았거나
            //주변에 플레이어를 공격하는 몬스터가 없을 때
            if (viewRange >= dist)
            {
                // print("주변에 플레이어가 있음");
                //시야에 플레이어가 들어왔을 때
                if (isTracePlayer())
                {
                    //print("시야에 플레이어가 있음");
                    //플레이어가 보이면
                    if (ViewPlayer() && !isAction)
                    {
                        // print("플레이어가 보임");
                        //플레이어가 공격거리 안에 들어왔을 때
                        if (attackDist >= dist)
                        {
                            state = State.ATTACK;
                        }
                        else
                            state = State.TRACE;
                    }
                    //플레이어가 안보이면
                    else
                        state = State.IDLE;

                }
                else
                    state = State.IDLE;
            }
            //주변 몬스터가 플레이어를 발견했을 때
            else if (isFindPlayer && !isAction)
            {
                state = State.TRACE;
            }

            else
                state = State.IDLE;

            yield return wait;
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

        Vector3 player = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y + 0.5f, GameManager.Instance.playerTr.position.z);
        Vector3 dir = (player - transform.position).normalized;
        //플레이어를 향해 레이캐스트
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            // print(hit.collider.gameObject.name);
            //찾으면 true 못찾으면 false반환
            Find = hit.collider.CompareTag("PLAYER");
        }
        return Find;
    }
    //상태가 TRACE일때 플레이어방향으로 이동하는 메서드
    IEnumerator TracePlayer()
    {
        //상태가 TRACE일때만 반복
        while (state == State.TRACE && !isFreeze)
        {
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
            moveDirection.Normalize();// 방향을 정규화
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            //rb.velocity = moveDirection * moveSpeed;

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
            print("�÷��̾� ������" + damage);
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }
    //플레이어를 발견하고 추격할 때 주변에 있는 몬스터에게 알리는 메서드
    void FindPlayer()
    {
        //발견반경 * 1.5만큼 범위내의 몬스터들 찾기
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange * 0.7f, 1 << enemyLayer);

        //있을 경우 inFindPlayer를 true로 변경
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
    //갑자기 시야각에서 플레이어가 사라지면 idle로 변경되던 문제를 해결하기위한 메서드
    //한번이라도 공격시작하면 chaseMaxTime만큼 시야각에서 벗어나도 추격함
    void chase()
    {
        if (isChase && state != State.ATTACK && !isFreeze)
        {
            if (chaseTime <= chaseMaxTime)
            {
                Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
                moveDirection.Normalize(); // 방향을 정규화
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
        moveDirection.Normalize();  // 방향을 정규화
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

    //IDamage인터페이스 상속 메서드
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
        rb.velocity = Vector3.zero;
        isFindPlayer = false;
        isChase = false;
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
    }

    /*public void IFreeze()
    {
        if(isFreeze) isFreeze = false;
        else isFreeze = true;
    }*/
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
