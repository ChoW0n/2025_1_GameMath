using UnityEngine;

public class AutoLockOnSystem : MonoBehaviour
{
    public Transform target;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Camera 움직임 설정")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("총알 발사 지점, 프리펩")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lockOnPoint;
    private bool isLockedOn = false;
    private Vector3 originPosition;
    private Quaternion originRotation;


    private void Start()
    {
        //원점 회전, 포지션 기억
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
            isLockedOn = false; // 더 이상 이동하지 않음
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
                // 현재 위치와 타겟 사이의 고정된 1/4 지점 계산
                lockOnPoint = Vector3.Lerp(transform.position, target.position, 0.75f);
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
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Init(target);
            }
        }
    }
}