using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour, IDamage
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void getDamage(int damage)
    {
        Debug.Log("방패로 막음");
    }
}
