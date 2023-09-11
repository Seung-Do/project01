using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialHeal : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        maker.SetActive(true);
        text.text = "���� ������ ���������� �÷��̾��� ü���� �� ĭ �Ҹ��ϰ� ü���� ��ĭ ������ ���� ������� ���մϴ�\r\n���� ü���� �Ҹ��ߴٸ� ���ʿ� ���̴� ������ ������ ü���� ȸ���� �� �ֽ��ϴ�\r\n�غ� �Ǹ� �����ʿ� ��Ÿ�� ǥ�� �������� �̵��� �ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        if ((GameManager.Instance.playerTr.position.x > 2f && GameManager.Instance.playerTr.position.x < 3f) && (GameManager.Instance.playerTr.position.z < 1.5f && GameManager.Instance.playerTr.position.z > 0.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
