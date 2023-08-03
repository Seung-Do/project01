using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialReadySheild : TutorialBase
{
    public GameObject canvas;
    public Transform playerTr;
    public TMP_Text text;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        maker.SetActive(true);
        text.text = "3��° ������ ���߸����� ������ ���� ���������� �÷��̾��� ü���� �� ĭ �Ҹ��ϰ� ü���� ��ĭ ������ ���� ������� ���մϴ�\r\n���� ü���� �Ҹ��ߴٸ� ���ʿ� ���̴� ������ ������ ü���� ȸ���� �� �ֽ��ϴ�\r\n�غ� �Ǹ� �����ʿ� ��Ÿ�� ǥ�� �������� �̵��� �ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > 2f && playerTr.position.x < 3f) && (playerTr.position.z < 4.5f && playerTr.position.z > 3.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
