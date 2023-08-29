using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    [SerializeField] private float radius;
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

                IDamage damage = collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.getDamage(100);
                }
            }
        }
    }
}