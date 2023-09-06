using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Golem_JumpAttack : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(attack());
        StartCoroutine(Off());
    }

    //�ݰ� 2�ȿ� �ִ� �ݶ��̴��� �� PlayerDamage�� ���� �ݶ��̴��� ������ ����
    void JumpAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        foreach (Collider collider in colliders)
        {
            GameObject hit = collider.gameObject;
            PlayerDamage damage = hit.GetComponent<PlayerDamage>();

            if (damage != null)
            {
                damage.getDamage(20);
            }
        }
    }
    //n�ʵ� ��Ȱ��ȭ
    IEnumerator Off()
    {
        yield return new  WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.1f);
        JumpAttack();
    }
}
