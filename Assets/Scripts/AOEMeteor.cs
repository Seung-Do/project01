using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMeteor : MonoBehaviour
{
    [SerializeField] private float radius;
    private WaitForSeconds wait = new WaitForSeconds(1f);
    public Color sphereColor = Color.red;
    void Start()
    {

    }

    
    void Update()
    {

    }
    void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * 5f, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY") || collider.gameObject.layer == LayerMask.NameToLayer("BOSS"))
            {
                Animator animator = collider.gameObject.GetComponent<Animator>();

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
}
