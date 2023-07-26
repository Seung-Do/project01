using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDamage : MonoBehaviour
{
    private bool isDamage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("MONSTER") && !isDamage)
        {
            Animator animator = other.GetComponent<Animator>();
            animator.SetTrigger("damage");
            StartCoroutine(AnimatorSlow(animator));
            isDamage = true;
        }
    }
    IEnumerator AnimatorSlow(Animator animator)
    {
        animator.speed = 0.2f;
        yield return new WaitForSeconds(0.6f);
        animator.speed = 1f;
        isDamage = false;
    }
}
