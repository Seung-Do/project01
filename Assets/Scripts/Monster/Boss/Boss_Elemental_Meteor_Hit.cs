using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Elemental_Meteor_Hit : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Off(2));
    }
    IEnumerator Off(float num)
    {
        yield return new WaitForSeconds(num);
        gameObject.SetActive(false);
    }
}
