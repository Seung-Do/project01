using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialReadySheild : TutorialBase
{
    public GameObject canvas;
    public Transform playerTr;
    public TMP_Text text;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        maker.SetActive(true);
        text.text = "3번째 마법인 폭발마법은 강력한 광역 마법이지만 플레이어의 체력을 한 칸 소모하고 체력이 한칸 남았을 때는 사용하지 못합니다\r\n만약 체력을 소모했다면 완쪽에 보이는 생명의 샘에서 체력을 회복할 수 있습니다\r\n준비가 되면 오른쪽에 나타난 표시 지점으로 이동해 주세요";
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > 2f && playerTr.position.x < 3f) && (playerTr.position.z < 4.5f && playerTr.position.z > 3.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
