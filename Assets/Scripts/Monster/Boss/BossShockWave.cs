using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockWave : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        shockWave();
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }    
    void shockWave()
    {
        GameObject Shock = GameManager.Instance.poolManager[1].Get(14);
        Shock.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, 0, GameManager.Instance.playerTr.position.z);
    }
}
