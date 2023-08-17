using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public testPlayer testPlayer;
    public PoolManager poolManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
