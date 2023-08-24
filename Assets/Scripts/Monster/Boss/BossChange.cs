using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : MonoBehaviour
{
    public Material[] mat;
    public Boss_Elemental boss;
    int num;

    private void Start()
    {
        num = boss.Type;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = mat[num];
    }
    void Update()
    {
        num = boss.Type;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = mat[num];
        }
    }
}
