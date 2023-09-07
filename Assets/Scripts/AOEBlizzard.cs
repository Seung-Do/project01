using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AOEBlizzard : MonoBehaviour
{
    [SerializeField] private float radius;
    private WaitForSeconds wait = new WaitForSeconds (2f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY") || collider.gameObject.layer == LayerMask.NameToLayer("BOSS"))
            {
                Animator animator = collider.gameObject.GetComponent<Animator>();
                NavMeshAgent agent = collider.GetComponent<NavMeshAgent>();
                StartCoroutine(SlowDownAnimation(animator));
                StartCoroutine(SlowDownAgent(agent));
                IDamage damage = collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    StartCoroutine(DamageWait(damage));
                }
            }
        }
    }
    IEnumerator DamageWait(IDamage damage)
    {
        yield return wait;
        damage.getDamage(100);
    }
    /* private IEnumerator ChangeAnimatorSpeed(Animator animator, float duration)
     {
         float elapsedTime = 0;
         while (elapsedTime < duration)
         {
             float t = elapsedTime / duration;

             float newSpeed = Mathf.Lerp(0.2f, 0.7f, t);
             animator.speed = newSpeed;

             elapsedTime += Time.deltaTime;
             yield return null;
         }

         animator.speed = 1f; 
     }*/
    private IEnumerator SlowDownAnimation(Animator animator)
    {
        animator.speed = 0.5f;
        yield return wait;
        animator.speed = 1f;
    }
    private IEnumerator SlowDownAgent(NavMeshAgent agent)
    {
        agent.speed /= 2f;
        yield return wait;
        agent.speed *= 2f;
    }

}
