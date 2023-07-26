using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialStart : TutorialBase
{
    public GameObject panel;
    public Transform playerTr;
    public TMP_Text text;
    Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
    Quaternion rotationR = Quaternion.Euler(0f, -270f, 0f);

    public override void Enter()
    {
        panel.SetActive(true);
        text.text = "����� �������� â���Դϴ�\r\n���� ���ۿ� ���� ������ �˾� ���ڽ��ϴ�\r\n���� �潺ƽ�� �¿�� �Է��Ͽ� �������� �����մϴ�\r\n���������� ���� �Է��ؼ� ������ ������ ���ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        //Debug.Log(playerTr.rotation.y);
        if(playerTr.rotation == rotation || playerTr.rotation == rotationR)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        panel.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
