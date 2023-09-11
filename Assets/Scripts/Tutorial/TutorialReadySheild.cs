using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialReadySheild : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject rightControllerImage;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        canvas.SetActive(true);
        rightControllerImage.SetActive(true);
        maker.SetActive(true);
        text.text = "������ ��Ʈ�ѷ��� A��ư�� ������ ��(��)�� �׸��� ��ư�� ���� ���� ���а� ��ȯ�˴ϴ�\r\n���� ���ݸ� ���� �� �ְ� ���� ������ ���� ���ϹǷ� �����ϼ���\r\n���� ������ ���� �غ� �Ǹ� �տ� ��Ÿ�� ǥ�� �������� �̵��� �ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        if ((GameManager.Instance.playerTr.position.x > 2f && GameManager.Instance.playerTr.position.x < 3f) && (GameManager.Instance.playerTr.position.z < 4.5f && GameManager.Instance.playerTr.position.z > 3.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
