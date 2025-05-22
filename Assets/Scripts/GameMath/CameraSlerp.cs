using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlerp : MonoBehaviour
{
    public Transform target;
    [SerializeField] private LayerMask enemyLayer;
    float speed = 2f;

    void Update()
    {
        HandleInput();
        if (target != null)
        {
            Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
            float t = 1f - Mathf.Exp(-speed * Time.deltaTime);
            transform.rotation = ManualSlerp(transform.rotation, lookRot, t);
        }
    }

    Quaternion ManualSlerp(Quaternion from, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(from, to);

        if (dot < 0f)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            dot = -dot;
        }
        
        if (1f - dot < 0.01f)
        {
            Quaternion lerp = new Quaternion(
                Mathf.Lerp(from.x,  to.x, t),
                Mathf.Lerp(from.y, to.y, t),
                Mathf.Lerp(from.z, to.z, t),
                Mathf.Lerp(from.w, to.w, t)
                );
            return lerp.normalized;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float ratioA = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
            ratioA * from.x + ratioB * to.x,
            ratioA * from.y + ratioB * to.y,
            ratioA * from.z + ratioB * to.z,
            ratioA * from.w + ratioB * to.w
        );

        return result.normalized;
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
    }
}
