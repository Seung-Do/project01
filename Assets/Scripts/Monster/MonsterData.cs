using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scripts Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("# Main Info")]
    public int MonsterType;  //�����Ǵ� ���� Ÿ��
    //0 : Boar
    //1 : Stag
    //2 : Spider
    //3 : KingCobra
    //4 : Shield
    //5 : Witch

    [Header("# Monster Data")]
    public float Health;    //ü��
    public float Speed;     //�̵� �ӵ�
    public int Damage;    //���� ������
    public float AttackDistance;    //���� ��Ÿ�
    public float ViewRange; //�߰� ����

    [Header("# Sound")]
    public float Sound;
}
