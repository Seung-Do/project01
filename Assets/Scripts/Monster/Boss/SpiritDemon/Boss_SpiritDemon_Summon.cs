using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiritDemon_Summon : MonoBehaviour
{
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    Vector3 pos5;
    Vector3 pos6;
    Vector3 rot;
    private void Start()
    {
        pos1 = new Vector3(7.5f, -3.5f, 83);
        pos2 = new Vector3(-2f, -3.5f, 83);
        pos3 = new Vector3(7.5f, -3.5f, 81);
        pos4 = new Vector3(-2f, -3.5f, 81);
        pos5 = new Vector3(-2, 2, 86.5f);
        pos6 = new Vector3(7.5f, 2, 86.5f);
        rot = new Vector3(0, 180, 0);
    }
    public void SummonWarriorZombie1()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(0);
        zombie1.transform.position = pos1;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(0);
        zombie2.transform.position = pos2;
        zombie2.transform.localRotation = Quaternion.Euler(rot);

        print("@@@@ 근거리 1 소환" + zombie1.gameObject.name);
        print("@@@@ 근거리 1 소환" + zombie2.gameObject.name);
    }
    public void SummonWarriorZombie2()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(1);
        zombie1.transform.position = pos3;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(1);
        zombie2.transform.position = pos4;
        zombie2.transform.localRotation = Quaternion.Euler(rot);
        print("@@@@ 근거리 2 소환" + zombie1.gameObject.name);
        print("@@@@ 근거리 2 소환" + zombie2.gameObject.name);
    }
    public void SummonMageZombie()
    {
        GameObject zombie1 = GameManager.Instance.poolManager[2].Get(2);
        zombie1.transform.position = pos5;
        zombie1.transform.localRotation = Quaternion.Euler(rot);
        GameObject zombie2 = GameManager.Instance.poolManager[2].Get(2);
        zombie2.transform.position = pos6;
        zombie2.transform.localRotation = Quaternion.Euler(rot);
        print("@@@@ 원거리 소환" + zombie1.gameObject.name);
        print("@@@@ 원거리 소환" + zombie2.gameObject.name);
    }

    public void SummonWarriorZombie()
    {
        SummonWarriorZombie1();
        SummonWarriorZombie2();
    }
}
