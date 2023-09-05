using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class CheckPoint02 : MonoBehaviour
{
    [SerializeField] private GameObject location02;
    [SerializeField] private GameObject location03;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject lightningText;
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
            int enemyCount = 0;
            int health = 0;
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerDistance);
            foreach (Collider collider in colliders)
            {             
                if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
                    enemyCount++;

                if (collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
                {
                    location02.SetActive(false);
                    health = collider.gameObject.GetComponent<PlayerDamage>().hp;
                    ControllManager controllManager = collider.gameObject.GetComponent<ControllManager>();
                    if(controllManager.lightningPosible) 
                    {
                        location03.SetActive(true);
                        lightningText.SetActive(true);
                        gameObject.SetActive(false);
                    }                  
                }            
            }
            //Debug.Log(enemyCount);
            if (enemyCount == 0 && health == 100)
            {
                GameManager.Instance.bossTime.SetActive(true);
                yield return new WaitForSeconds(3);
                GameManager.Instance.Boss0Load();
                yield return new WaitForSeconds(3);
                boss.SetActive(true);
                GameManager.Instance.bossTime.SetActive(false);
            }
            yield return new WaitForSeconds(1);
        }
    }

}
