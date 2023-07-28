using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float damage = 20f;
    float speed = 1000f;
    float destroyDistance = 20f;

    Rigidbody rb;
    Transform tr;
    Transform rightControllerTr;
    public ParticleSystem collisonEffect;
    public ParticleSystem fireEffect;




    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        rightControllerTr = GameObject.Find("Right Controller").GetComponent<Transform>();         
    }

    private void OnEnable()
    {
        Vector3 playerDirection = Camera.main.transform.forward;

        // Y 축 회전을 고려하지 않도록 설정
        playerDirection.y = 0f;

        // 정규화하여 방향 벡터로 변환
        playerDirection.Normalize();

       
        rb.AddForce(playerDirection * speed);
        collisonEffect.Stop();
        fireEffect.Play();
      

    }
    private void OnDisable()
    {
        rb.Sleep();      
    }
    void Update()
    {
       
        Vector3 startPosition = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        fireEffect.Stop();
        collisonEffect.Play();
        if (collision.gameObject.CompareTag("MONSTER"))
        {
           collision.gameObject.GetComponent<MonsterDamage>().hitNumber++;
            Animator animator = collision.gameObject.GetComponent<Animator>();
            animator.SetTrigger("damage");
        }
        Destroy(gameObject,1f);
    }
}
