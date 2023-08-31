using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss_SpiritDemon : MonoBehaviour
{
    [SerializeField] Boss_SpiritDemon_Data data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;

    [SerializeField] float hp;
    public int damage;
    [SerializeField] float move;
    bool isDead;

    float attackDist;
    float specialAttackDist;
    float DashAttackDist;
    float dist;

    float attackTime;
    float attackMaxTime;
    float castTime;
    float castMaxTime;
    float specialAttackTime;
    float specialAttackMaxTime;
    float DashAttackTime;
    float DashAttackMaxTime;

    bool cool;
    bool canAttack;
    bool canCast;
    bool canSpecialAttack;
    bool canDashAttack;

    [SerializeField] bool isChange;
    [SerializeField] bool isAttacking;

    public enum State
    {
        IDLE,
        TRACE,
        WALK,
        ATTACK,
        DASHATTACK,
        SPECIALATTACK,
        MAGE,
        DEAD
    }
    [SerializeField] State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.1f);
    }
    private void Start()
    {
        StartCoroutine(Action());
        StartCoroutine(Phase1State());
    }
    private void OnEnable()
    {
        isAttacking = false;
        canDashAttack = false;
        canAttack = false;
        canCast = false;
        canSpecialAttack = false;
        move = 0;
        isDead = false;
        hp = data.Health;
        anim.SetBool("Dead", false);
        attackMaxTime = data.AttackTime;
        castMaxTime = data.CassTime;
        specialAttackMaxTime = data.SpecialAttackTime;
        DashAttackMaxTime = data.DashAttackTime;
        attackDist = data.AttackDistance;
        specialAttackDist = data.SpecialAttackDistance;
        DashAttackDist = data.DashAttackDistance;
        damage = data.Damage;
    }

    void Update()
    {
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

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
        DashAttackTime += Time.fixedDeltaTime;
        castTime += Time.fixedDeltaTime;
        specialAttackTime += Time.fixedDeltaTime;

        if (attackTime > attackMaxTime)
            canAttack = true;
        if (castTime > castMaxTime)
            canCast = true;
        if (specialAttackTime > specialAttackMaxTime)
            canSpecialAttack = true;
        if (DashAttackTime > DashAttackMaxTime)
            canDashAttack = true;

        Vector3 gravity = -15f * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    //1페이즈 상태를 정하는 코루틴
    IEnumerator Phase1State()
    {
        print("페이즈1");
        /*while (isChange)
        {
            yield return wait;

            StartCoroutine(Phase2State());
            yield break;
        }*/
        isChange = true;
        anim.SetBool("Phase", true);
        yield break;
    }
    //2페이즈 상태를 정하는 코루틴
    IEnumerator Phase2State()
    {
        print("페이즈2");
        while (!isDead)
        {
            //자체적으로 쿨타임을 가져 반복적으로 상태가 변화는것을 방지
            if (cool)
            {
                print("쿨타임");
                state = State.WALK;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }
            //거리가 멀면 달려가서 접근
            else if (dist > DashAttackDist)
            {
                state = State.TRACE;
            }
            //대쉬 공격
            else if (dist <= DashAttackDist && dist > specialAttackDist && canDashAttack)
            {
                state = State.DASHATTACK;
            }
            //특수 공격
            else if (dist <= specialAttackDist && dist > attackDist && canSpecialAttack)
            {
                state = State.SPECIALATTACK;
            }
            //근거리 공격
            else if (dist <= attackDist && canAttack)
            {
                state = State.ATTACK;
                rb.velocity = Vector3.zero;
                move = 0;
            }
            //접근
            else if (dist > attackDist)
            {
                state = State.WALK;
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
            yield return new WaitForSeconds(1f);
            switch (state)
            {
                case State.IDLE:
                    StartCoroutine(Idle());
                    anim.SetFloat("Move", move);
                    break;
                case State.TRACE:
                    StartCoroutine(Run());
                    anim.SetFloat("Move", move);
                    break;
                case State.WALK:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    break;
                case State.ATTACK:
                    move = 0;
                    randomAttackAnim();
                    attackTime = 0f;
                    canAttack = false;
                    isAttacking = true;
                    break;
                case State.SPECIALATTACK:
                    move = 0;
                    randomSpecialAttackAnim();
                    specialAttackTime = 0f;
                    canSpecialAttack = false;
                    isAttacking = true;
                    break;
                case State.DASHATTACK:
                    move = 0;
                    anim.SetTrigger("DashAttack");
                    DashAttackTime = 0f;
                    canDashAttack = false;
                    isAttacking = true;
                    break;
                case State.DEAD:
                    move = 0;
                    anim.SetTrigger("Dead");
                    isDead = true;
                    break;
            }
        }
    }

    //시점을 플레이어한테 고정
    void AttackLook()
    {
        if (isChange && !isAttacking)
        {
            //Vector3 pos = new Vector3(GameManager.Instance.playerTr.position.x, 1, GameManager.Instance.playerTr.position.z);
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
    //랜덤하게 특수공격 모션 재생
    void randomSpecialAttackAnim()
    {
        int ran = Random.Range(0, 4);

        switch (ran)
        {
            case 0:
                anim.SetTrigger("Attack3");
                break;
            case 1:
                anim.SetTrigger("Attack4");
                break;
            case 2:
                anim.SetTrigger("Attack5");
                break;
            case 3:
                anim.SetTrigger("Attack6");
                break;
        }
    }
    //걷기 속도 증가
    IEnumerator Move()
    {
        if (move >= 0.6)
        {
            move = 0.6f;
            yield break;
        }
        while (move <= 0.6)
        {
            move += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    //달리기 속도 증가
    IEnumerator Run()
    {
        if (move >= 0.8)
        {
            move = 0.8f;
            yield break;
        }
        while (move <= 0.8f)
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

    //애니메이션 이벤트로 호출
    public void cooldown()
    {
        cool = true;
        isAttacking = false;
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

    public void ChangePhase()
    {
        StartCoroutine(Phase2State());
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerDamage player = collision.collider.GetComponent<PlayerDamage>();
        if (player != null)
        {
            Vector3 repulsionDirection = (transform.position - GameManager.Instance.playerTr.position).normalized;
            rb.AddForce(repulsionDirection * 10, ForceMode.Impulse);
        }
    }
}
