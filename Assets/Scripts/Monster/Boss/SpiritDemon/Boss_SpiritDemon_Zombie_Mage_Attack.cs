using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Zombie_Mage_Attack : MonoBehaviour
{
   
    void Start()
    {
        transform.LookAt(GameManager.Instance.playerTr.position);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerDamage damage = other.GetComponent<PlayerDamage>();
        if(damage != null )
        {
            damage.getDamage(20);
        }
        gameObject.SetActive(false);
    }
}
