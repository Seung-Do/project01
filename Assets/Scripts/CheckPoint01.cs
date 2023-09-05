using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint01 : MonoBehaviour
{
    [SerializeField] private GameObject location01;
    [SerializeField] private GameObject location02;
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private float playerDistance;

    void Start()
    {
        StartCoroutine(Check());
    }
    void Update()
    {
        
    }
    IEnumerator Check()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerDistance);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
                {
                    location01.SetActive(false);
                    location02.SetActive(true);
                    foreach (GameObject monster in monsters)
                    {
                        monster.SetActive(true);
                    }
                    gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
