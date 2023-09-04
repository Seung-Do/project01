using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
       transform.position = new Vector3(GameManager.Instance.playerTr.position.x, 50, GameManager.Instance.playerTr.position.z);
    }
}
