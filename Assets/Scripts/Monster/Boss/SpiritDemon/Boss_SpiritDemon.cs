using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_SpiritDemon : MonoBehaviour, IDamage
{
    [SerializeField] Boss_SpiritDemon_Data data;
    [SerializeField] GameObject PhaseFX;
    [SerializeField] GameObject ShieldFX;
    [SerializeField] GameObject SmokeFX;
    [SerializeField] GameObject HandFX;
    [SerializeField] GameObject HealFX;
    [SerializeField] GameObject DebuffFX;
    Boss_SpiritDemon_Summon summon;
    NavMeshAgent nav;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;

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
    float summonTime;
    float summonMaxTime;

    bool cool;
    bool canAttack;
    bool canCast;
    bool canSpecialAttack;
    bool canDashAttack;
    [SerializeField] bool canSummon;
    bool isAction;
    bool isSummoned;

    public bool isChange;
    [SerializeField] bool isAttacking;

    public bool isOver;
    public bool isAllDead;

    public int summonInt;

    public enum State
    {
        IDLE,
        TRACE,
        WALK,
        SUMMON,
        ATTACK,
        DASHATTACK,
        SPECIALATTACK,
        DEAD,
        WAIT,
        BLOOD
    }
    [SerializeField] State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.1f);
        summon = GetComponent<Boss_SpiritDemon_Summon>();
        nav = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
    }
    private void Start()
    {
        StartCoroutine(Action());
        StartCoroutine(Phase1State());
    }
    private void OnEnable()
    {
        summonInt = 0;
        isSummoned = false;
        isAction = false;
        isAttacking = false;
        canDashAttack = false;
        canAttack = false;
        canCast = false;
        canSpecialAttack = false;
        canSummon = true;
        move = 0;
        isDead = false;
        hp = data.Health;
        anim.SetBool("Dead", false);
        attackMaxTime = data.AttackTime;
        castMaxTime = data.CassTime;
        specialAttackMaxTime = data.SpecialAttackTime;
        DashAttackMaxTime = data.DashAttackTime;
        summonMaxTime = data.SummonTime;
        attackDist = data.AttackDistance;
        specialAttackDist = data.SpecialAttackDistance;
        DashAttackDist = data.DashAttackDistance;
        damage = data.Damage;
    }

    void Update()
    {
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        //AttackLook();

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
        summonTime += Time.fixedDeltaTime;

        if (attackTime > attackMaxTime)
            canAttack = true;
        if (castTime > castMaxTime)
            canCast = true;
        if (specialAttackTime > specialAttackMaxTime)
            canSpecialAttack = true;
        if (DashAttackTime > DashAttackMaxTime)
            canDashAttack = true;
        if (summonTime > summonMaxTime)
            canSummon = true;

        Vector3 gravity = -15f * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    //1페이즈 상태를 정하는 코루틴
    IEnumerator Phase1State()
    {
        yield return new WaitForSeconds(2f);
        print("페이즈1");
        while (!isChange)
        {
            if (cool)
            {
                state = State.IDLE;
                nav.speed = 0;
                yield return new WaitForSeconds(1f);
                cool = false;
            }
            if (isAllDead)
                state = State.SUMMON;
            else
                state = State.IDLE;

            if (isOver)
            {
                isChange = true;

                anim.SetBool("Phase", true);
                summonMaxTime = 30;
                summonTime = 0;
                canSummon = false;
                yield break;
            }
            yield return wait;
        }
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
                state = State.WALK;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }

            if (canSummon && hp < data.Health / 2)
            {
                state = State.SUMMON;
                isSummoned = true;
            }
            else if(canCast && !canSummon && hp < data.Health / 2 && isSummoned)
            {
                state = State.BLOOD;
            }
            //거리가 멀면 달려가서 접근
            else if (dist > DashAttackDist && !isAction)
            {
                state = State.TRACE;
            }
            //대쉬 공격
            else if (dist <= DashAttackDist && dist > specialAttackDist && canDashAttack && !isAction)
            {
                state = State.DASHATTACK;
            }
            //특수 공격
            else if (dist <= specialAttackDist && dist > attackDist && canSpecialAttack && !isAction)
            {
                state = State.SPECIALATTACK;
            }
            //근거리 공격
            else if (dist <= attackDist && canAttack && !isAction)
            {
                state = State.ATTACK;
                rb.velocity = Vector3.zero;
                move = 0;
            }
            //접근
            else if (dist > attackDist && !isAction)
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
                    nav.speed = 0;
                    break;
                case State.TRACE:
                    StartCoroutine(Run());
                    anim.SetFloat("Move", move);
                    nav.speed = move * 5;
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;
                case State.WALK:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    nav.speed = move * 5;
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;
                case State.SUMMON:
                    anim.SetFloat("Move", 0);
                    nav.speed = 0;
                    SummonAnim();
                    isAllDead = false;
                    canSummon = false;
                    canCast = false;
                    summonTime = 0f;
                    castTime = 0f;
                    break;
                case State.BLOOD:
                    nav.speed = 0;
                    anim.SetTrigger("Blood");
                    castTime = 0f;
                    canCast = false;
                    break;
                case State.ATTACK:
                    nav.speed = 0;
                    randomAttackAnim();
                    attackTime = 0f;
                    canAttack = false;
                    isAttacking = true;
                    rb.angularDrag = 0f;
                    break;
                case State.SPECIALATTACK:
                    nav.speed = 0;
                    randomSpecialAttackAnim();
                    specialAttackTime = 0f;
                    canSpecialAttack = false;
                    isAttacking = true;
                    break;
                case State.DASHATTACK:
                    nav.speed = 0;
                    anim.SetTrigger("DashAttack");
                    DashAttackTime = 0f;
                    canDashAttack = false;
                    isAttacking = true;
                    break;
                case State.WAIT:
                    nav.speed = 0;
                    break;
                case State.DEAD:
                    nav.speed = 0;
                    yield break;
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

        if (hp <= 0)
            return;

        if(ShieldFX.activeSelf)
        {
            print("무적상태");
        }
        else
        {
            hp -= damage;
            print("Boss 남은 HP" + hp);
            if (hp <= 0)
            {
                StartCoroutine(Death());
            }
        }
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
    public void SummonAnim()
    {
        if (canSummon)
        {
            if (summonInt % 2 == 0)
            {
                anim.SetTrigger("Summon1");
            }
            else
                anim.SetTrigger("Summon2");
        }
    }
    public void Summon()
    {
        if (summonInt > 3)
            summonInt = 0;
        switch (summonInt)
        {
            case 0:
                summon.SummonWarriorZombie();
                break;
            case 1:
                summon.SummonMageZombie();
                break;
            case 2:
                summon.SummonWarriorZombie();
                summon.SummonMageZombie();
                break;
            case 3:
                summon.SummonGolem();
                break;
            default:
                break;
        }
        summonInt++;
        summonTime = 0;

        if (hp <= data.Health / 2)
        {
            HandFxOn();
            StartCoroutine(Skull());
        }
    }
    //애니메이션 이벤트에서 호출
    public void Phase2FX()
    {
        PhaseFX.SetActive(true);
        StartCoroutine(PhaseFxOff());
    }
    IEnumerator PhaseFxOff()
    {
        yield return new WaitForSeconds(8f);
        PhaseFX.SetActive(false);
    }
    public void WaitOn()
    {
        isAction = true;
    }
    public void WaitOff()
    {
        isAction = false;
    }
    public void ShieldOff()
    {
        ShieldFX.SetActive(false);
    }
    public void SmokeOn()
    {
        if(hp < data.Health / 2)
            SmokeFX.SetActive(true);
    }
    IEnumerator Skull()
    {
        yield return new WaitForSeconds(3);
        BloodReady();
    }
    void BloodReady()
    {
        foreach (GameObject obj in summon.summonList)
        {
            Boss_SpiritDemon_Golem golem = obj.GetComponent<Boss_SpiritDemon_Golem>();
            Boss_SpiritDemon_Zombie zombie = obj.GetComponent<Boss_SpiritDemon_Zombie>();
            Boss_SpiritDemon_Zombie_Mage mage = obj.GetComponent<Boss_SpiritDemon_Zombie_Mage>();
            if (golem != null)
                golem.Skull();
            if(zombie != null)
                zombie.Skull();
            if(mage != null) 
                mage.Skull();
        }
    }

    public void BloodBoom()
    {
        bool isNull = true;
        foreach (GameObject obj in summon.summonList)
        {
            Boss_SpiritDemon_Golem golem = obj.GetComponent<Boss_SpiritDemon_Golem>();
            Boss_SpiritDemon_Zombie zombie = obj.GetComponent<Boss_SpiritDemon_Zombie>();
            Boss_SpiritDemon_Zombie_Mage mage = obj.GetComponent<Boss_SpiritDemon_Zombie_Mage>();
            if (golem != null)
            {
                hp += golem.hp * 2;
                golem.getDamage(1000);
                golem.Blood();
                isNull = false;
            }
            if (zombie != null)
            {
                hp += zombie.hp;
                zombie.getDamage(1000);
                zombie.Blood();
                isNull = false;
            }
            if (mage != null)
            {
                hp += mage.hp * 1.5f;
                mage.getDamage(1000);
                mage.Blood();
                isNull = false;
            }
        }
        if (!isNull)
        {
            HealFX.SetActive(true);
        }
        else
        {
            DebuffFX.SetActive(true);
            hp -= 200;
        }
    }
    public void HandFxOn()
    {
        HandFX.SetActive(true);
    }
    public void HandFxOff()
    {
        HandFX.SetActive(false);
    }
    IEnumerator Death()
    {
        state = State.DEAD;
        anim.SetTrigger("Dead");
        SmokeFX.SetActive(false);
        nav.speed = 0;
        isDead = true;
        yield return new WaitForSeconds(1);
        rb.isKinematic = true;
        coll.enabled = false;
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}
