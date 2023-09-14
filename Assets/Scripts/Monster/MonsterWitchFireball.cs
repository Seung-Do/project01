using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWitchFireball : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private float speed;
    [SerializeField] private int damageSize;
    void OnEnable()
    {
        pos = transform.position;
        StartCoroutine(Off());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Left Hand") || other.gameObject.layer == LayerMask.NameToLayer("Right Hand")))
        {
            //Debug.Log("몬스터 파이어볼 레이어" + other.gameObject.name);
            PlayerDamage damage = other.GetComponent<PlayerDamage>();
            if (damage != null)
            {
                gameObject.SetActive(false);
                transform.position = pos;
                damage.getDamage(damageSize);
            }           
            gameObject.SetActive(false);
            transform.position = pos;
        }
    }
    IEnumerator Off()
    {
        //2초후 총알 비활성화
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        transform.position = pos;
    }
}