using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGargoyleBullet : MonoBehaviour
{
    public bool Throw;
    private void OnEnable()
    {
        Throw = false;
    }
    void Update()
    {
        if(Throw)
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }
    private void OnParticleTrigger()
    {
        gameObject.SetActive(false);
    }
    public void lookPlayer()
    {
        transform.rotation = Quaternion.LookRotation(GameManager.Instance.testPlayer.transform.position);
    }
}
