using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCastSpell : TutorialBase
{
    public GameObject canvas;
    public Transform playerTr;
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
        text.text = "���� ��Ʈ�ѷ��� �׸���ư�� ������ å�� ��ȯ�ǰ� å�� ������ �������� ���� ������ ��Ÿ�� �ֽ��ϴ�\r\n�׸���ư�� ����ä X��ư�� ���� ������ �ٲ� �� �ֽ��ϴ�\r\n������ ��Ʈ�ѷ��� ���� �������� ��¦ ����ֽð� A��ư�� ����ä ��Ʈ�ѷ��� ������ ���� ���� �հ����� ��ư���� ���� ������ �߻�˴ϴ�\r\n������ �߻�ȴٸ� �տ� ǥ�÷� ���ּ���";
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > -3f && playerTr.position.x < -2f) && (playerTr.position.z < 4.5f && playerTr.position.z > 3.5f))
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
