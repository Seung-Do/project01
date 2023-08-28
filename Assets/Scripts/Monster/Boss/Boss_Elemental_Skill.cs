using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Boss_Elemental_Skill : MonoBehaviour
{
    public GameObject[] spike;

    int ran;
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
                earth(ran);
                backearth();
                break;
            case 4:
                magic();
                break;
        }
    }
    public void fire()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(7);
        skill.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y / 2, GameManager.Instance.playerTr.position.z);
        skill.transform.parent = GameManager.Instance.playerTr;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(11);
    }

    public void ice()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(8);
        skill.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y / 2, GameManager.Instance.playerTr.position.z);
        skill.transform.parent = GameManager.Instance.playerTr;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(11);
    }

    public void thunder()
    {
        thunder2();
        thunder2();
    }
    void thunder2()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(9);
        skill.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y / 2, GameManager.Instance.playerTr.position.z);
        skill.transform.parent = GameManager.Instance.playerTr;
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
            StartCoroutine(frontearthSkill(i));
        }
    }
    public void backearth()
    {
        for (int i = 0; i < spike.Length; i++)
        {
            StartCoroutine(backearthSkill(i));
        }
    }

    public void magic()
    {
        GameObject skill = GameManager.Instance.poolManager[1].Get(11);
        skill.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y / 2, GameManager.Instance.playerTr.position.z);
        skill.transform.parent = GameManager.Instance.playerTr;
        GameObject Death = GameManager.Instance.poolManager[1].Get(12);
        //나중에 카메라위치로 변경
        Death.transform.position = new Vector3(GameManager.Instance.playerTr.position.x, GameManager.Instance.playerTr.position.y / 2 - 1, GameManager.Instance.playerTr.position.z);
        Death.transform.parent = GameManager.Instance.playerTr;
        Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
        skilloff.skilloff(7);
        Boss_Elemental_Skill_Off Deathskilloff = Death.GetComponent<Boss_Elemental_Skill_Off>();
        Deathskilloff.skilloff(7);
    }

    IEnumerator frontearthSkill(int num)
    {
        //발판 5개로 할 경우 9부터 시작 발판의 갯수를 다르게갈 경우 시작수 변경
        //발판 1개의 가로사이즈가 4이므로 1, 3으로 놔눠짐
        //다음 발판으로 넘어갈 경우 num에 * 4를 해줘 다음 발판으로 넘어감
        //int a = 9 - (num * 4);
        int a = 18 - (num * 6);
        for (int i = 1; i <= 21; i += 3)
        {
            GameObject skill = GameManager.Instance.poolManager[1].Get(10);
            skill.transform.parent = transform;
            skill.transform.localPosition = new Vector3(a, 0, i);
            Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
            skilloff.skilloff(5);

           /* if (i % 2 == 0)
            {
                GameObject Rskill = GameManager.Instance.poolManager[1].Get(10);
                Rskill.transform.parent = transform;
                Rskill.transform.localPosition = new Vector3(a, 0, i);
                Boss_Elemental_Skill_Off skilloff = Rskill.GetComponent<Boss_Elemental_Skill_Off>();
                skilloff.skilloff(5);
            }
           else
            {
                GameObject Lskill = GameManager.Instance.poolManager[1].Get(10);
                Lskill.transform.parent = transform;
                Lskill.transform.localPosition = new Vector3(a - 2, 0, i);
                Boss_Elemental_Skill_Off skilloff = Lskill.GetComponent<Boss_Elemental_Skill_Off>();
                skilloff.skilloff(5);
            }*/

            yield return new WaitForSeconds(0.07f);
        }
        foreach (var spikes in spike)
        {
            spikes.SetActive(false);
        }
    }
    IEnumerator backearthSkill(int num)
    {
        //발판 5개로 할 경우 9부터 시작 발판의 갯수를 다르게갈 경우 시작수 변경
        //발판 1개의 가로사이즈가 4이므로 1, 3으로 놔눠짐
        //다음 발판으로 넘어갈 경우 num에 * 4를 해줘 다음 발판으로 넘어감
        //int a = 9 - (num * 4);
        int a = 18 - (num * 6);
        for (int i = -1; i >= -21; i -= 3)
        {
            GameObject skill = GameManager.Instance.poolManager[1].Get(13);
            skill.transform.parent = transform;
            skill.transform.localPosition = new Vector3(a, 0, i);
            Boss_Elemental_Skill_Off skilloff = skill.GetComponent<Boss_Elemental_Skill_Off>();
            skilloff.skilloff(5);
            /* if (i % 2 == 0)
             {
                 GameObject Rskill = GameManager.Instance.poolManager[1].Get(13);
                 Rskill.transform.parent = transform;
                 Rskill.transform.localPosition = new Vector3(a, 0, i);
                 Boss_Elemental_Skill_Off skilloff = Rskill.GetComponent<Boss_Elemental_Skill_Off>();
                 skilloff.skilloff(5);
             }
             else
             {
                 GameObject Lskill = GameManager.Instance.poolManager[1].Get(13);
                 Lskill.transform.parent = transform;
                 Lskill.transform.localPosition = new Vector3(a - 2, 0, i);
                 Boss_Elemental_Skill_Off skilloff = Lskill.GetComponent<Boss_Elemental_Skill_Off>();
                 skilloff.skilloff(5);
             }*/

            yield return new WaitForSeconds(0.07f);
        }
        foreach (var spikes in spike)
        {
            spikes.SetActive(false);
        }
    }

    public void rand(int num)
    {
        ran = num;
        StartCoroutine(SpikeWaring(num));
    }
    IEnumerator SpikeWaring(int a)
    {
        yield return new WaitForSeconds(5);
        spike[a].SetActive(true);
    }
}
