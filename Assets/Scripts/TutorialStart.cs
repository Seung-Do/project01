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
        text.text = "여기는 마법사의 창고입니다\r\n게임 조작에 대해 간단히 알아 보겠습니다\r\n왼쪽 썸스틱을 좌우로 입력하여 스냅턴이 가능합니다\r\n오른쪽으로 세번 입력해서 오른쪽 방향을 봐주세요";
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
