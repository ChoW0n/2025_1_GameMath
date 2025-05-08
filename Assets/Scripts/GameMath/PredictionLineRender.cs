using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PredictionLineRender : MonoBehaviour
{
    [Header("설정값")]
    [SerializeField] private float extend = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject bulletPrefab;   // 총알 프리팹
    [SerializeField] private Transform startPoint;      // 발사 위치

    private Transform target;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        HandleInput();

        if (target == null || startPoint == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        Vector3 a = startPoint.position;
        Vector3 b = target.position;
        Vector3 prediction = Vector3.LerpUnclamped(a, b, extend);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, a);
        lineRenderer.SetPosition(1, prediction);
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

        // 총알 발사
        if (Input.GetKeyDown(KeyCode.Space) && target != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, startPoint.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Init(target);
            }
        }
    }
}