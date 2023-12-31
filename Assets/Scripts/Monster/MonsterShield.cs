using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShield : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    Monster monster;

    float dashTime;
    float dashMaxTime = 10f;
    float dashPower = 5f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        monster = GetComponent<Monster>();
    }
    private void OnEnable()
    {
        dashTime = 10f;
    }
    public void ShieldAttack()
    {
        anim.SetTrigger("Attack");
    }
    public void ShieldDash()
    {
        if(dashTime > dashMaxTime)
        {
            anim.SetTrigger("Dash");
            rb.AddForce(Vector3.forward * dashPower, ForceMode.Impulse);
            dashTime = 0;
        }
    }

    private void Update()
    {
        dashTime += Time.deltaTime;
        if((int) monster.state == 3)
        {
            rb.useGravity = true;   
        }

    }
}
