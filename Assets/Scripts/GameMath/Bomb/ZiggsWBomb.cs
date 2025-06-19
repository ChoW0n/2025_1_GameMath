using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZiggsWBomb : MonoBehaviour
{
    [Header("폭발")]
    public float fuseTime = 3f;
    public float radius = 4f;
    public float force = 450f;
    public float upward = 1.3f;
    public LayerMask pushLayers;
    public GameObject fx;

    ZiggsWController owner; bool detonated;

    public void Initialize(Transform ownerTr)
    {
        owner = ownerTr.GetComponent<ZiggsWController>();
        Invoke(nameof(Detonate), fuseTime);
    }

    public void Detonate()
    {
        if (detonated) return;
        detonated = true;

        if (fx) Instantiate(fx, transform.position, Quaternion.identity);

        Collider[] cols = Physics.OverlapSphere(transform.position, radius, pushLayers);
        foreach (var c in cols)
        {
            if (!c.attachedRigidbody) continue;
            Vector3 dir = (c.transform.position - transform.position).normalized + Vector3.up * upward;
            float dist = Vector3.Distance(c.transform.position, transform.position);
            float atten = 1f - Mathf.Clamp01(dist / radius);
            c.attachedRigidbody.AddForce(dir.normalized * force * atten, ForceMode.Impulse);
        }

        owner?.OnBombDestroyed();
        Destroy(gameObject);
    }
}