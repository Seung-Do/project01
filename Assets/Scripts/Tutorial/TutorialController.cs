using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials; 

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    Transform playerTr;
    [SerializeField]
    private GameObject hands;
    


    void Start()
    {
        SetNextTutorial();
        playerTr = GameManager.Instance.playerTr;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if (currentIndex >= tutorials.Count - 1)
        {
            Debug.Log("튜토리얼 완료");
            CompleteTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        currentTutorial.Enter();
    }

    public void CompleteTutorials()
    {
        currentTutorial = null;

        GameManager.Instance.Stage1Load();
    }
}
