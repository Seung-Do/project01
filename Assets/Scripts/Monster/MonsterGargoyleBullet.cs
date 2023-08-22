using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGargoyleBullet : MonoBehaviour
{
    public bool Throw;
    Vector3 maxScale;
    private void Start()
    {
        maxScale = new Vector3(2, 2, 2);
    }
    private void OnEnable()
    {
        Throw = false;
        transform.localScale = Vector3.zero;
    }
    void Update()
    {
        GrowUp();
        if(Throw)
            transform.Translate(Vector3.forward * Time.deltaTime * 7);
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
    public void lookPlayer()
    {
        transform.LookAt(GameManager.Instance.testPlayer.transform.position);
    }
    void GrowUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, maxScale, Time.deltaTime * 10);
    }
}
