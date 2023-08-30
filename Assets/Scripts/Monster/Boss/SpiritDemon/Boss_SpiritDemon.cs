using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon : MonoBehaviour
{
    public BossData[] data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;

    [SerializeField] float hp;
    float speed;
    int damage;
    [SerializeField] float move;
    [SerializeField] float moveSpeed;
    bool isDead;

    float attackDist;
    float dist;

    float attackTime;
    float attackMaxTime;

    bool cool;
    bool canAttack;

    [SerializeField] float MaxHp;

    bool isChange;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        MAGE,
        DEAD
    }
    [SerializeField] State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
    }
    private void Start()
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
    }
    private void OnEnable()
    {
        canAttack = false;
        move = 0;
        isDead = false;
        hp = MaxHp;
        anim.SetBool("Dead", false);
    }

    void Update()
    {
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);
        moveSpeed = move * speed;

        if(isChange)
            AttackLook();

        if (hp <= 0)
        {
            state = State.DEAD;
            print("죽음");
        }
    }
    private void FixedUpdate()
    {
        attackTime += Time.fixedDeltaTime;

        if (attackTime > attackMaxTime)
            canAttack = true;
    }

    //몬스터의 상태를 정하는 코루틴
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            //자체적으로 쿨타임을 가져 반복적으로 상태가 변화는것을 방지
            if (cool)
            {
                print("쿨타임");
                state = State.IDLE;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }
            //접근
            else if (dist > attackDist)
            {
                state = State.TRACE;
            }
            //근거리 공격
            else if (dist <= attackDist && canAttack)
            {
                state = State.ATTACK;
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
                    StartCoroutine(TracePlayer());
                    break;
                case State.ATTACK:
                    move = 0;
                    rb.velocity = Vector3.zero;
                    randomAttackAnim();
                    attackTime = 0f;
                    canAttack = false;
                    break;
                case State.DEAD:
                    move = 0;
                    anim.SetTrigger("Dead");
                    isDead = true;
                    break;
            }
        }
    }

    //상태가 TRACE일때 플레이어방향으로 이동하는 메서드
    IEnumerator TracePlayer()
    {
        //상태가 TRACE일때만 반복
        while (state == State.TRACE)
        {
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
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
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist <= attackDist)
        {
            //플레이어가 데미지 받는 메서드
            print("플레이어 데미지" + damage);
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }

    //시점을 플레이어한테 고정
    void AttackLook()
    {
        if (true)
        {
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
            moveDirection.Normalize(); // 방향을 정규화
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        }
    }

    //랜덤하게 공격 모션 재생
    void randomAttackAnim()
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
    //이동시 속도 증가
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
    //Idle 시 속도 감소
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

    public IEnumerator Ready()
    {
        yield return new WaitForSeconds(10f);
        anim.SetBool("Spelling", true);
    }

    //애니메이션 이벤트로 호출
    public void cooldown()
    {
        cool = true;
    }
    public void useAttack()
    {
        canAttack = false;
    }

    public IEnumerator normalCasting()
    {
        anim.SetBool("NormalSpell", false);
        yield return null;
    }

    public void getDamage(int damage)
    {
        /*//피격되면 속도 감소
        speed -= 0.5f;
        //속도 회복 코루틴
        StartCoroutine(RecoverySpeed());*/

        hp -= damage;
        print("남은 HP" + hp);
    }
}
