using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject boss;
    //[SerializeField] RotateDoor door;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("PLAYER"))
        {
            boss.SetActive(true);
            GameManager.Instance.DoorClose();
           // door.ChangeDoor();
            gameObject.SetActive(false);
        }
    }
}
