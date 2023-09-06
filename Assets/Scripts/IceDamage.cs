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
                    IFreeze monster = collider.GetComponent<IFreeze>();
                    if (monster != null)
                        StartCoroutine(AnimatorSlowMonster(animator, monster));
                    else
                        StartCoroutine(AnimatorSlow(animator));
                }
                
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("BOSS"))
            {
                IDamage damage = collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.getDamage(50);
                }
            }
        }
    }
    IEnumerator AnimatorSlowMonster(Animator animator, IFreeze monster)
    {      
        animator.speed = 0f;
        monster.IFreeze();
        yield return waitTime;
        animator.speed = 1f;
        monster.IFreeze();
    }
    IEnumerator AnimatorSlow(Animator animator)
    {
        animator.speed = 0f;
        yield return waitTime;
        animator.speed = 1f;
    }
       
}
