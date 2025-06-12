using UnityEngine;

public class LolStyleCamera : MonoBehaviour
{
    [Header("필수")]
    [SerializeField] Transform target;      // 플레이어 캐릭터

    [Header("시점 각도·오프셋")]
    [SerializeField] float tiltAngle = 45f; // 아래로 기울기
    [SerializeField] float yawAngle = 45f; // 시계방향 회전(LoL 기본 ≈45)
    [SerializeField] float followDistance = 15f;
    [SerializeField] float heightOffset = 10f;

    [Header("부드러운 추적")]
    [SerializeField] float followSmooth = 0.08f;   // 위치 보간
    Vector3 velocity = Vector3.zero;

    [Header("줌 (휠)")]
    [SerializeField] float minDist = 8f, maxDist = 22f;
    [SerializeField] float zoomSpeed = 40f;

    [Header("화면 가장자리/드래그 카메라 이동")]
    [SerializeField] float edgeSize = 8f;           // px
    [SerializeField] float panSpeed = 25f;
    [SerializeField] KeyCode dragKey = KeyCode.LeftAlt;

    Camera cam;

    void Awake() => cam = GetComponent<Camera>();

    void LateUpdate()
    {
        if (target == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        followDistance = Mathf.Clamp(followDistance - scroll * zoomSpeed * Time.deltaTime, minDist, maxDist);

        Quaternion rot = Quaternion.Euler(tiltAngle, yawAngle, 0f);
        Vector3 wantedPos = target.position
                          + rot * new Vector3(0, heightOffset, -followDistance);

        Vector3 pan = GetPanInput() * panSpeed * Time.deltaTime;
        wantedPos += Quaternion.Euler(0, yawAngle, 0) * pan;   // Yaw 기준 로컬 X/Z 로 이동

        transform.position = Vector3.SmoothDamp(transform.position, wantedPos, ref velocity, followSmooth);

        transform.rotation = rot;
    }

    Vector3 GetPanInput()
    {
        Vector3 move = Vector3.zero;

        Vector2 m = Input.mousePosition;
        if (m.x < edgeSize) move.x = -1;
        else if (m.x > Screen.width - edgeSize) move.x = 1;

        if (m.y < edgeSize) move.z = -1;
        else if (m.y > Screen.height - edgeSize) move.z = 1;

        if (Input.GetKey(dragKey) && Input.GetMouseButton(1))
        {
            move.x = -Input.GetAxis("Mouse X");
            move.z = -Input.GetAxis("Mouse Y");
        }

        return move.normalized;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}