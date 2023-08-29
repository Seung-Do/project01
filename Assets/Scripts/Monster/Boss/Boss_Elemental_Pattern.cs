using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Elemental_Pattern : MonoBehaviour
{
    Boss_Elemental boss;
    Boss_Elemental_Skill bossSkill;
    [SerializeField] GameObject fireShield;
    [SerializeField] GameObject PatternObject;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject magicShield;
    [SerializeField] GameObject DivineShine;
    int Type;

    bool isPattern;
    private void Start()
    {
        boss = GetComponent<Boss_Elemental>();
        bossSkill = GetComponent<Boss_Elemental_Skill>();
    }
    private void OnEnable()
    {
        isPattern = true;
    }

    private void Update()
    {
        Type = boss.Type;
    }
    public void Pattern()
    {
        if (isPattern)
        {
            print("패턴 시작");
            switch (Type)
            {
                case 0:
                    StartCoroutine(shield());
                    break;
                case 1:
                    SpawnPatternObject();
                    break;
                case 2:
                    StartCoroutine(SakuraShield());
                    break;
                case 3:
                    RandomSpike();
                    break;
                case 4:
                    StartCoroutine(MagicShield());
                    break;
            }
        }
        isPattern = false;
    }

    public void patternOn()
    {
        isPattern = true;
    }
    IEnumerator shield()
    {
        fireShield.SetActive(true);
        yield return new WaitForSeconds(20);
        fireShield.SetActive(false);
    }
    void RandomSpike()
    {
        int a = Random.Range(0, 7);
        bossSkill.rand(a);
    }
    void SpawnPatternObject()
    {
        int a = Random.Range(0, 2);
        int b = Random.Range(0, 2);
        if (a == 0)
            a = -1;
        if (b == 0)
            b = -1;
        PatternObject.transform.localPosition = new Vector3(Random.Range(1f, 15f) * a, 2, Random.Range(1f, 15f) * b);
        PatternObject.SetActive(true);
    }
    IEnumerator SakuraShield()
    {
        print("시작");
        int a = Random.Range(0, 2);
        int b = Random.Range(0, 2);
        if (a == 0)
            a = -1;
        if (b == 0)
            b = -1;
        Shield.transform.localPosition = new Vector3(Random.Range(1f, 15) * a, 0, Random.Range(1f, 15) * b);
        Shield.SetActive(true);
        yield return new WaitForSeconds(22f);
        Shield.SetActive(false);
    }

    IEnumerator MagicShield()
    {
        int a = Random.Range(0, 2);
        int b = Random.Range(0, 2);
        if (a == 0)
            a = -1;
        if (b == 0)
            b = -1;
        Vector3 pos = new Vector3(Random.Range(1f, 15) * a, 0, Random.Range(1f, 15) * b);
        DivineShine.transform.localPosition = pos;
        DivineShine.SetActive(true);
        yield return new WaitForSeconds(3f);
        magicShield.transform.localPosition = pos;
        magicShield.SetActive(true);
        yield return new WaitForSeconds(6f);
        DivineShine.SetActive(false);
        Collider coll = magicShield.GetComponent<Collider>();
        coll.enabled = false;
    }
}
