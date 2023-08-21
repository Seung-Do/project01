using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisible : MonoBehaviour
{
    private Transform playerTr; // �÷��̾��� Transform ������Ʈ
    private float visibleDistance = 8f; // ť�긦 ���̰� �� �ּ� �Ÿ�
    private MeshRenderer wallRenderer;
    private Collider playerCollider;
    private Collider wallCollider;

    private void Start()
    {
        wallRenderer = GetComponent<MeshRenderer>();
        playerTr = GameManager.Instance.playerTr;
        playerCollider = playerTr.GetComponent<Collider>();
        wallCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        // �÷��̾�� �� ���� ���� ����� ������ ���
        Vector3 closestPointOnWall = wallCollider.ClosestPoint(playerTr.position);

        // �÷��̾�� ���� ����� ���� ������ �Ÿ� ���
        float distanceToWall = Vector3.Distance(playerTr.position, closestPointOnWall);

        //Debug.Log(distanceToWall.ToString());

        if (distanceToWall <= visibleDistance)
        {
            wallRenderer.enabled = true;
        }
        else
        {
            wallRenderer.enabled = false;   
        }
    }
}







