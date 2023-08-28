using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeletonMage : MonoBehaviour
{
    public Animator animator;
    public GameObject electroMagic;
    public Transform MagicSpawn;
    Transform playerTr;
    Transform monsterTr;
    private WaitForSeconds attackInterval = new WaitForSeconds(3f);
    private WaitForSeconds aniDelay = new WaitForSeconds(0.5f);
    public int attackCount =0;
    readonly float damping = 10f;
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>(); 
        monsterTr = GetComponent<Transform>();
    }
        
    void OnEnable()
    {
        // �ڷ�ƾ�� �����Ͽ� ���� �ð����� ������ �����մϴ�.
       StartCoroutine(AttackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.LookRotation(playerTr.position - monsterTr.position);
        monsterTr.rotation = Quaternion.Slerp(monsterTr.rotation, rot, Time.deltaTime * damping);

        if (attackCount >= 3)
        {
            StopAllCoroutines();
            //Debug.Log("�ڷ�ƾ����");
        }

    }
    IEnumerator AttackCoroutine()
    {
        while (attackCount < 3) 
        {
            yield return attackInterval;
            animator.SetTrigger("attack");
            yield return aniDelay;
            Attack();
            yield return attackInterval;
            attackCount++;
        }
    }

    private void Attack()
    {
       
        GameObject magic = Instantiate(electroMagic, MagicSpawn.position, MagicSpawn.rotation);
        Destroy(magic, 1.5f);
    }
}
