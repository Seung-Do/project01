using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWitchFireball : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.forward * 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
