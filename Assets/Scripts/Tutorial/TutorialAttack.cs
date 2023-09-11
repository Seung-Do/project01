using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialAttack : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
 
    public GameObject skeletonObject;
    public GameObject rightControllerImage;
    public TutorialSkeleton skeleton;

    public override void Enter()
    {
        skeletonObject.SetActive(true); 
        //GameObject skeletonMon = Instantiate(skeletonObject, new Vector3( -2.5f, 0.11f, 12.5f ), Quaternion.Euler(0, 180f, 0));
     
        canvas.SetActive(true);
        text.text = "<해골 병사를 3번 공격하세요!>";
    }

    public override void Execute(TutorialController controller)
    {
        if(skeleton.hitNumber == 3)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        StartCoroutine(SkeletonOut());
        rightControllerImage.SetActive(false);
        text.text = "";
    }

    IEnumerator SkeletonOut()
    {
        yield return new WaitForSeconds(1f);
        
        skeletonObject.SetActive(false);
    }
   
}
