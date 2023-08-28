using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternObject : MonoBehaviour, IDamage
{
    public void getDamage(int damage)
    {
        testPlayer player = GameManager.Instance.playerTr.GetComponent<testPlayer>();
        player.shield.SetActive(true);
        Destroy(gameObject);
    }
}
