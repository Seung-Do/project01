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
    [SerializeField] private Fountain fountain;
    private bool isMove;
    

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
            if (!isMove)
            {
                if (enemyCount == 0 && (fountain.isTouched || health == 100))
                {
                    Debug.Log("보스존으로 이동");
                    Debug.Log(fountain.isTouched);
                    Debug.Log(health);
                    GameManager.Instance.bossTime.SetActive(true);
                    yield return new WaitForSeconds(3);
                    GameManager.Instance.Boss0Load();
                    yield return new WaitForSeconds(3);
                    boss.SetActive(true);
                    GameManager.Instance.bossTime.SetActive(false);

                    location02.SetActive(false);
                    isMove = true;
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}
