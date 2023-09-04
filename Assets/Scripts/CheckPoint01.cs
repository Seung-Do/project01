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
        
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.playerTr.position);
        if (distance <= playerDistance)
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

   
}
