using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Elemental : MonoBehaviour
{
    public BossData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    public GameObject pos;

    float hp;
    float speed;
    float damage;
    float move;
    float moveSpeed;
    bool isDead;

    float attackDist;

    int Type;

    float attackTime;
    float attackMaxTime = 1f;
    float mageTime;
    float mageMaxTime = 4.5f;

    public bool canAttack;
    public bool canMage;
    bool canSpell;

    bool cool;
    bool isChanged;
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        MAGE,
        DEAD,
        HIT,
        CHANGE,
        SPELL
    }
    public State state = State.IDLE;

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
        canSpell = true;
        canMage = false;
        canAttack = false;
        isChanged = false;
        move = 0;
        isDead = false;
        hp = data.Health;
        speed = data.Speed;
        damage = data.Damage;
        attackDist = data.AttackDistance;
        anim.SetBool("Dead", false);
    }

    void Update()
    {
        moveSpeed = move * speed;

        AttackLook();
    }
    private void FixedUpdate()
    {
        attackTime += Time.fixedDeltaTime;
        mageTime += Time.fixedDeltaTime;

        if (attackTime > attackMaxTime)
            canAttack = true;
        if (mageTime > mageMaxTime)
            canMage = true;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while(!isDead)
        {
            //���
            if (hp <= 0)
            {
                state = State.DEAD;
                isDead = true;
            }

            if (cool)
            {
                state = State.IDLE;
                print("��ٿ� ����");
                yield return new WaitForSeconds(0.5f);
                cool = false;
                print("��ٿ� ����");
            }

            float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

            //ü�� ���� ���ϰ� �Ǹ� ����
            if (hp < data.Health / 2 && !isChanged)
            {
                state = State.CHANGE;
                isChanged = true;
                canSpell = true;
            }
            //ü�� 70������ 60���̻��϶�, 30������ 20���̻��϶� ��ü����  
            else if(((hp / data.Health * 100) <= 70 && (hp / data.Health * 100) >= 60 && canSpell)
                || ((hp / data.Health * 100) <= 30 && (hp / data.Health * 100) >= 20) && canSpell)
            {
                state = State.SPELL;
            } 
            //���Ÿ� ����
            else if (dist > 5 && canMage)
            {
                state = State.MAGE;
                yield return new WaitForSeconds(5);
            }
            //����
            else if (dist > attackDist && !canMage)
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
                case State.MAGE:
                    move = 0;
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Mage"); 
                    yield return new WaitForSeconds(5);
                    cooldown();
                    useMage();
                    break;
                case State.CHANGE:
                    move = 0;
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Roar");
                    break;
                case State.HIT:
                    move = 0;
                    anim.SetTrigger("Hit");
                    break;
                case State.DEAD:
                    move = 0;
                    anim.SetBool("Dead", true);
                    break;
                case State.SPELL:
                    move = 0;
                    anim.SetTrigger("Cast");
                    anim.SetBool("Spelling", true);
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
            Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
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
        float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

        if (dist < attackDist - 0.5f)
        {
            //�÷��̾ ������ �޴� �޼���
            print("�÷��̾� ������" + damage);
        }
    }

    void AttackLook()
    {
        Vector3 moveDirection = GameManager.Instance.testPlayer.transform.position - transform.position;
        moveDirection.Normalize(); // ������ ����ȭ
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);

    }

    //IDamage�������̽� ��� �޼���
    public void getDamage()
    {
        //������ �޴� ���� �ۼ�
    }

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

    public IEnumerator casting()
    {
        yield return new WaitForSeconds(10f);
        anim.SetBool("Spelling", false);
        canSpell = false;
    }

    public void cooldown()
    {
        cool = true;
    }
    public void useMage()
    {
        mageTime = 0f;
        canMage = false;
        anim.SetBool("Spelling", false);
    }
    public void useAttack()
    {
        canAttack = false;
    }
    public void Skill()
    {
        StartCoroutine(FireBall(10));
        //StartCoroutine(TimeCast());
    }

    IEnumerator FireBall(int num)
    {
        GameObject skill = GameManager.Instance.poolManager.Get(num);
        skill.transform.position = pos.transform.position;
        anim.SetBool("Spelling", true);
        BossBullet bullet = skill.GetComponent<BossBullet>();
        yield return new WaitForSeconds(2.5f);
        bullet.lookPlayer();
        bullet.Throw = true;
    }
    IEnumerator TimeCast()
    {
        GameObject skill = GameManager.Instance.poolManager.Get(8);
        skill.transform.position = transform.position;
        anim.SetBool("Spelling", true);
        yield return new WaitForSeconds(1.5f);
        GameObject Time = GameManager.Instance.poolManager.Get(9);
        Time.transform.position = GameManager.Instance.testPlayer.transform.position;
    }
}
