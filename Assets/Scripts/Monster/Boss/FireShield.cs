using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour, IDamage
{
   [SerializeField] GameObject shield;
    public void getDamage(int damage)
    {
        print("�ν���");
        shield.SetActive(false);
        PlayerDamage playerDamage = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        playerDamage.isSuper = true;
    }
}
