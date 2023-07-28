using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBall : MonoBehaviour
{
    Rigidbody rb;
    Transform tr;

    float damage = 20f;
    float speed = 1000f;


    public ParticleSystem collisonEffect;
    public ParticleSystem electroEffect;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        collisonEffect.Stop();
        electroEffect.Play();
    }
    private void OnDisable()
    {
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        electroEffect.Stop();
        collisonEffect.Play();
        Destroy(gameObject, 0.6f);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
