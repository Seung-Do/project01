using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] m_SpawnObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            gameObject.SetActive(false);
            foreach (GameObject obj in m_SpawnObject)
            {
                obj.SetActive(true);
            }
            print("스폰포인트 활성화");
            
        }
    }
}
