using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Zombie", menuName = "Scripts Object/ZombieData")]
public class Boss_SpiritDemon_Zombie_Data : ScriptableObject
{
    [Header("# Main Info")]
    public float Health;    //체력
    public int Damage;    //공격 데미지

    [Header("# Attack Info")]
    public float AttackDistance;    //공격 사거리

    [Header("# Sound")]
    public float Sound;
}