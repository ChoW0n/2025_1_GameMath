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
        // ���콺 ��Ŭ�� ������ ����
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 clickedPoint = hit.point;

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

        // �̵� ���� ��
        if (currentTarget != null)
        {
            Vector3 target = currentTarget.Value;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < stopDistance)
            {
                // �����ϸ� ���� ������ ����
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
