using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Zombie_Mage : MonoBehaviour, IDamage
{
    [SerializeField] Boss_SpiritDemon_Zombie_Data data;
    [SerializeField] GameObject pos;
    [SerializeField] ParticleSystem par;
    public Boss_SpiritDemon_Summon summon;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    Collider coll;

    [SerializeField]
    float hp;

    bool isAttacking;
    bool isDead;
    public bool isFreeze;
    bool cool;
    bool isStart;

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
        isAttacking = true;
        isStart = false;
        isDead = false;
        hp = data.Health;
        anim.SetBool("Dead", false);
        isFreeze = false;
        rb.isKinematic = false;
        coll.enabled = true;
        state = State.IDLE;
    }

    void Update()
    {
        if (isFreeze)
            rb.velocity = Vector3.zero;

        if (!isFreeze && !isDead)
            AttackLook();
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
                state = State.IDLE;
                yield return new WaitForSeconds(1f);
                cool = false;
            }

            //�÷��̾ ���ݰŸ� �ȿ� ������ ��
            if (!isDead)
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
                case State.ATTACK:
                    rb.velocity = Vector3.zero;
                    anim.SetTrigger("Attack");
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
        if (isStart)
        {
            hp -= damage;
            if (hp > 0)
                anim.SetTrigger("Hit");
            else
                StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
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
    public void attack()
    {
        GameObject bullet = GameManager.Instance.poolManager[2].Get(3);
        bullet.transform.position = pos.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(GameManager.Instance.playerTr.position);
    }
    public void particleOn()
    {
        par.Play();
    }
}
