using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Elemental_Skill_Off : MonoBehaviour
{
    void Start()
    {
    }
    IEnumerator Off(float num)
    {
        yield return new WaitForSeconds(num);
        gameObject.SetActive(false);
        print("��ų ��Ȱ��ȭ");
    }
    public void skilloff(float num)
    {
        StartCoroutine(Off(num));
    }
}
