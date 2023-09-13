using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            //보스 잡고 생성된 포탈에서 이동하는 코드
            print("클리어 포탈 진입");
        }
    }
}
