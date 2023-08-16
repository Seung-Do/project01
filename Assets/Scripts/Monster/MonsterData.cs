using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scripts Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("# Main Info")]
    public int MonsterType;  //�����Ǵ� ���� Ÿ��

    [Header("# Monster Data")]
    public float Health;    //ü��
    public float Speed;     //�̵� �ӵ�
    public float Damage;    //���� ������
    public float AttackDistance;    //���� ��Ÿ�
    public float ViewRange; //�߰� ����

    [Header("# Sound")]
    public float Sound;
}
