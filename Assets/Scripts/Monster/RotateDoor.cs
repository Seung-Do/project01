using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;
    float LeftDoorRot = 60;
    float RightDoorRot = -60;
    float LeftDoorOpenRot = 180;
    float RightDoorOpenRot = -180;
    float lerpAmount;
    bool isOpenDoor = false;
    void Start()
    {

    }

    void Update()
    {
        lerpAmount += Mathf.Clamp01(0 + Time.deltaTime);

        if (isOpenDoor && LeftDoor.transform.rotation.eulerAngles.y != LeftDoorRot)
        {
            OpenDoor();
        }
        else if (!isOpenDoor && RightDoor.transform.rotation.eulerAngles.y != RightDoorRot)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        float Ly = Mathf.Lerp(LeftDoorOpenRot, LeftDoorRot, lerpAmount);
        float Ry = Mathf.Lerp(RightDoorOpenRot, RightDoorRot, lerpAmount);
        LeftDoor.transform.rotation = Quaternion.Euler(0, Ly, 0);
        RightDoor.transform.rotation = Quaternion.Euler(0, Ry, 0);
    }
    void CloseDoor()
    {
        float Ly = Mathf.Lerp(LeftDoorRot, LeftDoorOpenRot, lerpAmount);
        float Ry = Mathf.Lerp(RightDoorRot, RightDoorOpenRot, lerpAmount);
        LeftDoor.transform.rotation = Quaternion.Euler(0, Ly, 0);
        RightDoor.transform.rotation = Quaternion.Euler(0, Ry, 0);
    }
    public void ChangeDoor()
    {
        if(isOpenDoor) isOpenDoor = false;
        else isOpenDoor = true;

        lerpAmount = 0;
    }
}
