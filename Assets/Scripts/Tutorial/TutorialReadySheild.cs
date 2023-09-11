using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialReadySheild : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject rightControllerImage;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        canvas.SetActive(true);
        rightControllerImage.SetActive(true);
        maker.SetActive(true);
        text.text = "오른쪽 컨트롤러의 A버튼을 누르고 원(○)을 그리고 버튼을 때면 마법 방패가 소환됩니다\r\n마법 공격만 막을 수 있고 근접 공격은 막지 못하므로 주의하세요\r\n마법 공격을 막을 준비가 되면 앞에 나타난 표시 지점으로 이동해 주세요";
    }

    public override void Execute(TutorialController controller)
    {
        if ((GameManager.Instance.playerTr.position.x > 2f && GameManager.Instance.playerTr.position.x < 3f) && (GameManager.Instance.playerTr.position.z < 4.5f && GameManager.Instance.playerTr.position.z > 3.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
