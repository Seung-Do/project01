using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scripts Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("# Main Info")]
    public int MonsterType;  //스폰되는 몬스터 타입
    //0 : Boar
    //1 : Stag
    //2 : Spider
    //3 : KingCobra
    //4 : Shield
    //5 : Witch

    [Header("# Monster Data")]
    public float Health;    //체력
    public float Speed;     //이동 속도
    public int Damage;    //공격 데미지
    public float AttackDistance;    //공격 사거리
    public float ViewRange; //발각 범위

    [Header("# Sound")]
    public float Sound;
}
