using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCastSpell : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject rightControllerImage;
    public GameObject leftControllerImage;
    public GameObject maker;

    public override void Enter()
    {
        canvas.SetActive(true);
        maker.SetActive(true);
        rightControllerImage.SetActive(true);
        leftControllerImage.SetActive(true);
        text.text = "���� ��Ʈ�ѷ��� �׸���ư�� ������ å�� ��Ÿ���� å�� ������ �������� ��밡���� ������ ���� �ֽ��ϴ�\r\n������ ��Ʈ�ѷ��� ��¦ ���� ��� A��ư�� ����ä ������(/)�� �׷��ְ� �հ����� ��ư���� ���� ������ �߻�˴ϴ�\r\n������ ���� X(��)����� �׸��� ���� ������ ����մϴ�\r\n������ ���Ǹ� ���� �������� �����ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        if ((GameManager.Instance.playerTr.position.x > -3f && GameManager.Instance.playerTr.position.x < -2f) && (GameManager.Instance.playerTr.position.z < 4.5f && GameManager.Instance.playerTr.position.z > 3.5f))
            controller.SetNextTutorial();

    }

    public override void Exit()
    {
        canvas.SetActive(false);
        maker.SetActive(false);
        leftControllerImage.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
