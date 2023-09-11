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
        text.text = "���� ������ ���� �غ� �������ϴ�\r\n�����ʿ� ��Ż�� ���� ������ ���۵˴ϴ�";
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
