using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : MonoBehaviour
{
    private float interactRadius = 0.3f;
    private WaitForSeconds waitTime = new WaitForSeconds(5f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRadius);
       
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            {
                Animator animator = collider.GetComponent<Animator>();
      
                IDamage damage = collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.getDamage(50);
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                        StartCoroutine(AnimatorSlowMonster(animator, monster));
                    else
                        StartCoroutine(AnimatorSlow(animator));
                }
                
            }
        }
    }
    IEnumerator AnimatorSlowMonster(Animator animator, Monster monster)
    {      
        animator.speed = 0f;
        monster.isFreeze = true;
        yield return waitTime;
        animator.speed = 1f;
        monster.isFreeze = false;   
    }
    IEnumerator AnimatorSlow(Animator animator)
    {
        animator.speed = 0f;
        yield return waitTime;
        animator.speed = 1f;
    }
       
}
