using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWitchFireball : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.forward * 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.getDamage(1);
            gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}