using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCueCamera : MonoBehaviour
{
    [SerializeField] Transform tableCenter; // 당구판 중심 Empty
    [SerializeField] float distance = 7f;
    [SerializeField] float height = 4f;
    [SerializeField] float yawSpeed = 120f;
    [SerializeField] float zoomSpeed = 3f;
    [SerializeField] float minDist = 5f, maxDist = 10f;

    Rigidbody targetCue;                   // 현재 턴 큐볼
    float yaw;

    void LateUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        yaw += x * yawSpeed * Time.deltaTime;

        // 마우스 스크롤로 확대/축소
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDist, maxDist);

        // 현재 턴 큐볼 중심으로 시점 자동 보정
        if (BilliardGameManager.Instance != null)
            targetCue = BilliardGameManager.Instance.CurrentCue;

        Vector3 pivot = targetCue != null ? targetCue.position : tableCenter.position;

        Quaternion rot = Quaternion.Euler(15f, yaw, 0f);
        Vector3 offset = rot * new Vector3(0, height, -distance);

        transform.position = pivot + offset;
        transform.LookAt(pivot);
    }
}
