using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderShield : MonoBehaviour
{
    private void Start()
    {
        PlayerDamage playerDamage = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        playerDamage.isSuper = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerDamage>() != null)
        {
            PlayerDamage playerDamage = other.GetComponent<PlayerDamage>();
            playerDamage.isSuper = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerDamage>() != null)
        {
            PlayerDamage playerDamage = other.GetComponent<PlayerDamage>();
            playerDamage.isSuper = false;
        }
    }
    private void OnDisable()
    {
        PlayerDamage playerDamage = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        playerDamage.isSuper = false;
    }
}
