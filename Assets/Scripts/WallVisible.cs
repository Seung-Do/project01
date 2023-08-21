using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisible : MonoBehaviour
{
    private Transform playerTr; // 플레이어의 Transform 컴포넌트
    private float visibleDistance = 8f; // 큐브를 보이게 할 최소 거리
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
        // 플레이어와 벽 간의 가장 가까운 지점을 계산
        Vector3 closestPointOnWall = wallCollider.ClosestPoint(playerTr.position);

        // 플레이어와 가장 가까운 지점 사이의 거리 계산
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







