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

    //몬스터의 상태를 정하는 코루틴
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

            //hp가 0이하가 되었을 때
            if (hp <= 0)
            {
                state = State.DEAD;
                isDead = true;
            }
            //시야에 플레이어가 들어오지 않았거나
            //주변에 플레이어를 공격하는 몬스터가 없을 때
            else if (viewRange >= dist)
            {
                //시야에 플레이어가 들어왔을 때
                if (isTracePlayer())
                {
                    //플레이어가 보이면
                    if (ViewPlayer())
                    {
                        //플레이어가 공격거리 안에 들어왔을 때
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
                    //플레이어가 안보이면
                    else
                        state = State.IDLE;

                }
                else
                    state = State.IDLE;

            }
            //주변 몬스터가 플레이어를 발견했을 때
            else if (isFindPlayer)
            {
                state = State.TRACE;
            }
            //데미지 받았을 때
            else if (isHit)
            {
                state = State.HIT;
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


    //시야각에 플레이어가 있나 없는 체크하는 메서드
    bool isTracePlayer()
    {
        bool FindPlayer = false;
        //설정된 반경만큼 OverlapSphere 메소드를 활용하여 플레이어 탐지
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange, 1 << playerLayer);

        //설정된 반경안에 플레이어가 탐지된다면
        if (colls.Length == 1)
        {
            Vector3 dir = (GameManager.Instance.testPlayer.transform.position - transform.position).normalized;
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

        Vector3 dir = (GameManager.Instance.testPlayer.transform.position - transform.position).normalized;
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
            Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
            moveDirection.Normalize(); // 방향을 정규화
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            rb.velocity = moveDirection * moveSpeed;

            yield return Time.deltaTime;
        }
    }
    //근접공격 범위안에 플레이어가 있는지 판단
    //애니메이션 이벤트에서 호출
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        if (dist < 3)
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
                float dist = Vector3.Distance(transform.position, GameManager.Instance.testPlayer.transform.position);

                if (dist < 3)
                {
                    Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
                    moveDirection.Normalize(); // 방향을 정규화
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
        isHit = true;
        //데미지 받는 내용 작성
    }
    //애니메이션이벤트에서 hit애니메이션 끝날때 호출
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
