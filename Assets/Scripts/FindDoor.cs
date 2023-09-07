using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDoor : MonoBehaviour
{
    public Animator leftAnim;
    public Animator rightAnim;
    void Start()
    {
        GameManager.Instance.leftDoorAnim = leftAnim;
        GameManager.Instance.rightDoorAnim = rightAnim;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
