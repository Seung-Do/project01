using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;
    [SerializeField]
    private string nextScene = "stage01";

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    Transform playerTr;


    void Start()
    {
        SetNextTutorial();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

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

        if (!nextScene.Equals(""))
        {
            SceneManager.LoadScene(nextScene);
            playerTr.position = new Vector3(22.5f, 25.6f, -2.5f);
            playerTr.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }
}
