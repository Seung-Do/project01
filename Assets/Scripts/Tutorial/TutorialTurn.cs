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
        text.text = "�ٽ� ���� �潺ƽ�� �������� ���� �Է��ؼ� ���� ������ ���ּ���";
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
