using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss_SpiritDemon_Zombie : MonoBehaviour, IDamage
{
    public Boss_SpiritDemon_Zombie_Data data;
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

    float attackDist;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD,
        WALK
    }
    public State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        coll = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        move = 0;
        isAttacking = true;
        isDead = false;
        hp = data.Health;
        damage = data.Damage;
        attackDist = data.AttackDistance;
        anim.SetBool("Dead", false);
        isFreeze = false;
        rb.isKinematic = false;
        coll.enabled = true;
    }

    void Update()
    {
        if(isDead) return;

        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);

        if (isFreeze)
            rb.velocity = Vector3.zero;

        if (!isAttacking)
            AttackLook();

        if (!isFreeze && !isAttacking)
            backup();
    }

    //������ ���¸� ���ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        yield return wait;
        while (!isDead)
        {
            //hp�� 0���ϰ� �Ǿ��� ��
            if (hp <= 0)
            {
                state = State.DEAD;
                print("����");
                yield break;
            }

            //��ü������ ��Ÿ���� ���� �ݺ������� ���°� ��ȭ�°��� ����
            if (cool)
            {
                print("��Ÿ��");
                state = State.WALK;
                yield return new WaitForSeconds(0.5f);
                cool = false;
            }

            //�÷��̾ ���ݰŸ� �ȿ� ������ ��
            if (attackDist >= dist)
            {
                state = State.ATTACK;
            }
            else if (attackDist < dist)
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
                case State.WALK:
                    StartCoroutine(Walk());
                    anim.SetFloat("Move", move);
                    break;
                case State.TRACE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    break;
                case State.ATTACK:
                    rb.velocity = Vector3.zero;
                    randomAttackAnim();
                    isAttacking = true;
                    cool = true;
                    break;
                case State.DEAD:
                    anim.SetBool("Dead", true);
                    isDead = true;
                    StartCoroutine(Death());
                    yield break;
            }
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
            print("�÷��̾� ������" + damage);
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
        cool = true;
        isAttacking = false;
    }

    //IDamage�������̽� ��� �޼���
    public void getDamage(int damage)
    {
        hp -= damage;
        if (hp > 0)
            anim.SetTrigger("Hit");
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
        yield return new WaitForSeconds(1);
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
    }
    void backup()
    {
        if(dist < 1.5f)
        {
            print("�ڷ� ������");
            transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.z);

        }
    }
}
