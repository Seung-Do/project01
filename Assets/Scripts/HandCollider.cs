using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    GameObject teleportInteractor;
    private void OnEnable()
    {
        teleportInteractor = GameObject.Find("Teleport Interactor").gameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        teleportInteractor.SetActive(false);
        Debug.Log("¼Õ¿¡ ´ê¾ÒÀ½");
    }

}
