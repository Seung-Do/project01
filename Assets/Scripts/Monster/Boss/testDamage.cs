using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamage : MonoBehaviour
{
    [SerializeField] int dam;
    private void OnTriggerEnter(Collider other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        if(damage != null )
        {
            damage.getDamage(dam);
        }
    }
}
