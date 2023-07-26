using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMove : TutorialBase
{
    public GameObject panel;
    public Transform playerTr;
    public TMP_Text text;
    public ParticleSystem point;
    void Start()
    {
        
    }

    public override void Enter()
    {
        panel.SetActive(true);
        text.text = "���� �潺ƽ�� ������ ���Ͽ� �ڷ���Ʈ ������ ���� �� ������ �潺ƽ�� ������ �� �������� �ڷ���Ʈ�մϴ�\r\n�ڷ���Ʈ�� ������ ������ ��������� ǥ�õǸ� �Ұ����� ������ ���������� ǥ�õ˴ϴ�\r\n������ �潺ƽ�� ����ؼ� �����¿� �̵��� �����մϴ�\r\n�¿��� �潺ƽ�� ����ؼ� �տ� ���̴� ǥ�� �������� �̵��� �ּ���";
        point.Play();
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > -3 && playerTr.position.x < -2) && (playerTr.position.z < 3 && playerTr.position.z > 2))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        panel.SetActive(false);
        point.Stop();
    }
}
