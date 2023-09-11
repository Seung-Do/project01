using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTurn : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    
    
    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
    Quaternion rotationR = Quaternion.Euler(0f, -360f, 0f);

    public override void Enter()
    {
        canvas.SetActive(true);
        text.text = "다시 왼쪽 썸스틱을 왼쪽으로 세번 입력해서 원래 방향을 봐주세요";
    }

    public override void Execute(TutorialController controller)
    {
        if (GameManager.Instance.playerTr.rotation == rotation || GameManager.Instance.playerTr.rotation == rotationR)
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
