using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpellPosition : MonoBehaviour
{
    public Boss_Elemental boss;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MONSTER"))
        {
            boss.isSpellPos = true;
            print("½ºÆç À§Ä¡");
        }
    }
}
