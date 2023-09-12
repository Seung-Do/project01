using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage0Quit : MonoBehaviour
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
        GameManager.Instance.PassTutorial();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
