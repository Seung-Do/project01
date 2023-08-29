using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDamage : MonoBehaviour
{
    private bool isDamage = false;
    private WaitForSeconds waitTime = new WaitForSeconds(0.7f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*void OnParticleCollision(GameObject other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            isDamage = true;
            Animator animator = other.GetComponent<Animator>();
            IDamage damage = other.gameObject.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.getDamage(50);
                StartCoroutine(AnimatorSlow(animator));
            }         
            
        }
    }*/
   

    IEnumerator AnimatorSlow(Animator animator)
    {
        animator.speed = 0.3f;
        yield return waitTime;
        animator.speed = 1f;
        isDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ENEMY") && !isDamage)
        {
            isDamage = true;
            Animator animator = other.GetComponent<Animator>();
            IDamage damage = other.gameObject.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.getDamage(50);
                StartCoroutine(AnimatorSlow(animator));
            }
        }
    }
}
