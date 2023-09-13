using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGuardian : MonoBehaviour
{
    Rigidbody rb;
    public GameObject firePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lookPlayer();
    }

    void lookPlayer()
    {
        Vector3 dir = GameManager.Instance.playerTr.position - firePos.transform.position + Vector3.up * 0.8f;
        firePos.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void fireballAttack()
    {
        GameObject fireball = GameManager.Instance.poolManager[0].Get(1);
        //GameObject fireball = Instantiate(fireBall);
        fireball.transform.position = firePos.transform.position;
        fireball.transform.rotation = firePos.transform.rotation;
    }
}
