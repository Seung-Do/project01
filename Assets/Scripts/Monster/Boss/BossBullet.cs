using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public bool Throw;
    [SerializeField] private float speed;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        Throw = false;
        StartCoroutine(Off(8));
    }
    void Update()
    {
        if (Throw)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        //StartCoroutine(Off(1));
        Debug.Log("�����ҷ��� ���� : "+other.gameObject.name);
        IDamage damage = other.GetComponent<IDamage>();
        if(damage != null)
            damage.getDamage(20);
        gameObject.SetActive(false);
    }
    public void lookPlayer()
    {
        transform.LookAt(GameManager.Instance.playerTr.position + Vector3.up);
    }
    IEnumerator Off(float num)
    {
        //2���� �Ѿ� ��Ȱ��ȭ
        yield return new WaitForSeconds(num);
        gameObject.SetActive(false);
    }
}
