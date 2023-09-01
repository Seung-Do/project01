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
        transform.Translate(Vector3.forward * 0.7f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Left Hand") || other.gameObject.layer == LayerMask.NameToLayer("Right Hand")))
        {
            //Debug.Log("���� ���̾ ���̾�" + other.gameObject.name);
            PlayerDamage damage = other.GetComponent<PlayerDamage>();
            if (damage != null)
            {
                damage.getDamage(20);
                gameObject.SetActive(false);
            }
            
            gameObject.SetActive(false);
        }
    }
}