using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HpBar : MonoBehaviour
{
    /* Camera uiCamera;
     Canvas canvas;
     RectTransform rectParent;   //부모
     RectTransform rectHp;       //자기자신

     //hpBar 이미지의 위치를 조절할 오프셋
     [HideInInspector]   //public 인스펙터에서 숨김
     public Vector3 offset = Vector3.zero;
     public Transform targetTr;  //추적 대상

     void Start()
     {
         canvas = GetComponentInParent<Canvas>();
         uiCamera = canvas.worldCamera;
         rectParent = canvas.GetComponent<RectTransform>();
         rectHp = this.gameObject.GetComponent<RectTransform>();
     }

     void LateUpdate()
     {
         //월드좌표 > 스크린좌표 > 캔버스좌표 로 변환
         //결과적으로 오브젝트와 UI요소의 위치 일치
         //월드 좌표를 스크린의 좌표로 변환
         var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

         //카메라의 뒤쪽일 때 좌표값 보정
         if (screenPos.z < 0f)
         {
             screenPos *= -1f;
         }
         var localPos = Vector2.zero;    //RectTransform 좌표
         //스크린 좌표를 RectTransform 기준의 좌표로 변환
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
         //hpBar의 위치를 계산된 RectTransform 좌표로 설정
         rectHp.localPosition = localPos;
     }*/

    private void Update()
    {
        transform.LookAt(GameManager.Instance.playerTr.position);
    }
}
