using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShield : MonoBehaviour
{
    public GameObject shield;

    void Update()
    {
        PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        if(player.isSuper)
            shield.SetActive(true);
        else
            shield.SetActive(false);
    }
}
