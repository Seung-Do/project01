using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockWave : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(1);
        Wave();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }    

    void Wave()
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
}
