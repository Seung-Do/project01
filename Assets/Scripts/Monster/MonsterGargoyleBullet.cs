using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        PlayerDamage damage = other.GetComponent<PlayerDamage>();
        if (damage != null)
        {
            damage.getDamage(20);
            gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    public void lookPlayer()
    {
        transform.LookAt(GameManager.Instance.playerTr.position+Vector3.up*0.8f);
    }
    void GrowUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, maxScale, Time.deltaTime * 10);
    }
}
