using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBoss : MonoBehaviour
{
    public Boss_Elemental boss;
    void Start()
    {
        GameManager.Instance.elemental = boss;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
