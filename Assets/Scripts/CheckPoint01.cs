using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint01 : MonoBehaviour
{
    [SerializeField] private GameObject location01;
    [SerializeField] private GameObject location02;
    [SerializeField] private GameObject checkPoint02;
    [SerializeField] private GameObject monsters;
    [SerializeField] private float playerDistance;
    private float dist;
    void Start()
    {
        StartCoroutine(Distance());
        StartCoroutine(Check());
    }
    void Update()
    {

    }
    /* IEnumerator Check()
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
                     checkPoint02.SetActive(true);
                     foreach (GameObject monster in monsters)
                     {
                         monster.SetActive(true);
                     }
                     gameObject.SetActive(false);
                 }
             }
             yield return new WaitForSeconds(1);
         }
     }*/
    IEnumerator Check()
    {
        while (true)
        {
            if (dist <= playerDistance)
            {
                location01.SetActive(false);
                location02.SetActive(true);
                             
                if (checkPoint02 != null)
                {
                    checkPoint02.SetActive(true);
                    monsters.SetActive(true);
                }
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Distance()
    {
        while (true)
        {
            dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
