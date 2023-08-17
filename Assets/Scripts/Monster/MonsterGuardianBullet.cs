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
        Vector3 dir = GameManager.Instance.testPlayer.transform.position - transform.position;
        transform.localRotation = Quaternion.LookRotation(dir);
        transform.Translate(Vector3.forward * 1f);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("PLAYER"))
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator Off()
    {
        //2���� �Ѿ� ��Ȱ��ȭ
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
