using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
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
    }

    void Update()
    {
        StartCoroutine(CheckState());
        chaseTime += Time.deltaTime;
        chase();
    }

    IEnumerator CheckState()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        //시야에 플레이어가 들어오지 않았거나
        //주변에 플레이어를 공격하는 몬스터가 없을 때
        if (viewRange >= dist)
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
        else if (isFindPlayer)
        {
            state = State.TRACE;
        }
        //hp가 0이하가 되었을 때
        else if (hp <= 0)
        {
            state = State.DEAD;
        }
        else
            state = State.IDLE;

        yield return wait;
    }
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
                    StartCoroutine(TracePlayer());
                    FindPlayer();
                    break;
                case State.ATTACK:
                    chaseTime = 0f;
                    isChase = true;
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Idle");
                    rb.velocity = Vector3.zero;
                    RandomAttack();
                    FindPlayer();
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
    bool isTracePlayer()
    {
        bool FindPlayer = false;
        //설정된 반경만큼 OverlapSphere 메소드를 활용하여 플레이어 탐지
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange, 1 << playerLayer);

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

    public Vector3 CirclePoint(float angle)
    {
        //angle을 트랜트폼의 y에 맞게 설정해줌
        angle += transform.eulerAngles.y;

        //그냥 앵글만 곱하면 수의 오류가 일어나므로 pi/2(Radion)을 곱해줌
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    public bool ViewPlayer()
    {
        bool Find = false;
        RaycastHit hit;

        Vector3 dir = (GameManager.Instance.testPlayer.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            Find = hit.collider.CompareTag("PLAYER");
        }
        return Find;
    }

    IEnumerator TracePlayer()
    {
        while (state == State.TRACE)
        {
            Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
            moveDirection.Normalize(); // 방향을 정규화
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            rb.velocity = moveDirection * speed;

            yield return Time.deltaTime;
        }
    }

    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        if (dist < attackDist + 0.5f)
        {
            //플레이어가 데미지 받는 메서드
            print("플레이어 데미지" + damage);
        }
    }
    void FindPlayer()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange * 1.5f, 1 << enemyLayer);

        if (colls.Length >= 1)
        {
            foreach (var coll in colls)
            {
                Monster monster = coll.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.isFindPlayer = true;
                }
            }
        }
    }
    void chase()
    {
        if (isChase && state != State.ATTACK)
        {
            if (chaseTime <= chaseMaxTime)
            {
                Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
                moveDirection.Normalize(); // 방향을 정규화
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
                rb.velocity = moveDirection * speed;
            }
            else
                isChase = false;
        }
    }
    void RandomAttack()
    {
        Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
        moveDirection.Normalize(); // 방향을 정규화
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

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
