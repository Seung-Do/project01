using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterGuardianBullet : MonoBehaviour
{
    ParticleSystem particle;
    private void Start()
    {
        StartCoroutine(Off());
    }
    void Update()
    {
        Vector3 dir = GameManager.Instance.playerTr.position - transform.position + Vector3.up * 0.8f;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.Translate(Vector3.forward * 0.7f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Left Hand") || other.gameObject.layer == LayerMask.NameToLayer("Right Hand")))
        {
            //Debug.Log("몬스터 파이어볼 레이어" + other.gameObject.name);
            IDamage damage = other.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.getDamage(20);
                gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
        }
    }
    IEnumerator Off()
    {
        //2초후 총알 비활성화
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
