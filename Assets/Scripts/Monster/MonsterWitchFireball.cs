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
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Left Hand") || other.gameObject.layer == LayerMask.NameToLayer("Right Hand")))
        {
            //Debug.Log("몬스터 파이어볼 레이어" + other.gameObject.name);
            IDamage damage = other.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.getDamage(20);
                gameObject.SetActive(false);
            }
            
            gameObject.SetActive(false);
        }
    }
}