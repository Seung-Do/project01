using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRune : MonoBehaviour
{
    [SerializeField] GameObject target;
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
            target.SetActive(false);
            GameManager.Instance.CenterHallStage1();
            gameObject.SetActive(false);
        }
    }
}
