using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Elemental_Skill_Damage : MonoBehaviour
{
    [SerializeField] int Type;
    private void Awake()
    {
        Type = GameManager.Instance.elemental.Type;
    }
    private void Start()
    {
        TypeDamage();
    }
    void TypeDamage()
    {
        switch (Type)
        {
            case 0:
                Hit(10);
                break;
            case 1:
                StartCoroutine(Ice(30));
                break;
            case 2:
                StartCoroutine(Thunder(30));
                break;
            case 3:
                Hit(20);
                break;
            case 4:
                StartCoroutine(Death(70));
                break;
        }
    }
    void Hit(int num)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        foreach (Collider collider in colliders)
        {
            GameObject hit = collider.gameObject;
            PlayerDamage damage = hit.GetComponent<PlayerDamage>();

            if (damage != null)
            {
                damage.getDamage(num);
            }
        }
    }
    IEnumerator Ice(int num)
    {
        int a = 0;
        while (a <= 4)
        {
            a++;
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(num);
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator Thunder(int num)
    {
        int a = 0;
        while(a <= 4)
        {
            a++;
            PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
            player.getDamage(num);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Death(int num)
    {
        yield return new WaitForSeconds(3.3f);
        PlayerDamage player = GameManager.Instance.playerTr.GetComponent<PlayerDamage>();
        player.getDamage(num);
    }
}
