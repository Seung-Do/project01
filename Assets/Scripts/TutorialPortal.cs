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
        text.text = "���� ��Ż�� ���� ������ ���۵˴ϴ�";
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
