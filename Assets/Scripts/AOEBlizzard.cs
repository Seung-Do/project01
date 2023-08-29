using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            {
                Animator animator = collider.gameObject.GetComponent<Animator>();
                StartCoroutine(ChangeAnimatorSpeed(animator,2f));   
                IDamage damage = collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    StartCoroutine(DamageWait(damage, wait));
                }
            }
        }
    }
    IEnumerator DamageWait(IDamage damage, WaitForSeconds wait)
    {
        yield return wait;
        damage.getDamage(100);
    }
    private IEnumerator ChangeAnimatorSpeed(Animator animator, float duration)
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
    }

}
