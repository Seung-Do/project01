using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialStart : TutorialBase
{
    public GameObject canvas;
    public Transform playerTr;
    public TMP_Text text;
    public GameObject leftControllerImage;

    Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
    Quaternion rotationR = Quaternion.Euler(0f, -270f, 0f);


    public override void Enter()
    {
        canvas.SetActive(true);
        leftControllerImage.SetActive(true);
        text.text = "�÷��̾� ���ۿ� ���� �˾ƺ��ڽ��ϴ�\r\n���� �潺ƽ�� �¿�� �Է��Ͽ� �������� �����մϴ�\r\n���� �潺ƽ�� ���������� ���� �Է��ؼ� ������ ������ ���ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        //Debug.Log(playerTr.rotation.y);
        if (playerTr.rotation == rotation || playerTr.rotation == rotationR)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        canvas.SetActive(false);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
