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

                FreezableMonster monster = collider.GetComponent<FreezableMonster>();
                if (monster != null)
                {

                    IDamage damage = collider.gameObject.GetComponent<IDamage>();
                    if (damage != null)
                    {
                        StartCoroutine(AnimatorSlowMonster(animator, monster, damage));
                    }
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
    IEnumerator AnimatorSlowMonster(Animator animator, FreezableMonster monster, IDamage damage)
    {
        animator.speed = 0f;
        monster.IFreeze();
        print("¾ó¸²");     
        yield return waitTime;
        animator.speed = 1f;
        damage.getDamage(50);
        monster.IFreeze();
    }

}
