using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamage : MonoBehaviour
{
    private float interactRadius = 0.6f;
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

                IFreeze freeze = collider.GetComponent<IFreeze>();
                if (freeze != null)
                {

                    IDamage damage = collider.gameObject.GetComponent<IDamage>();
                    if (damage != null)
                    {
                        StartCoroutine(AnimatorSlowMonster(animator, freeze, damage));
                    }
                }
                else
                {
                    IDamage damage = collider.gameObject.GetComponent<IDamage>();
                    if (damage != null)
                    {
                        damage.getDamage(50);
                    }
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
    IEnumerator AnimatorSlowMonster(Animator animator, IFreeze freeze, IDamage damage)
    {
        animator.speed = 0f;
        freeze.IFreeze();
        print("¾ó¸²");
        damage.getDamage(50);
        yield return waitTime;
        animator.speed = 1f;
        freeze.IFreeze();
    }
    IEnumerator AnimatorSlow(Animator animator)
    {
        animator.speed = 0f;
        yield return waitTime;
        animator.speed = 1f;
    }

}
