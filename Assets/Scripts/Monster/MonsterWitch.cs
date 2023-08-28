using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWitch : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public ParticleSystem fire;
    public GameObject fireBall;

    float speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        speed = 7f;
    }

    private void Update()
    {
        lookPlayer();
    }

    void lookPlayer()
    {
        Vector3 dir = GameManager.Instance.playerTr.position - fire.transform.position + Vector3.up * 0.8f;
        fire.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void fireballAttack()
    {
        GameObject fireball = GameManager.Instance.poolManager[0].Get(0);
        fireball.transform.position = fire.transform.position;
        fireball.transform.rotation = fire.transform.rotation;
    }
}