using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialMove : TutorialBase
{
    public GameObject canvas;
    public Transform playerTr;
    public TMP_Text text;
    public GameObject maker;
    public GameObject teleportInteractor;
    public GameObject rightControllerImage;
    public GameObject leftControllerImage;
    void Start()
    {
        
    }

    public override void Enter()
    {
        canvas.SetActive(true);
       GameManager.Instance.moveProvider.enabled = true;
        rightControllerImage.SetActive(true);
        teleportInteractor.SetActive(true);
        text.text = "���� �潺ƽ�� ������ ���Ͽ� �ڷ���Ʈ ������ ���� �� ������ �潺ƽ�� ������ �� �������� �ڷ���Ʈ�մϴ�\r\n�ڷ���Ʈ�� ������ ������ ��������� ǥ�õǸ� �Ұ����� ������ ���������� ǥ�õ˴ϴ�\r\n������ �潺ƽ�� ����ؼ� �����¿� �̵��� �����մϴ�\r\n�¿��� �潺ƽ�� ����Ͽ� �տ� ���̴� ǥ�� �������� �̵��� �ּ���";
        maker.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > -3f && playerTr.position.x < -2f) && (playerTr.position.z < 1.5f && playerTr.position.z > 0.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        canvas.SetActive(false);
        maker.SetActive(false);
        rightControllerImage.SetActive(false);
        leftControllerImage.SetActive(false);

    }
}
