using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternObject : MonoBehaviour, IDamage
{
    public void getDamage(int damage)
    {
        PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        player.isSuper = true;
        gameObject.SetActive(false);
    }
}
