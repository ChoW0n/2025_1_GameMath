using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardShooter : MonoBehaviour
{
    [Header("힘 조절")]
    [SerializeField] float maxPower = 8f;           // 100% 힘
    [SerializeField] float chargeTime = 1.0f;       // 충전 100%까지 걸리는 시간
    [SerializeField] LayerMask tableLayer;          // 테이블(Plane)을 지정

    Rigidbody cueRb;
    float holdTimer = 0f;

    void Start()
    {
        // cueBall 찾기 (Tag 로 구분)
        GameObject cue = GameObject.FindGameObjectWithTag("CueBall");
        if (cue != null) cueRb = cue.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (cueRb == null) return;

        // 마우스 왼쪽 버튼 누르는 동안 힘 충전
        if (Input.GetMouseButton(0))
        {
            holdTimer += Time.deltaTime;
        }

        // 버튼에서 손을 뗄 때 힘 발사
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 테이블에 레이캐스트 클릭 지점 구하기
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, tableLayer))
            {
                Vector3 dir = (hit.point - cueRb.position);
                dir.y = 0f;                          // 수평 힘만 주기
                if (dir.sqrMagnitude > 0.001f)
                {
                    Vector3 nDir = dir.normalized;
                    float powerPercent = Mathf.Clamp01(holdTimer / chargeTime);
                    float force = powerPercent * maxPower;

                    cueRb.AddForce(nDir * force, ForceMode.Impulse);
                }
            }
            holdTimer = 0f;                          // 충전 초기화
        }
    }
}
