using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeleton : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetTrigger("damage");
    }
    
}
