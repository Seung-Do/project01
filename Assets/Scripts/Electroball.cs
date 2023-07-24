using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electroball : MonoBehaviour
{
    float damage = 30f;
    float speed = 600f;
    float destroyDistance = 20f;

    Rigidbody rb;
    Transform tr;
    ParticleSystem electroEffect;
    Vector3 startPosition;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        electroEffect = GetComponentInChildren<ParticleSystem>();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        Vector3 playerDirection = Camera.main.transform.forward;

        // Y �� ȸ���� ������� �ʵ��� ����
        playerDirection.y = 0f;

        // ����ȭ�Ͽ� ���� ���ͷ� ��ȯ
        playerDirection.Normalize();

       
        rb.AddForce(playerDirection * speed);

        electroEffect.Play();

    }
    private void OnDisable()
    {

        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
        electroEffect.Stop();
    }
    void Update()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
