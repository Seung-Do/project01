using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWitchFireball : MonoBehaviour
{
    Vector3 pos;
    void OnEnable()
    {
        pos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * 0.7f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Left Hand") || other.gameObject.layer == LayerMask.NameToLayer("Right Hand")))
        {
            //Debug.Log("몬스터 파이어볼 레이어" + other.gameObject.name);
            PlayerDamage damage = other.GetComponent<PlayerDamage>();
            if (damage != null)
            {
                gameObject.SetActive(false);
                transform.position = pos;
                damage.getDamage(20);
            }           
            gameObject.SetActive(false);
            transform.position = pos;
        }
    }
}