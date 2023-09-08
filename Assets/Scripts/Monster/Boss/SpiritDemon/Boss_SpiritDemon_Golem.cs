using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_SpiritDemon_Golem : MonoBehaviour, IDamage, IFreeze
{
    public Boss_SpiritDemon_Zombie_Data data;
    public Boss_SpiritDemon_Summon summon;
    [SerializeField] GameObject SkullFX;
    [SerializeField] GameObject BloodFX;
    [SerializeField] GameObject FireFX;
    NavMeshAgent nav;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;

    [SerializeField]
    public float hp;
    int damage;
    float move;
    float dist;

    bool isAttacking;
    bool isDead;
    public bool isFreeze;
    bool cool;
    bool isStart;
   [SerializeField] bool isAction;
    bool canJumpAttack;

    float attackDist;
    float JumpAttackTime;
    float JumpAttackMaxTime = 10f;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD,
        WAIT,
        JUMPATTACK
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
        SkullFX.SetActive(false);
        BloodFX.SetActive(false);
        move = 0;
        JumpAttackTime = 0f;
        canJumpAttack = false;
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
        if (isDead) return;
        //�÷��̾���� �Ÿ� ����
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

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

        /*if (!isAttacking)
            AttackLook();*/
    }
    private void FixedUpdate()
    {
        JumpAttackTime += Time.fixedDeltaTime;
        if (JumpAttackTime > JumpAttackMaxTime)
            canJumpAttack = true;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            //hp�� 0���ϰ� �Ǿ��� ��
            if (hp <= 0)
            {
                state = State.DEAD;
                yield break;
            }

            //��ü������ ��Ÿ���� ���� �ݺ������� ���°� ��ȭ�°��� ����
            if (cool)
            {
                state = State.IDLE;
                nav.speed = 0;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }

            //�÷��̾ ���ݰŸ� �ȿ� ������ ��
            if (attackDist >= dist && !isAction)
            {
                if (canJumpAttack)
                {
                    state = State.JUMPATTACK;
                }
                else
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

    //���¿� ���� �ൿ�� �����ϴ� �ڷ�ƾ
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
                case State.TRACE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    nav.speed = move * 4;
                    nav.SetDestination(GameManager.Instance.playerTr.position);
                    break;
                case State.ATTACK:
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Attack");
                    isAttacking = true;
                    cool = true;
                    break;
                case State.JUMPATTACK:
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("JumpAttack");
                    isAttacking = true;
                    cool = true;
                    canJumpAttack = false;
                    JumpAttackTime = 0;
                    break;
                case State.DEAD:
                    isDead = true;
                    StartCoroutine(Death());
                    yield break;
                case State.WAIT:
                    nav.speed = 0;
                    break;
            }
            yield return wait;
        }
    }

    //�������� �����ȿ� �÷��̾ �ִ��� �Ǵ�
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist < attackDist)
        {
            //�÷��̾ ������ �޴� �޼���
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }

    void AttackLook()
    {
        Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
        moveDirection.Normalize(); // ������ ����ȭ
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

    }
    //�ִϸ��̼� �̺�Ʈ�� ȣ��
    public void cooldown()
    {
        isAttacking = false;
    }

    //IDamage�������̽� ��� �޼���
    public void getDamage(int damage)
    {
        if (hp <= 0)
            return;

        if (isStart)
        {
            hp -= damage;
            nav.speed = 0;
            if (hp <= 0)
                StartCoroutine(Death());
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
    public void JumpAttack()
    {
        Vector3 pos = new Vector3(-0.4f, 0.3f, -1);
        GameObject jumpAttack = GameManager.Instance.poolManager[2].Get(5);
        jumpAttack.transform.position = transform.position + Vector3.down * 0.2f;
    }
    public void Skull()
    {
        SkullFX.SetActive(true);
    }
    public void Blood()
    {
        BloodFX.SetActive(true);
    }
    public void FireFxOn()
    {
        FireFX.SetActive(true);
    }
    public void IFreeze()
    {
        if (isFreeze) isFreeze = false;
        else isFreeze = true;
    }
}
