using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialHeal : TutorialBase
{
    public GameObject canvas;
    public TMP_Text text;
    public GameObject maker;

    void Start()
    {

    }
    public override void Enter()
    {
        maker.SetActive(true);
        text.text = "광역 마법은 강력하지만 플레이어의 체력을 한 칸 소모하고 체력이 한칸 남았을 때는 사용하지 못합니다\r\n만약 체력을 소모했다면 왼쪽에 보이는 생명의 샘에서 체력을 회복할 수 있습니다\r\n준비가 되면 오른쪽에 나타난 표시 지점으로 이동해 주세요";
    }

    public override void Execute(TutorialController controller)
    {
        if ((GameManager.Instance.playerTr.position.x > 2f && GameManager.Instance.playerTr.position.x < 3f) && (GameManager.Instance.playerTr.position.z < 1.5f && GameManager.Instance.playerTr.position.z > 0.5f))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        maker.SetActive(false);
        canvas.SetActive(false);
    }
}
