using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : MonoBehaviour
{
    public Material mat;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
        }
    }
}
