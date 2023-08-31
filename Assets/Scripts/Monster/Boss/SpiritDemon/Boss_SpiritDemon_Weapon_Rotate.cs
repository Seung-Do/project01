using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss_SpiritDemon_Weapon_Rotate : MonoBehaviour
{
    bool isRotate;
    Quaternion targetRotation;
    float rotSpeed = 50f;

    void OnEnable()
    {
        isRotate = false;
        targetRotation = Quaternion.Euler(180, 180, 0);
    }

    void Update()
    {
        if (!isRotate)
        {
            float step = rotSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, step);

            // 정확한 각도 비교를 통해 회전 종료 조건 확인
            if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
            {
                transform.localRotation = targetRotation;
                isRotate = true;
            }
        }
    }
}
