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
        //Debug.Log(transform.position.ToString());
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("MONSTER"))
            {

                Animator animator = collider.GetComponent<Animator>();
                collider.GetComponent<MonsterDamage>().hitNumber++;
                StartCoroutine(AnimatorSlow(animator));
            }
        }
    }
    IEnumerator AnimatorSlow(Animator animator)
    {      
        animator.speed = 0f;
        yield return waitTime;
        animator.speed = 1f;
    }
}
