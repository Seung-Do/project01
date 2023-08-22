using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scripts Object/BossData")]
public class BossData : ScriptableObject
{
    [Header("# Main Info")]
    public int BossType;

    [Header("# Monster Data")]
    public float Health;    //ü��
    public float Speed;     //�̵� �ӵ�
    public float Damage;    //���� ������
    public float AttackDistance;    //���� ��Ÿ�

    [Header("# Sound")]
    public float Sound;
}
