using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Boss_SpiritDemon_Zombie : MonoBehaviour, IDamage
{
    public Boss_SpiritDemon_Zombie_Data data;
    public Boss_SpiritDemon_Summon summon;
    NavMeshAgent nav;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;

    [SerializeField]
    float hp;
    int damage;
    float move;
    float dist;

    bool isAttacking;
    bool isDead;
    public bool isFreeze;
    bool cool;
    bool isStart;
    bool isAction;

    float attackDist;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD,
        WALK,
        WAIT
    }
    public State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        coll = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        move = 0;
        isAction = false;
        isStart = false;
        isAttacking = true;
        isDead = false;
        hp = data.Health;
        damage = data.Damage;
        attackDist = data.AttackDistance;
        anim.SetBool("Dead", false);
        isFreeze = false;
        rb.isKinematic = false;
        coll.enabled = true;
        state = State.IDLE;
}

    void Update()
    {
        if(isDead) return;

        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (isFreeze)
            rb.velocity = Vector3.zero;

        /*if (!isAttacking)
            AttackLook();*/
    }

    //몬스터의 상태를 정하는 코루틴
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            //자체적으로 쿨타임을 가져 반복적으로 상태가 변화는것을 방지
            if (cool)
            {
                state = State.IDLE;
                nav.speed = 0;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }

            //플레이어가 공격거리 안에 들어왔을 때
            if (attackDist >= dist && !isAction)
            {
                state = State.ATTACK;
            }
            else if (attackDist < dist && !isAction)
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
            switch (state)
            {
                case State.IDLE:
                    StartCoroutine(Idle());
                    anim.SetFloat("Move", move);
                    nav.speed = 0;
                    break;
              /*  case State.WALK:
                    StartCoroutine(Walk());
                    anim.SetFloat("Move", move);
                    nav.speed = move * 5;
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;*/
                case State.TRACE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    nav.speed = move * 5;
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;
                case State.ATTACK:
                    rb.velocity = Vector3.zero;
                    randomAttackAnim();
                    isAttacking = true;
                    cool = true;
                    break;
                case State.DEAD:
                    isDead = true;
                    yield break;
                case State.WAIT:
                    nav.speed = 0;
                    break;
            }
            yield return wait;
        }
    }

    //근접공격 범위안에 플레이어가 있는지 판단
    //애니메이션 이벤트에서 호출
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist < attackDist)
        {
            //플레이어가 데미지 받는 메서드
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }

    void AttackLook()
    {
        Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
        moveDirection.Normalize(); // 방향을 정규화
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

    }
    //애니메이션 이벤트로 호출
    public void cooldown()
    {
        isAttacking = false;
    }

    //IDamage인터페이스 상속 메서드
    public void getDamage(int damage)
    {
        if(isStart)
            {
            hp -= damage;
            if (hp > 0)
                anim.SetTrigger("Hit");
            else
                StartCoroutine(Death());
        }
    }

    void randomAttackAnim()
    {
        int ran = Random.Range(0, 3);

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
        }
    }
    IEnumerator Move()
    {
        if (move >= 0.9)
        {
            move = 0.9f;
            yield break;
        }
        while (move <= 0.9)
        {
            move += Time.deltaTime * 0.5f;
            yield return Time.deltaTime;
        }
    }
    IEnumerator Idle()
    {
        if (move <= 0)
        {
            move = 0f;
            yield break;
        }

        while (move >= 0)
        {
            move -= Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    IEnumerator Walk()
    {
        if (move <= 0.3)
        {
            move = 0.3f;
            yield break;
        }

        while (move >= 0.3)
        {
            move -= Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
     IEnumerator Death()
    {
        nav.speed = 0;
        isDead = true;
        state = State.DEAD;
        anim.SetBool("Dead", true);
        yield return new WaitForSeconds(1);
        summon.RemoveList(gameObject);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
    public void StateStart()
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
        isAttacking = false;
        isStart = true;
    }
    public void WaitOn()
    {
        isAction = true;
    }
    public void WaitOff()
    {
        isAction = false;
        cool = true;
    }
}
