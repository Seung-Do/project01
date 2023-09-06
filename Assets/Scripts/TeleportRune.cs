using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRune : MonoBehaviour
{
    [SerializeField] GameObject runeEffect01;
    [SerializeField] GameObject runeEffect02;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PLAYER"))
        {
            runeEffect01.SetActive(false);
            runeEffect02.SetActive(false);
            GameManager.Instance.CenterHallStage1();
        }
    }
}
