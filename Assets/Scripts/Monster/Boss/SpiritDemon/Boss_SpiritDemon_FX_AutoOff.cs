using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_FX_AutoOff : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(off());
    }

    IEnumerator off()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}

