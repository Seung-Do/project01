using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMove : TutorialBase
{
    public GameObject imageMove;
    public Transform playerTr;
    private bool isCompleted;
    void Start()
    {
        
    }

    public override void Enter()
    {
        imageMove.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if ((playerTr.position.x > -3 && playerTr.position.x < -1) && (playerTr.position.z < 3 && playerTr.position.z > 1))
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
        imageMove.SetActive(false);
    }
}
