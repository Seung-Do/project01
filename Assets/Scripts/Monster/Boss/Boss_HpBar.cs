using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HpBar : MonoBehaviour
{
    /* Camera uiCamera;
     Canvas canvas;
     RectTransform rectParent;   //�θ�
     RectTransform rectHp;       //�ڱ��ڽ�

     //hpBar �̹����� ��ġ�� ������ ������
     [HideInInspector]   //public �ν����Ϳ��� ����
     public Vector3 offset = Vector3.zero;
     public Transform targetTr;  //���� ���

     void Start()
     {
         canvas = GetComponentInParent<Canvas>();
         uiCamera = canvas.worldCamera;
         rectParent = canvas.GetComponent<RectTransform>();
         rectHp = this.gameObject.GetComponent<RectTransform>();
     }

     void LateUpdate()
     {
         //������ǥ > ��ũ����ǥ > ĵ������ǥ �� ��ȯ
         //��������� ������Ʈ�� UI����� ��ġ ��ġ
         //���� ��ǥ�� ��ũ���� ��ǥ�� ��ȯ
         var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

         //ī�޶��� ������ �� ��ǥ�� ����
         if (screenPos.z < 0f)
         {
             screenPos *= -1f;
         }
         var localPos = Vector2.zero;    //RectTransform ��ǥ
         //��ũ�� ��ǥ�� RectTransform ������ ��ǥ�� ��ȯ
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
         //hpBar�� ��ġ�� ���� RectTransform ��ǥ�� ����
         rectHp.localPosition = localPos;
     }*/

    private void Update()
    {
        transform.LookAt(GameManager.Instance.playerTr.position);
    }
}
