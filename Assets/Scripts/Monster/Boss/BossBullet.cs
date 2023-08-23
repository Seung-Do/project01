using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public bool Throw;
    private void Start()
    {
        StartCoroutine(Off());
    }
    private void OnEnable()
    {
        Throw = false;
    }
    void Update()
    {
        if (Throw)
            transform.Translate(Vector3.forward * Time.deltaTime * 7);
    }
    private void OnTriggerEnter(Collider other)
    {
        //gameObject.SetActive(false);
    }
    public void lookPlayer()
    {
        transform.LookAt(GameManager.Instance.testPlayer.transform.position);
    }
    IEnumerator Off()
    {
        //2초후 총알 비활성화
        yield return new WaitForSeconds(8f);
        gameObject.SetActive(false);
    }
}
