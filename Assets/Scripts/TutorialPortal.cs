using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPortal : TutorialBase
{
    [SerializeField] private StageMovePortal stageMovePortal;

    public GameObject canvas;
    public TMP_Text text;
    public override void Enter()
    {
        text.text = "이제 포탈로 들어가면 모험이 시작됩니다";
    }

    public override void Execute(TutorialController controller)
    {
        if(stageMovePortal.moveNext)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        
    }

}
