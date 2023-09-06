using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    public ParticleSystem buff;
    public ParticleSystem flow01;
    public ParticleSystem flow02;
    private WaitForSeconds wait= new WaitForSeconds(3f);
    public bool isTouched;
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
        if (other.gameObject.CompareTag("PLAYER") && !isTouched)
        {
            isTouched = true;
            buff.Play();
            other.gameObject.GetComponent<PlayerDamage>().getDamage(-100);
            flow01.Stop();
            flow02.Stop();
        }
    }    
      
}
