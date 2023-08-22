using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Elemental : MonoBehaviour
{
    public MonsterData data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;

    public MonsterShield shield;

    float hp;
    float speed;
    float damage;
    float move;
    float moveSpeed;
    bool isDead;

    float attackDist;

    int Type;

    float attackTime;
    float attackMaxTime = 3f;
    float mageTime;
    float mageMaxTime = 7f;

    bool canAttack;
    bool canMage;
    bool canSpell;

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
        StartCoroutine(Action());
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
        Type = data.MonsterType;
        anim.SetBool("Dead", false);
    }

    void Update()
    {
        StartCoroutine(CheckState());
        moveSpeed = move * speed;
        attackTime += Time.deltaTime;
        mageTime += Time.deltaTime;

        AttackLook();

        if (attackTime > attackMaxTime)
            canAttack = true;
        if(mageTime > mageMaxTime)
            canMage = true;
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while(!isDead)
        {
            float dist = Vector3.Distance(GameManager.Instance.testPlayer.transform.position, transform.position);

            //���
            if (hp <= 0)
            {
                state = State.DEAD;
                isDead = true;
            }
            //ü�� ���� ���ϰ� �Ǹ� ����
            else if (hp < data.Health / 2 && !isChanged)
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
            else if (dist > 5 && canAttack)
            {
                state = State.ATTACK;
            }
            //�ٰŸ� ����
            else if (dist < 5 && canMage)
            {
                state = State.MAGE;
            }
            //����
            else if (dist < 5)
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
                    rb.velocity = Vector3.zero;
                    randomAttackAnim();
                    break;
                case State.MAGE:
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Mage");
                    break;
                case State.CHANGE:
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Roar");
                    break;
                case State.HIT:
                    anim.SetTrigger("Hit");
                    break;
                case State.DEAD:
                    anim.SetBool("Dead", true);
                    break;
                case State.SPELL:
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

        if (dist < attackDist + 0.5f)
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
        while (move <= 1)
        {
            move += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    IEnumerator Idle()
    {
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
}
