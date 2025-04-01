using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVisualizer : MonoBehaviour
{
    public float viewAngle = 60f;
    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        //���� �þ� ���
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        //������ �þ� ���
        Vector3 RightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, RightBoundary);

        //ĳ���� ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
}
