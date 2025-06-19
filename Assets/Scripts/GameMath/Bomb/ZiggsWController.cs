using UnityEngine;

public class ZiggsWController : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float throwSpeed = 13f;
    [SerializeField] float throwAngleDeg = 35f;
    [SerializeField] float maxRange = 8f;
    [SerializeField] LayerMask groundMask;   // Ground 전용

    Camera cam;
    ZiggsWBomb active;

    void Awake() => cam = Camera.main;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.W)) return;

        /* 이미 설치된 폭탄이 있으면 즉폭 */
        if (active)
        {
            active.Detonate();
            active = null;
            return;
        }

        if (!bombPrefab) { Debug.LogError("bombPrefab Missing"); return; }

        /* 마우스→지면 월드 좌표 */
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),
                             out RaycastHit hit, 100f, groundMask))
            return;

        Vector3 flatDir = hit.point - transform.position;
        flatDir.y = 0f;
        if (flatDir.sqrMagnitude < 0.1f) return;

        /* 사거리 제한 */
        if (flatDir.magnitude > maxRange)
            flatDir = flatDir.normalized * maxRange;

        /* 투척 각도를 위로 기울여 속도 벡터 계산 */
        Vector3 launchDir =
            Quaternion.Euler(-throwAngleDeg, 0, 0) * flatDir.normalized;

        GameObject bomb = Instantiate(bombPrefab,
                                      transform.position + Vector3.up * 1.0f,
                                      Quaternion.identity);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = launchDir * throwSpeed;   // 반드시 Rigidbody 있어야 함

        active = bomb.GetComponent<ZiggsWBomb>();
        active.Initialize(transform);            // 플레이어 reference
    }

    public void OnBombDestroyed() => active = null;
}