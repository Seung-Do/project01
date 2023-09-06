using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMageAttack : MonoBehaviour
{
    [SerializeField] GameObject fire;
    private void Update()
    {
        lookPlayer();
    }

    void lookPlayer()
    {
        Vector3 dir = GameManager.Instance.playerTr.position - fire.transform.position + Vector3.up * 0.8f;
        fire.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void attack()
    {
        GameObject fireball = GameManager.Instance.poolManager[2].Get(3);
        fireball.transform.position = fire.transform.position;
        fireball.transform.rotation = fire.transform.rotation;
    }
}
