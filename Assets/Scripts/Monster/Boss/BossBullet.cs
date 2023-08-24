using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public bool Throw;
    private void Start()
    {
        StartCoroutine(Off(8));
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
        StartCoroutine(Off(1));
        IDamage damage = other.GetComponent<IDamage>();
        if(damage != null)
            damage.getDamage();
    }
    public void lookPlayer()
    {
        transform.LookAt(GameManager.Instance.testPlayer.transform.position);
    }
    IEnumerator Off(float num)
    {
        //2초후 총알 비활성화
        yield return new WaitForSeconds(num);
        gameObject.SetActive(false);
    }
}
