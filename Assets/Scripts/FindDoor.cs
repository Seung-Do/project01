using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDoor : MonoBehaviour
{
    public Animator leftAnim;
    public Animator rightAnim;
    public GameObject Star;
    void Start()
    {
        GameManager.Instance.leftDoorAnim = leftAnim;
        GameManager.Instance.rightDoorAnim = rightAnim;
        GameManager.Instance.doorStar = Star;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
