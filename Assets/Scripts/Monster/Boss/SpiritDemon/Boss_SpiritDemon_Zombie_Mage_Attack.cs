using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Zombie_Mage_Attack : MonoBehaviour
{
    ParticleSystem par;
    bool isThrow;
    private void Awake()
    {
        par = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        StartCoroutine(Throw());
    }
    void Update()
    {
        if (!isThrow) 
        {
            Vector3 dir = GameManager.Instance.playerTr.position - transform.position + Vector3.up * 0.8f;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else
            transform.Translate(Vector3.forward * Time.deltaTime * 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerDamage damage = other.GetComponent<PlayerDamage>();
        if (damage != null)
        {
            damage.getDamage(20);
            gameObject.SetActive(false);
        }
        isThrow = false;
        gameObject.SetActive(false);
    }
    IEnumerator Throw()
    {
        par.Play();
        yield return new WaitForSeconds(0.1f);
        isThrow = true;
    }
}
