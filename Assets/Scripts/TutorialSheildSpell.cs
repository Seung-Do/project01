using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSheildSpell : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject rightControllerImage;

    void Start()
    {

    }
    public override void Enter()
    {
       
        text.text = "3��° ������ ���߸����� ������ ���� ���������� �÷��̾��� ü���� �� ĭ �Ҹ��ϰ� ü���� ��ĭ ������ ���� ������� ���մϴ�\r\n���� ü���� �Ҹ��ߴٸ� ���ʿ� ���̴� ������ ������ ü���� ȸ���� �� �ֽ��ϴ�\r\n�غ� �Ǹ� �����ʿ� ��Ÿ�� ǥ�� �������� �̵��� �ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
       
    }

    
    

    
}
