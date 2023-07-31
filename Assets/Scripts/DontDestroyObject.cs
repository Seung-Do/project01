using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private static DontDestroyObject Instance = null;
    private void OnEnable()
    {
       
        DontDestroyOnLoad(gameObject);

    }
}
