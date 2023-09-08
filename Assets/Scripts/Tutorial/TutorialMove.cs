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
        text.text = "왼쪽 썸스틱을 앞으로 향하여 텔레포트 지점을 정할 수 있으며 썸스틱을 놓으면 그 지점으로 텔레포트합니다\r\n텔레포트가 가능한 지점은 보라색으로 표시되며 불가능한 지점은 빨간색으로 표시됩니다\r\n오른쪽 썸스틱을 사용해서 전후좌우 이동이 가능합니다\r\n좌우의 썸스틱을 사용하여 앞에 보이는 표시 지점으로 이동해 주세요";
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
