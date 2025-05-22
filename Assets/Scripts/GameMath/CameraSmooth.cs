using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1f, -2f);
   
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] float maxSpeed = 100f;

    Vector3 velocity = Vector3.zero;


    void Update()
    {
        HandleInput();
    }
    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = target.position + target.TransformDirection(offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref velocity,
            smoothTime,
            maxSpeed,
            Time.deltaTime);
    }

    private void HandleInput()
    {
        // 타겟 선택
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
            {
                target = hit.transform;
            }
        }

        // 타겟 해제
        if (Input.GetMouseButtonDown(1))
        {
            target = null;
        }
    }
}
