using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZiggsBomb : MonoBehaviour
{
    [Header("폭탄 파라미터")]
    [SerializeField] int maxGroundBounces = 3;
    [SerializeField] float explosionRadius = 2.5f;
    [SerializeField] int damage = 50;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject explosionFX;

    int bounceCount;
    Rigidbody rb;

    /* 중복 OnCollisionEnter 방지용 */
    float bounceCooldown = 0.05f;     // 0.05초 내 재충돌 무시
    float lastBounceTime = -999f;

    void Awake() => rb = GetComponent<Rigidbody>();

    /* ───── 충돌 처리 ───── */
    void OnCollisionEnter(Collision col)
    {
        int layer = col.gameObject.layer;

        /* 1) 적과 충돌 → 즉시 폭발 */
        if (((1 << layer) & enemyLayer) != 0)
        {
            Explode();
            return;
        }

        /* 2) 지면 바운스 카운트 */
        if (((1 << layer) & groundLayer) != 0)
        {
            if (Time.time - lastBounceTime < bounceCooldown)
                return;                    // 같은 바운스를 여러 번 세지 않음

            lastBounceTime = Time.time;
            bounceCount++;

            if (bounceCount >= maxGroundBounces)
                Explode();
        }
    }

    /* ───── 폭발 ───── */
    void Explode()
    {
        if (explosionFX) Instantiate(explosionFX, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (var h in hits)
        {
            if (h.TryGetComponent(out EnemyTarget enemy))
                enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}