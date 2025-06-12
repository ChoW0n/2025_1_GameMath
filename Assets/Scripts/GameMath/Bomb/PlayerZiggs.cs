using UnityEngine;

public class PlayerZiggs : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float baseThrowPower = 10f;
    [SerializeField] float upwardAngle = 35f;      // 위로 살짝 던지기

    Camera cam;

    void Awake() => cam = Camera.main;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ThrowBomb();
    }

    void ThrowBomb()
    {
        if (bombPrefab == null) { Debug.LogWarning("Bomb Prefab missing!"); return; }


        // 월드 마우스 위치 얻기 (지면 평면과 레이캐스트)
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Debug.Log("Raycast miss – 바닥을 클릭하지 않았거나 Ground 레이어 미설정");
            return;
        }

        Vector3 dir = (hit.point - transform.position);
        dir.y = 0f;                        // 수평방향
        if (dir.sqrMagnitude < 0.1f) return;

        Vector3 forward = dir.normalized;

        // 투척 방향 = 수평 forward + 위쪽 성분
        Vector3 launchDir = Quaternion.Euler(-upwardAngle, 0, 0) * forward;

        GameObject bomb = Instantiate(bombPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        // …(Instantiate 이후)
        Debug.Log("Bomb spawned OK");

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = launchDir * baseThrowPower;
    }
}