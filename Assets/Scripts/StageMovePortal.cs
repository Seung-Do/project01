using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMovePortal : MonoBehaviour
{
    public bool moveNext= false;    
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
            moveNext = true;
    }
}
