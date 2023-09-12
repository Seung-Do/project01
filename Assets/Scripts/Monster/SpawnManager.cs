using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawnList = new List<GameObject>(); //인스펙터창에서 받아올 몬스터 스폰 리스트
    bool isSpawn = false;   //소환 유무
    public float spawnDistance; //소환할 거리
    [SerializeField] float dist; //플레이어와의 거리
    int spawnCount = 0;
    private void OnEnable()
    {
        StartCoroutine(Distance());
        spawnCount = spawnList.Count;
    }

    void Update()
    {
        //일정 거리가 되고 소환한적이 없으면 리스트에 있는 몬스터들 활성화
        if(dist <= spawnDistance && !isSpawn)
        {
            foreach(GameObject spawn in spawnList) 
            {
               /* Monster monster = spawn.GetComponent<Monster>();
                Monster_Witch Witch = spawn.GetComponent<Monster_Witch>();
                //소환된 몬스터가 Monster스크립트를 가지고 있으면
                if (monster != null)
                {
                    monster.spawn = GetComponent<SpawnManager>();
                }
                //소환된 몬스터가 Monster_Witch 스크립트를 가지고 있으면
                else if (Witch != null)
                {
                    Witch.spawn = GetComponent<SpawnManager>();
                }*/

                spawn.SetActive(true);
            }
            //소환될 몬스터들 전부 활성화되면 isSpawn = Ture
            isSpawn = true;
        }
        //몬스터가 소환이 되었으면 몬스터가 죽었는지 아닌지 판단
        if(isSpawn)
        {
            if (spawnCount == 0)
                StartCoroutine(off());

            /*foreach (GameObject spawn in spawnList)
            {
                Monster monster = spawn.GetComponent<Monster>();
                Monster_Witch Witch = spawn.GetComponent<Monster_Witch>();
                //소환된 몬스터가 Monster스크립트를 가지고 있으면
                if (monster != null)
                {
                    if (monster.state == Monster.State.DEAD)
                        spawnList.Remove(spawn);
                }
                //소환된 몬스터가 Monster_Witch 스크립트를 가지고 있으면
                else if (Witch != null)
                {
                    if (Witch.state == Monster_Witch.State.DEAD)
                        spawnList.Remove(spawn);
                }
            }*/
            foreach (GameObject spawn in spawnList)
            {
                Monster monster = spawn.GetComponent<Monster>();
                Monster_Witch Witch = spawn.GetComponent<Monster_Witch>();
                //소환된 몬스터가 Monster스크립트를 가지고 있으면
                if (monster != null)
                {
                    if (monster.state == Monster.State.DEAD)
                        spawnCount--;
                }
                //소환된 몬스터가 Monster_Witch 스크립트를 가지고 있으면
                else if (Witch != null)
                {
                    if (Witch.state == Monster_Witch.State.DEAD)
                        spawnCount--;
                }
            }
        }
    }
    IEnumerator Distance()
    {
        //몬스터가 소환되면 멈춤
        while (!isSpawn)
        {
            //1초 간격으로 플레이어와의 거리 측정
            dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator off()
    {
        spawnList.Clear();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
