using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Weapon : MonoBehaviour
{
    [SerializeField] GameObject Weapon;
    Collider coll;
    Boss_SpiritDemon boss;
    void Start()
    {
        coll = Weapon.GetComponent<Collider>();   
        boss = GetComponent<Boss_SpiritDemon>();
    }
    
   public void EquipWeapon()
    {
        Weapon.SetActive(true);
    }
    public void UseWeapon()
    {
        coll.enabled = true;
    }
    public void NotUseWeapon()
    {
        coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerDamage damage = other.GetComponent<PlayerDamage>();
        if (damage != null)
        {
            print("АјАн");
            damage.getDamage(boss.damage);
        }
    }
}
