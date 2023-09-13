using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShield : MonoBehaviour
{
    public GameObject shield;
    PlayerDamage player;
    private void Start()
    {
        player = GetComponent<PlayerDamage>();
    }
    void Update()
    {
        shield.SetActive(player.isSuper);
    }
}
