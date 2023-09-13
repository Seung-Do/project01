using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            GameManager.Instance.GameClear();
            print("클리어 포탈 진입");
        }
    }
}
