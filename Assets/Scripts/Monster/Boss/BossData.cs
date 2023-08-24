using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "Scripts Object/BossData")]
public class BossData : ScriptableObject
{
    [Header("# Main Info")]
    public int BossType;

    [Header("# Boss Data")]
    //public float Health;    //ü��
    public float Speed;     //�̵� �ӵ�
    public float Damage;    //���� ������
    public float AttackDistance;    //���� ��Ÿ�
    public float CassTime;  //��ų ��Ÿ��
    public float AttackTime; // ���� ��Ÿ��

    [Header("# Sound")]
    public float Sound;
}
