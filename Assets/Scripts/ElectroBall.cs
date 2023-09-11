using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBall : MonoBehaviour
{
    Rigidbody rb;
    float speed = 1000f;

    public ParticleSystem collisonEffect;
    public ParticleSystem electroEffect;
    public TutorialSkeletonMage skeletonMage;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        collisonEffect.Stop();
        electroEffect.Play();
        skeletonMage = GameObject.FindWithTag("MONSTER").GetComponent<TutorialSkeletonMage>();
    }
    private void OnDisable()
    {
        //tr.position = Vector3.zero;
        //tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        electroEffect.Stop();
        collisonEffect.Play();
        Destroy(gameObject, 0.6f);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        electroEffect.Stop();
        collisonEffect.Play();
        if (other.gameObject.CompareTag("SHIELD"))
        {
            skeletonMage.defenseNumber++;
            Destroy(gameObject, 0.5f);
        }
        else
            Destroy(gameObject, 0.5f);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
