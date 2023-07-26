using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    private float interactRadius = 5.6f;
    private WaitForSeconds waitTime = new WaitForSeconds(0.7f);
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
        Debug.Log(transform.position.ToString());
        foreach (Collider collider in colliders)
        {
            if(collider.CompareTag("MONSTER"))
            {             
               Animator animator = collider.GetComponent<Animator>();             
               StartCoroutine(AnimatorSlow(animator));           
            }
        }
    }
    IEnumerator AnimatorSlow(Animator animator)
    {
        yield return waitTime;
        animator.SetTrigger("damage");
        animator.speed = 0.5f;
        yield return waitTime;
        animator.speed = 1f;
      
    }
}
