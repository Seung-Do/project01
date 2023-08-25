using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGargoyleMeteor : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.7f);
        MeteorHit();
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    void MeteorHit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        foreach (Collider collider in colliders)
        {
            GameObject hit = collider.gameObject;
            IDamage damage = hit.GetComponent<IDamage>();

            if(damage != null)
            {
                damage.getDamage(20);
            }
        }
    }
    private void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = Color.yellow;

        // ��ü ����� �׸��� (���� ���� ǥ��)
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
