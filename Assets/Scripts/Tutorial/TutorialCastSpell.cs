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
        text.text = "왼쪽 컨트롤러의 그립버튼을 누르면 책이 나타나고 책의 오른쪽 페이지에 사용가능한 마법이 나와 있습니다\r\n오른쪽 컨트롤러를 살짝 위로 들고 A버튼을 누른채 슬러쉬(/)를 그려주고 손가락을 버튼에서 떼면 마법이 발사됩니다\r\n정면을 향해 X(α)모양을 그리면 광역 마법을 사용합니다\r\n마법이 사용되면 앞의 지점으로 나가주세요";
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
