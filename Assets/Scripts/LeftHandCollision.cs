using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandCollision : MonoBehaviour
{
    public GameObject teleportInteractor;
 
    private void OnCollisionStay(Collision collision)
    {
        teleportInteractor.SetActive(false);
        //Debug.Log("�޼��� ��� �������");
    }

    private void OnCollisionExit(Collision collision)
    {
        teleportInteractor.SetActive(true);
    }
}
