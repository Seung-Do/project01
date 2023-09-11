using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPortal : TutorialBase
{
    [SerializeField] private StageMovePortal stageMovePortal;

    public TMP_Text text;
    public GameObject portal;
    public override void Enter()
    {
        text.text = "이제 모험을 떠날 준비가 끝났습니다\r\n오른쪽에 포탈로 들어가면 모험이 시작됩니다";
        portal.SetActive(true);
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
