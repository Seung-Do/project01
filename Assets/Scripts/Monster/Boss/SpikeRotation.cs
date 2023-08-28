using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3 (transform.localPosition.x, -1, transform.localPosition.z);
    }
}
