using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawnList = new List<GameObject>(); //�ν�����â���� �޾ƿ� ���� ���� ����Ʈ
    bool isSpawn = false;   //��ȯ ����
    public float spawnDistance; //��ȯ�� �Ÿ�
    [SerializeField] float dist; //�÷��̾���� �Ÿ�
    private void OnEnable()
    {
        StartCoroutine(Distance());
    }

    void Update()
    {
        //���� �Ÿ��� �ǰ� ��ȯ������ ������ ����Ʈ�� �ִ� ���͵� Ȱ��ȭ
        if(dist <= spawnDistance && !isSpawn)
        {
            foreach(GameObject spawn in spawnList) 
            {
                spawn.SetActive(true);
            }
            //��ȯ�� ���͵� ���� Ȱ��ȭ�Ǹ� isSpawn = Ture
            isSpawn = true;
        }
        //���Ͱ� ��ȯ�� �Ǿ����� ���Ͱ� �׾����� �ƴ��� �Ǵ�
        if(isSpawn)
        {
            if(spawnList.Count == 0)
                gameObject.SetActive(false);
            foreach (GameObject spawn in spawnList)
            {
                Monster monster = spawn.GetComponent<Monster>();
                Monster_Witch Witch = spawn.GetComponent<Monster_Witch>();
                //��ȯ�� ���Ͱ� Monster��ũ��Ʈ�� ������ ������
                if (monster != null)
                {
                    if (monster.state == Monster.State.DEAD)
                        spawnList.Remove(spawn);
                }
                //��ȯ�� ���Ͱ� Monster_Witch ��ũ��Ʈ�� ������ ������
                else if (Witch != null)
                {
                    if (Witch.state == Monster_Witch.State.DEAD)
                        spawnList.Remove(spawn);
                }
            }
        }
    }
    IEnumerator Distance()
    {
        //���Ͱ� ��ȯ�Ǹ� ����
        while (!isSpawn)
        {
            //1�� �������� �÷��̾���� �Ÿ� ����
            dist = Vector3.Distance(GameManager.Instance.playerTr.position, transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
