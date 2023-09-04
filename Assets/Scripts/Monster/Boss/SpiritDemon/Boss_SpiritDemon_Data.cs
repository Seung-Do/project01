using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpiritDemon", menuName = "Scripts Object/SpiritDemonData")]
public class Boss_SpiritDemon_Data : ScriptableObject
{
    [Header("# Main Info")]
    public float Health;    //ü��
    public int Damage;    //���� ������
    public float CassTime;  //��ų ��Ÿ��

    [Header("# Attack Info")]
    public float AttackDistance;    //���� ��Ÿ�
    public float AttackTime; // ���� ��Ÿ��

    [Header("# SpecialAttack Info")]
    public float SpecialAttackDistance;    //Ư����ų ��Ÿ�
    public float SpecialAttackTime; // Ư����ų ��Ÿ��

    [Header("# DashAttack Info")]
    public float DashAttackDistance;    //�뽬��ų ��Ÿ�
    public float DashAttackTime; // �뽬��ų ��Ÿ��

    [Header("# Summon Info")]
    public float SummonTime; // ��ȯ ��Ÿ��

    [Header("# Sound")]
    public float Sound;
}