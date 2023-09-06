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

    //반경 2안에 있는 콜라이더들 중 PlayerDamage를 가진 콜라이더만 데미지 받음
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
    //n초뒤 비활성화
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
