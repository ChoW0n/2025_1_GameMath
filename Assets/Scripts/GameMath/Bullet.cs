using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private Transform target;

    public void Init(Transform tg)
    {
        target = tg;
        Destroy(gameObject, lifetime); // 일정 시간 후 자동 삭제
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 명중 거리 체크 (가까워지면 명중 처리)
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            Destroy(target.gameObject); // 적 제거
            //Destroy(gameObject); // 총알 제거
        }
    }
}