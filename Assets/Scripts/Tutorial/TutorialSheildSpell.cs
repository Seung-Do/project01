using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSheildSpell : TutorialBase
{
    public GameObject skeletonMageObject;
    public TutorialSkeletonMage skeletonMage;
    public GameObject canvas;
    public TMP_Text text;
    public GameObject rightControllerImage;

    void Start()
    {

    }
    public override void Enter()
    {
        canvas.SetActive(true);
        StartCoroutine(SpawnSkeleton());
        text.text = "마법 공격을 3번 막으세요!";
    }

    public override void Execute(TutorialController controller)
    {
        if (skeletonMage.defenseNumber == 3)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        rightControllerImage.SetActive(false);
        skeletonMageObject.SetActive(false);
        text.text = "";
    }

    private IEnumerator SpawnSkeleton()
    {
        yield return new WaitForSeconds(1.5f);
        skeletonMageObject.SetActive(true);
    }





}
