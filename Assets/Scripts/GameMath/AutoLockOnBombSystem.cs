using UnityEngine;
using System.Collections;

public class AutoLockOnBombSystem : MonoBehaviour
{
    public Transform target;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Camera 움직임 설정")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("폭탄 발사")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int bombCount = 10;
    [SerializeField] private float launchInterval = 0.1f;
    [SerializeField] private float flightDuration = 1.5f;
    [SerializeField] private float controlOffset = 2f;
    [SerializeField] private float heightOffset = 3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lockOnPoint;
    private bool isLockedOn = false;
    private Vector3 originPosition;
    private Quaternion originRotation;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    void Update()
    {
        HandleInput();

        if (target == null) return;

        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
        float maxDegrees = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, maxDegrees);
    }

    void LateUpdate()
    {
        if (!isLockedOn) return;

        float dist = Vector3.Distance(transform.position, lockOnPoint);
        if (dist < 0.01f)
        {
            transform.position = lockOnPoint;
            velocity = Vector3.zero;
            isLockedOn = false;
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            lockOnPoint,
            ref velocity,
            smoothTime,
            maxSpeed,
            Time.deltaTime);
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
            {
                target = hit.transform;
                lockOnPoint = Vector3.Lerp(transform.position, target.position, 0.25f);
                isLockedOn = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            target = null;
            isLockedOn = false;
            transform.position = originPosition;
            transform.rotation = originRotation;
        }

        if (Input.GetKeyDown(KeyCode.Space) && target != null && firePoint != null)
        {
            StartCoroutine(FireBombs());
        }
    }

    IEnumerator FireBombs()
    {
        for (int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

            // 충돌 감지용 레이어 설정
            BombCollision collision = bomb.GetComponent<BombCollision>();
            if (collision != null)
            {
                collision.enemyLayer = enemyLayer;
            }

            StartCoroutine(MoveBombBezier(bomb.transform));
            yield return new WaitForSeconds(launchInterval);
        }
    }

    IEnumerator MoveBombBezier(Transform bomb)
    {
        if (target == null) 
        {
            Destroy(bomb.gameObject);
            yield break;
        }
        Vector3 p0 = firePoint.position;
        Vector3 p3 = target.position;
        

        Vector3 p1 = p0 + Random.insideUnitSphere * controlOffset;
        p1.y += heightOffset;
        Vector3 p2 = p3 + Random.insideUnitSphere * controlOffset;
        p2.y += heightOffset;

        float t = 0f;
        while (t < 1f)
        {
            if (bomb == null)
                yield break;

            t += Time.deltaTime / flightDuration;
            bomb.position = GetPointOnBezierCurve(p0, p1, p2, p3, t);
            yield return null;
        }

        if (bomb != null)
            Destroy(bomb.gameObject);
    }

    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.LerpUnclamped(ab, bc, t);
    }
}