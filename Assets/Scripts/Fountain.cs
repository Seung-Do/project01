using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    public ParticleSystem buff;
    void Start()
    {      
        buff.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            buff.Play();
            other.gameObject.GetComponent<PlayerDamage>().getDamage(-100);
        }
    }    
                
}
