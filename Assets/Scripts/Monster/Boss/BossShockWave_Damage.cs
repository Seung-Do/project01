using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockWave_Damage : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        Wave();
    }
    void Wave()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4);

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
}
