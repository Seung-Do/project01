using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Zombie", menuName = "Scripts Object/ZombieData")]
public class Boss_SpiritDemon_Zombie_Data : ScriptableObject
{
    [Header("# Main Info")]
    public float Health;    //ü��
    public int Damage;    //���� ������

    [Header("# Attack Info")]
    public float AttackDistance;    //���� ��Ÿ�

    [Header("# Sound")]
    public float Sound;
}