using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : MonoBehaviour
{
    public Material[] mat;
    public Boss_Elemental boss;

    public void Change(int num)
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = mat[num];
    }
}
