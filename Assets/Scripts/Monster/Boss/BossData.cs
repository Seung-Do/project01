using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "Scripts Object/BossData")]
public class BossData : ScriptableObject
{
    [Header("# Main Info")]
    public int BossType;

    [Header("# Boss Data")]
    //public float Health;    //체력
    public float Speed;     //이동 속도
    public float Damage;    //공격 데미지
    public float AttackDistance;    //공격 사거리
    public float CassTime;  //스킬 쿨타임
    public float AttackTime; // 근접 쿨타임

    [Header("# Sound")]
    public float Sound;
}
