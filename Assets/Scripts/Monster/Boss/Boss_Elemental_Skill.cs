using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Boss_Elemental_Skill : MonoBehaviour
{
    public GameObject[] spike;
    public void TypeSkill(int num)
    {
        switch (num)
        {
            case 0:
                fire();
                break;
            case 1:
                ice();
                break;
            case 2:
                thunder();
                break;
            case 3:
                int a = Random.Range(0, spike.Length);
                earth(a);
                spike[a].SetActive(true);
                break;
            case 4:
                magic();
                break;
        }
    }
    public void fire()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(7);
        skill.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x, GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z);
        skill.transform.parent = GameManager.Instance.testPlayer.transform;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(11);
    }

    public void ice()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(8);
        skill.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x, GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z);
        skill.transform.parent = GameManager.Instance.testPlayer.transform;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(11);
    }

    public void thunder()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(9);
        skill.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x, GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z);
        skill.transform.parent = GameManager.Instance.testPlayer.transform;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(10);
    }

    public void earth(int num)
    {
        int a = num;
        for(int i = 0; i < spike.Length; i++) 
        {
            if(i == a)
                continue;
            StartCoroutine(earthSkill(i));
        }
    }

    public void magic()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(11);
        skill.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x, GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z);
        skill.transform.parent = GameManager.Instance.testPlayer.transform;
        GameObject Death = GameManager.Instance.poolManager[1].Get(12);
        //���߿� ī�޶���ġ�� ����
        Death.transform.position = new Vector3(GameManager.Instance.testPlayer.transform.position.x, GameManager.Instance.testPlayer.transform.position.y / 2, GameManager.Instance.testPlayer.transform.position.z);
        Death.transform.parent = GameManager.Instance.testPlayer.transform;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(7);
        Boss_Elemental_Skill_Off Deathskilloff = Death.GetComponent<Boss_Elemental_Skill_Off>();
        Deathskilloff.skilloff(7);
    }

    IEnumerator earthSkill(int num)
    {
        //���� 5���� �� ��� 9���� ���� ������ ������ �ٸ��԰� ��� ���ۼ� ����
        //���� 1���� ���λ���� 4�̹Ƿ� 1, 3���� ������
        //���� �������� �Ѿ ��� num�� * 4�� ���� ���� �������� �Ѿ
        int a = 9 - (num * 4);
        for(int i = 1; i <= 20; i++)
        {
            if(i % 2 == 0)
            {
                GameObject Rskill = GameManager.Instance.poolManager[1].Get(10);
                Rskill.transform.parent = transform;
                Rskill.transform.localPosition = new Vector3(a, 0, i);
            }
           else
            {
                GameObject Lskill = GameManager.Instance.poolManager[1].Get(10);
                Lskill.transform.parent = transform;
                Lskill.transform.localPosition = new Vector3(a - 2, 0, i);
            }

            yield return new WaitForSeconds(0.1f);
        }
        foreach(var spikes in spike)
        {
            spikes.SetActive(false);
        }
    }
}
