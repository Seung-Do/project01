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
        text.text = "왼쪽 컨트롤러의 그립버튼을 누르면 책이 소환되고 책의 오른쪽 페이지에 현재 마법이 나타나 있습니다\r\n그립버튼을 누른채 X버튼을 눌러 마법을 바꿀 수 있습니다\r\n오른쪽 컨트롤러를 위쪽 방향으로 살짝 들어주시고 A버튼을 누른채 컨트롤러를 앞으로 향한 다음 손가락을 버튼에서 떼면 마법이 발사됩니다\r\n마법이 발사된다면 앞에 표시로 가주세요";
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
