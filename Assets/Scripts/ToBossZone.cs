using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBossZone : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            GameManager.Instance.Boss0Load();          
        }
    }
}
