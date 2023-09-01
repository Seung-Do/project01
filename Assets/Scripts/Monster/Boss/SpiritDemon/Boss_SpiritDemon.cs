using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss_SpiritDemon : MonoBehaviour, IDamage
{
    [SerializeField] Boss_SpiritDemon_Data data;
    Boss_SpiritDemon_Summon summon;
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

    public bool isOver;
    public bool isAllDead;

    int summonTime;

    public enum State
    {
        IDLE,
        TRACE,
        WALK,
        SUMMON,
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
        summon = GetComponent<Boss_SpiritDemon_Summon>();
    }
    private void Start()
    {
        StartCoroutine(Action());
        StartCoroutine(Phase1State());
    }
    private void OnEnable()
    {
        summonTime = 0;
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
            print("����");
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

    //1������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator Phase1State()
    {
        print("������1");
        yield return new WaitForSeconds(1f);
        while (!isOver)
        {
            if (isAllDead)
                state = State.SUMMON;
            else
                state = State.IDLE;

            if (isOver)
            {
                isChange = true;

                anim.SetBool("Phase", true);
                yield break;
            }
            yield return wait;
        }
    }
    //2������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator Phase2State()
    {
        print("������2");
        while (!isDead)
        {
            //��ü������ ��Ÿ���� ���� �ݺ������� ���°� ��ȭ�°��� ����
            if (cool)
            {
                print("��Ÿ��");
                state = State.WALK;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }
            //�Ÿ��� �ָ� �޷����� ����
            else if (dist > DashAttackDist)
            {
                state = State.TRACE;
            }
            //�뽬 ����
            else if (dist <= DashAttackDist && dist > specialAttackDist && canDashAttack)
            {
                state = State.DASHATTACK;
            }
            //Ư�� ����
            else if (dist <= specialAttackDist && dist > attackDist && canSpecialAttack)
            {
                state = State.SPECIALATTACK;
            }
            //�ٰŸ� ����
            else if (dist <= attackDist && canAttack)
            {
                state = State.ATTACK;
                rb.velocity = Vector3.zero;
                move = 0;
            }
            //����
            else if (dist > attackDist)
            {
                state = State.WALK;
            }
            else
                state = State.IDLE;

            yield return wait;
        }
    }

    //���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ
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
                case State.SUMMON:
                    SummonAnim();
                    isAllDead = false;
                    break;
                case State.ATTACK:
                    move = 0;
                    randomAttackAnim();
                    attackTime = 0f;
                    canAttack = false;
                    isAttacking = true;
                    rb.angularDrag = 0f;
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

    //������ �÷��̾����� ����
    void AttackLook()
    {
        if (isChange && !isAttacking)
        {
            //Vector3 pos = new Vector3(GameManager.Instance.playerTr.position.x, 1, GameManager.Instance.playerTr.position.z);
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
            moveDirection.Normalize(); // ������ ����ȭ
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        }
    }

    //�����ϰ� ���� ��� ���
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
    //�����ϰ� Ư������ ��� ���
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
    //�ȱ� �ӵ� ����
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
    //�޸��� �ӵ� ����
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
    //Idle �� �ӵ� ����
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

    //�ִϸ��̼� �̺�Ʈ�� ȣ��
    public void cooldown()
    {
        cool = true;
        isAttacking = false;
    }

    public void getDamage(int damage)
    {
        /*//�ǰݵǸ� �ӵ� ����
        speed -= 0.5f;
        //�ӵ� ȸ�� �ڷ�ƾ
        StartCoroutine(RecoverySpeed());*/

        hp -= damage;
        print("���� HP" + hp);
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
        if (summonTime % 2 == 0)
        {
            anim.SetTrigger("Summon1");
        }
        else
            anim.SetTrigger("Summon2");
    }
    public void Summon()
    {
        switch (summonTime)
        {
            case 0:
                summon.SummonWarriorZombie();
                print("�ٰŸ� ���� ��ȯ");
                break;
            case 1:
                summon.SummonMageZombie();
                print("���Ÿ� ���� ��ȯ");
                break;
            case 2:
                summon.SummonWarriorZombie();
                summon.SummonMageZombie();
                print("�ٰŸ� ���Ÿ� ���� ��ȯ");
                break;
            case 3:
                print("�� ��ȯ");
                break;
            default:
                break;
        }
        summonTime++;
    }
}
