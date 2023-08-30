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
            print("����");
        }
    }
    private void FixedUpdate()
    {
        attackTime += Time.fixedDeltaTime;

        if (attackTime > attackMaxTime)
            canAttack = true;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while (!isDead)
        {
            //��ü������ ��Ÿ���� ���� �ݺ������� ���°� ��ȭ�°��� ����
            if (cool)
            {
                print("��Ÿ��");
                state = State.IDLE;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }
            //����
            else if (dist > attackDist)
            {
                state = State.TRACE;
            }
            //�ٰŸ� ����
            else if (dist <= attackDist && canAttack)
            {
                state = State.ATTACK;
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

    //���°� TRACE�϶� �÷��̾�������� �̵��ϴ� �޼���
    IEnumerator TracePlayer()
    {
        //���°� TRACE�϶��� �ݺ�
        while (state == State.TRACE)
        {
            Vector3 moveDirection = GameManager.Instance.playerTr.position - transform.position;
            moveDirection.Normalize(); // ������ ����ȭ
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            rb.velocity = moveDirection * moveSpeed;

            yield return Time.deltaTime;
        }
    }
    //�������� �����ȿ� �÷��̾ �ִ��� �Ǵ�
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    public void attack()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (dist <= attackDist)
        {
            //�÷��̾ ������ �޴� �޼���
            print("�÷��̾� ������" + damage);
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(damage);
        }
    }

    //������ �÷��̾����� ����
    void AttackLook()
    {
        if (true)
        {
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
    //�̵��� �ӵ� ����
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

    public IEnumerator Ready()
    {
        yield return new WaitForSeconds(10f);
        anim.SetBool("Spelling", true);
    }

    //�ִϸ��̼� �̺�Ʈ�� ȣ��
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
        /*//�ǰݵǸ� �ӵ� ����
        speed -= 0.5f;
        //�ӵ� ȸ�� �ڷ�ƾ
        StartCoroutine(RecoverySpeed());*/

        hp -= damage;
        print("���� HP" + hp);
    }
}
