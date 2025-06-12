using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow3D_2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stopDistance = 0.1f;

    private Queue<Vector3> moveQueue = new Queue<Vector3>();
    private Vector3? currentTarget = null;

    void Update()
    {
        // 마우스 우클릭 목적지 지정
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 clickedPoint = hit.point;
                clickedPoint.y += 1f;

                if (currentTarget == null)
                {
                    currentTarget = clickedPoint;
                }
                else
                {
                    moveQueue.Enqueue(clickedPoint);
                }
            }
        }

        // 이동 중일 때
        if (currentTarget != null)
        {
            Vector3 target = currentTarget.Value;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < stopDistance)
            {
                // 도착하면 다음 목적지 꺼냄
                if (moveQueue.Count > 0)
                {
                    currentTarget = moveQueue.Dequeue();
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
    }
}
