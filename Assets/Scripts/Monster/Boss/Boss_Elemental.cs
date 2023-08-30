using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Elemental : MonoBehaviour, IDamage
{
    public BossData[] data;
    WaitForSeconds wait;
    Rigidbody rb;
    Animator anim;
    public GameObject pos;
    Boss_Elemental_Skill bossSkill;
    BossChange change;
    public GameObject changeAura;
    public GameObject[] spellAura;

    Vector3 SpellPosition;

    [SerializeField] float hp;
    float speed;
    int damage;
    [SerializeField] float move;
    [SerializeField] float moveSpeed;
    bool isDead;

    float attackDist;
    [HideInInspector]
    public int Type;
    float dist;

    float attackTime;
    float attackMaxTime;
    float mageTime;
    float mageMaxTime;

    [SerializeField] bool canAttack;
    [SerializeField] bool canMage;
    [SerializeField] bool canSpell;

    bool cool;
    bool isChanged;
    bool isSpellMove;
    bool isSpelling;
    [HideInInspector]
    public bool isCanMoveSpellPos;
    public bool isSpellPos;

    [SerializeField] float MaxHp;
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        MAGE,
        DEAD,
        HIT,
        CHANGE,
        SPELL,
        SPELLMOVE
    }
    [SerializeField] State state = State.IDLE;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.5f);
        bossSkill = GetComponent<Boss_Elemental_Skill>();
        change = GetComponent<BossChange>();
        SpellPosition = Vector3.zero;
        //Type = Random.Range(0, 5);
        Type = 3;
    }
    private void Start()
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
    }
    private void OnEnable()
    {
        isSpelling = false;
        isCanMoveSpellPos = false;
        isSpellPos = false;
        isSpellMove = false;
        canSpell = true;
        canMage = false;
        canAttack = false;
        isChanged = false;
        move = 0;
        isDead = false;
        hp = MaxHp;
        speed = data[Type].Speed;
        damage = data[Type].Damage;
        attackDist = data[Type].AttackDistance;
        anim.SetBool("Dead", false);
        attackMaxTime = data[Type].AttackTime;
        mageMaxTime = data[Type].CassTime;
        change.Change(Type);
    }

    void Update()
    {
        dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);
        moveSpeed = move * speed;
        if (isSpellMove)
            MoveSpellPos();
        else
            AttackLook();

        if (state == State.SPELLMOVE && Mathf.Abs(transform.localPosition.x) <= 0.1f && Mathf.Abs(transform.localPosition.z) <= 0.1f)
            isSpellPos = true;

        /*if(state == State.SPELL)
            transform.rotation = Quaternion.Euler(Vector3.zero);*/
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

            //ü�� ���� ���ϰ� �Ǹ� ����
            if (hp <= MaxHp / 2 && !isChanged)
            {
                state = State.CHANGE;
                isChanged = true;
                canSpell = true;
                isCanMoveSpellPos = false;
                isSpellPos = false;
            }
            //ü�� 70������ 60���̻��϶�, 30������ 20���̻��϶� ��ü����  
            else if (((hp / MaxHp * 100) <= 70 && (hp / MaxHp * 100) >= 60 && canSpell && !isSpellPos)
                || ((hp / MaxHp * 100) <= 30 && (hp / MaxHp * 100) >= 20) && canSpell && !isSpellPos)
            {
                state = State.SPELLMOVE;
                isSpellMove = true;
                isCanMoveSpellPos = true;
            }
            else if (state == State.SPELLMOVE && isSpellPos)
            {
                state = State.SPELL;
                isSpellMove = false;

                if (Type >= 3)
                {
                    yield return new WaitForSeconds(16f);
                }
                else
                    yield return new WaitForSeconds(22f);
                isSpelling = false;
                PlayerDamage playerDamage = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
                playerDamage.isSuper = false;
            }
            //���Ÿ� ����
            else if (dist > 5 && canMage)
            {
                state = State.MAGE;
                yield return new WaitForSeconds(5);
                yield return !canMage;
            }
            //����
            else if ((dist > 5 && !canMage) || (dist > attackDist && canAttack)) 
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
                    yield return !canMage;
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
                    anim.SetTrigger("Dead");
                    yield break;
                case State.SPELLMOVE:
                    StartCoroutine(Move());
                    anim.SetFloat("Move", move);
                    isSpellMove = true;
                    break;
                case State.SPELL:
                    move = 0;
                    if (canSpell)
                    {
                        canSpell = false;
                        anim.SetTrigger("Cast");
                    }
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
        if(!isSpelling)
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

    public IEnumerator casting()
    {
        if (state == State.SPELL)
        {
            if (Type >= 3)
                yield return new WaitForSeconds(4f);
            else
                yield return new WaitForSeconds(10f);
            anim.SetBool("Spelling", false);
            canSpell = false;
            OffSpellAura();
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
    public void useMage()
    {
        mageTime = 0f;
        canMage = false;
    }
    public void useAttack()
    {
        canAttack = false;
    }
    //�ִϸ��̼� �̺�Ʈ���� ȣ��
    //�� Ÿ�Կ� �´� ��ų ����
    public void Skill()
    {
        int num = 0;

        switch (Type)
        {
            case 0:
                num = 0;
                break;
            case 1:
                num = 1;
                break;
            case 2:
                num = 2;
                break;
            case 3:
                num = 3;
                break;
            case 4:
                /*num = Random.Range(4, 6);
                if (num == 4)
                    StartCoroutine(TimeCast());
                else*/
                    num = 6;
                break;
        }

        StartCoroutine(skill(num));
    }

    //��ų ���� �ڷ�ƾ
    IEnumerator skill(int num)
    {
        if (state == State.SPELL)
            yield break;

        GameObject skill = GameManager.Instance.poolManager[1].Get(num);
        skill.transform.position = pos.transform.position;
        anim.SetBool("NormalSpell", true);

        //��, ���� Ÿ���� BossBullet��ũ��Ʈ�� �����������ʾ� Break
        if (num == 3)
            yield break;

        BossBullet bullet = skill.GetComponent<BossBullet>();
        yield return new WaitForSeconds(2.5f);
        bullet.lookPlayer();
        bullet.Throw = true;
    }
    //���� Ÿ�Ը� ���
   /* IEnumerator TimeCast()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject Time = GameManager.Instance.poolManager[1].Get(5);
        Time.transform.position = GameManager.Instance.playerTr.position;
    }*/

    //�ӵ� ȸ�� �޼���
    IEnumerator RecoverySpeed()
    {
        yield return new WaitForSeconds(1f);
        while (speed <= data[Type].Speed)
        {
            speed += Time.deltaTime * 0.5f;
            yield return Time.deltaTime;
        }
        if (speed >= data[Type].Speed)
            speed = data[Type].Speed;
    }

    public void useSkill()
    {
        if (state == State.SPELL)
            bossSkill.TypeSkill(Type);
    }
    public IEnumerator normalCasting()
    {
        anim.SetBool("NormalSpell", false);
        yield return null;
    }

    public void HpChange()
    {
        int a = 5;
        while (a != Type)
        {
            a = Random.Range(0, 5);
            if (a != Type)
            {
                Type = a;
                break;
            }
        }
        change.Change(Type);
    }
    public void OnChangeAura()
    {
        changeAura.SetActive(true);
    }
    public void OnSpellAura()
    {
        spellAura[Type].SetActive(true);
    }
    void OffSpellAura()
    {
        spellAura[Type].SetActive(false);
    }
    void MoveSpellPos()
    {
        Vector3 pos = new Vector3(SpellPosition.x, transform.localPosition.y, SpellPosition.z);
        //Vector3 moveDirection = SpellPos.transform.position - transform.position;
        Vector3 moveDirection = pos - transform.localPosition;
        moveDirection.Normalize(); // ������ ����ȭ
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        rb.velocity = moveDirection * moveSpeed;
    }
    public void DontLookPlayer()
    {
        isSpelling = true;
    }

    public void getDamage(int damage)
    {
        /*//�ǰݵǸ� �ӵ� ����
        speed -= 0.5f;
        //�ӵ� ȸ�� �ڷ�ƾ
        StartCoroutine(RecoverySpeed());*/

        hp -= damage;
        print("���� HP" + hp); 
        
        if (hp <= 0)
        {
            state = State.DEAD;
            print("����");
        }
    }
}
