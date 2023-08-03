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
    private void OnParticleCollision(GameObject other)
    {
        moveNext = true;
    }
}
