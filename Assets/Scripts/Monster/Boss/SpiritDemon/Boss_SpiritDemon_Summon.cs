using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Summon : MonoBehaviour
{
    Boss_SpiritDemon boss;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    //Vector3 pos5;
    //Vector3 pos6;
    Vector3 pos7;
    Vector3 rot;
    public List<GameObject> summonList;
    private void Awake()
    {
        boss = GetComponent<Boss_SpiritDemon>();
    }
    private void Start()
    {
        pos1 = new Vector3(7.5f, -3.5f, 83);
        pos2 = new Vector3(-2f, -3.5f, 83);
        pos3 = new Vector3(7.5f, -3.5f, 81);
        pos4 = new Vector3(-2f, -3.5f, 81);
        //pos5 = new Vector3(-2, 2, 86.5f);
        //pos6 = new Vector3(7.5f, 2, 86.5f);
        pos7 = new Vector3(2.6f, -4.9f, 72.5f);
        rot = new Vector3(0, 180, 0);
    }
    private void Update()
    {
        if (!boss.isAllDead && summonList.Count == 0 && !boss.isChange)
        {
            if (boss.summonInt > 3)
                boss.isOver = true;
            else
                boss.isAllDead = true;
        }
    }
    public void SummonWarriorZombie1()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(1);
        zombie1.transform.position = pos1;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(zombie1.transform.position);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(0);
        zombie2.transform.position = pos2;
        zombie2.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(zombie2.transform.position);

        //summonList에 추가
        summonList.Add(zombie1);
        summonList.Add(zombie2);

        //소환된 몬스터에게 Boss_SpiritDemon_Summon 추가
        Boss_SpiritDemon_Zombie summon1 = zombie1.GetComponent<Boss_SpiritDemon_Zombie>();
        summon1.summon = GetComponent<Boss_SpiritDemon_Summon>();
        Boss_SpiritDemon_Zombie summon2 = zombie2.GetComponent<Boss_SpiritDemon_Zombie>();
        summon2.summon = GetComponent<Boss_SpiritDemon_Summon>();
    }
    public void SummonWarriorZombie2()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(1);
        zombie1.transform.position = pos3;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(pos3);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(0);
        zombie2.transform.position = pos4;
        zombie2.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(pos4);

        //summonList에 추가
        summonList.Add(zombie1);
        summonList.Add(zombie2);

        //소환된 몬스터에게 Boss_SpiritDemon_Summon 추가
        Boss_SpiritDemon_Zombie summon1 = zombie1.GetComponent<Boss_SpiritDemon_Zombie>();
        summon1.summon = GetComponent<Boss_SpiritDemon_Summon>();
        Boss_SpiritDemon_Zombie summon2 = zombie2.GetComponent<Boss_SpiritDemon_Zombie>();
        summon2.summon = GetComponent<Boss_SpiritDemon_Summon>();
    }
    public void SummonMageZombie()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(2);
        zombie1.transform.position = pos1;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(pos1);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(2);
        zombie2.transform.position = pos2;
        zombie2.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(pos2);

        //summonList에 추가
        summonList.Add(zombie1);
        summonList.Add(zombie2);

        //소환된 몬스터에게 Boss_SpiritDemon_Summon 추가
        Boss_SpiritDemon_Zombie_Mage summon1 = zombie1.GetComponent<Boss_SpiritDemon_Zombie_Mage>();
        summon1.summon = GetComponent<Boss_SpiritDemon_Summon>();
        Boss_SpiritDemon_Zombie_Mage summon2 = zombie2.GetComponent<Boss_SpiritDemon_Zombie_Mage>();
        summon2.summon = GetComponent<Boss_SpiritDemon_Summon>();
    }
    public void SummonGolem()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(6);
        zombie1.transform.position = pos7;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        SummonFxOn(pos7);

        //summonList에 추가
        summonList.Add(zombie1);

        //소환된 몬스터에게 Boss_SpiritDemon_Summon 추가
        Boss_SpiritDemon_Golem summon1 = zombie1.GetComponent<Boss_SpiritDemon_Golem>();
        summon1.summon = GetComponent<Boss_SpiritDemon_Summon>();
    }
    public void SummonWarriorZombie()
    {
        SummonWarriorZombie1();
        SummonWarriorZombie2();
    }

    void SummonFxOn(Vector3 pos)
    {
        GameObject fx = GameManager.Instance.poolManager[2].Get(4);
        fx.transform.position = pos + Vector3.up * 0.1f;
        StartCoroutine(SummonFxOff(fx));
    }
    IEnumerator SummonFxOff(GameObject fx)
    {
        yield return new WaitForSeconds(5);
        fx.SetActive(false);
    }
    public void RemoveList(GameObject obj)
    {
        summonList.Remove(obj);
    }
}
