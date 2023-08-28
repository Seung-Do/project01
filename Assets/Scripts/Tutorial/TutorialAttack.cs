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
        text.text = "<�ذ� ���縦 5�� �����ϼ���!>";
    }

    public override void Execute(TutorialController controller)
    {
        if(skeleton.hitNumber == 1)
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
