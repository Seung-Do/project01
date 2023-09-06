using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRune : MonoBehaviour
{
    private Vector3 newPosition = new Vector3(2.5f, 20f, -17.5f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PLAYER"))
        {
            other.gameObject.transform.position = newPosition;
            GameManager.Instance.handsTr.position = newPosition;
        }
    }
}
