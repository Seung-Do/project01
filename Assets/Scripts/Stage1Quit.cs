using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Quit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        GameManager.Instance.RestartStage1();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
