using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            //���� ��� ������ ��Ż���� �̵��ϴ� �ڵ�
            print("Ŭ���� ��Ż ����");
        }
    }
}
