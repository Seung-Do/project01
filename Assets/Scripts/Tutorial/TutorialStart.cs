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
        text.text = "플레이어 조작에 대해 알아보겠습니다\r\n왼쪽 썸스틱을 좌우로 입력하여 스냅턴이 가능합니다\r\n왼쪽 썸스틱을 오른쪽으로 세번 입력해서 오른쪽 방향을 봐주세요";
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
