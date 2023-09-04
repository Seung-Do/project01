using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpiritDemon", menuName = "Scripts Object/SpiritDemonData")]
public class Boss_SpiritDemon_Data : ScriptableObject
{
    [Header("# Main Info")]
    public float Health;    //체력
    public int Damage;    //공격 데미지
    public float CassTime;  //스킬 쿨타임

    [Header("# Attack Info")]
    public float AttackDistance;    //공격 사거리
    public float AttackTime; // 근접 쿨타임

    [Header("# SpecialAttack Info")]
    public float SpecialAttackDistance;    //특수스킬 사거리
    public float SpecialAttackTime; // 특수스킬 쿨타임

    [Header("# DashAttack Info")]
    public float DashAttackDistance;    //대쉬스킬 사거리
    public float DashAttackTime; // 대쉬스킬 쿨타임

    [Header("# Summon Info")]
    public float SummonTime; // 소환 쿨타임

    [Header("# Sound")]
    public float Sound;
}