using UnityEngine;

public class MouseFollow3D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stopRadius = 0.05f;   // 도착으로 인정할 거리
    [SerializeField] float heightOffset = 1f;    // 목표 지점 y 보정
    [SerializeField] LayerMask groundMask;       // 바닥(레이캐스트용)

    Vector3 targetPos;
    bool hasTarget;

    void Update()
    {
        HandleClick();
        MoveToTarget();
    }


    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(1)) return;          // 우클릭이 아닐 때 return

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask)) return;

        // 목표 저장 & 활성화
        targetPos = hit.point + Vector3.up * heightOffset;
        hasTarget = true;
    }


    void MoveToTarget()
    {
        if (!hasTarget) return;

        Vector3 flatTarget = targetPos;
        flatTarget.y = transform.position.y;

        Vector3 dir = (flatTarget - transform.position);
        float dist = dir.magnitude;

        if (dist <= stopRadius)     
        {
            hasTarget = false;
            return;
        }

        Vector3 step = dir.normalized * moveSpeed * Time.deltaTime;
        transform.position += step;


        float newY = Mathf.Lerp(transform.position.y, targetPos.y, 10f * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}