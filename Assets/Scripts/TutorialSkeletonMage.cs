using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeletonMage : MonoBehaviour
{
    public Animator animator;
    private WaitForSeconds attackInterval = new WaitForSeconds(3f);
    public int attackCount =0;
    void Start()
    {
        
    }
        
    void OnEnable()
    {
        // �ڷ�ƾ�� �����Ͽ� ���� �ð����� ������ �����մϴ�.
       StartCoroutine(AttackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
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
            Attack();
            yield return attackInterval;
            attackCount++;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
    }
}
