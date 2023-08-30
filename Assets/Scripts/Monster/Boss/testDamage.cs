using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamage : MonoBehaviour
{
    int Type = 3;
    private void OnTriggerEnter(Collider other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        if(damage != null )
        {
            damage.getDamage(500);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HpChange();
        }
    }
    public void HpChange()
    {
        bool isSame;
        isSame = true;
        int a = 5;
        while (isSame)
        {
            a = Random.Range(0, 5);
            print("변신 번호" + a);
            if (a != Type)
            {
                Type = a;
                isSame = false;
                break;
            }
        }
    }
}
