using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialAttack : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
 
    public GameObject skeletonObject;
    MonsterDamage damage;
    public override void Enter()
    {
        skeletonObject.SetActive(true); 
        //GameObject skeletonMon = Instantiate(skeletonObject, new Vector3( -2.5f, 0.11f, 12.5f ), Quaternion.Euler(0, 180f, 0));
        damage = skeletonObject.GetComponent<MonsterDamage>();
        canvas.SetActive(true);
        text.text = "<�ذ� ���縦 5�� �����ϼ���!>";
    }

    public override void Execute(TutorialController controller)
    {
        if(damage.hitNumber == 5)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        StartCoroutine(SkeletonOut());
    }

    IEnumerator SkeletonOut()
    {
        yield return new WaitForSeconds(1f);
        
        skeletonObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
