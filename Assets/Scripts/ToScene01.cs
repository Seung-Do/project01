using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToScene01 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            ControllManager controllManager = other.gameObject.GetComponent<ControllManager>();
            if (controllManager.lightningPosible)
                GameManager.Instance.Stage1Load();
        }
    }
}
