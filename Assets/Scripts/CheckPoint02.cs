using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CheckPoint02 : MonoBehaviour
{
    [SerializeField] private GameObject location02;
    [SerializeField] private GameObject boss;
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
                int enemyCount = 0;
                int health = 0;

                if (collider.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
                    enemyCount++;

                if (collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
                {
                    health = collider.gameObject.GetComponent<PlayerDamage>().hp;
                    location02.SetActive(false);
                }

                if (enemyCount == 0 && health == 100)
                {
                    GameManager.Instance.bossTime.SetActive(true);
                    yield return new WaitForSeconds(3);
                    GameManager.Instance.Boss0Load();
                    yield return new WaitForSeconds(3);
                    boss.SetActive(true);
                    GameManager.Instance.bossTime.SetActive(false);
                    gameObject.SetActive(false);
                }

            }
            yield return new WaitForSeconds(1);
        }
    }

}
