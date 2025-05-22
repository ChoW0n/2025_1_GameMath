using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomPatrol : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float moveDistance = 2f;      // 이동 거리
    [SerializeField] private float moveDuration = 1f;      // 이동하는 데 걸리는 시간
    [SerializeField] private float waitTime = 1f;          // 원래 위치에서 대기 시간

    private Vector3 originPosition;
    private Vector3 targetPosition;
    private float timer = 0f;
    private bool isMovingOut = true;

    void Start()
    {
        originPosition = transform.position;
        ChooseNewTarget();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isMovingOut)
        {
            float t = Mathf.Clamp01(timer / moveDuration);
            transform.position = Vector3.Lerp(originPosition, targetPosition, t);

            if (t >= 1f)
            {
                isMovingOut = false;
                timer = 0f;
            }
        }
        else
        {
            float t = Mathf.Clamp01(timer / moveDuration);
            transform.position = Vector3.Lerp(targetPosition, originPosition, t);

            if (t >= 1f)
            {
                isMovingOut = true;
                timer = -waitTime; // 다음 이동까지 잠시 대기
                ChooseNewTarget();
            }
        }
    }

    void ChooseNewTarget()
    {
        int dir = Random.Range(0, 4);
        Vector3 offset = Vector3.zero;

        switch (dir)
        {
            case 0: offset = Vector3.left * moveDistance; break;
            case 1: offset = Vector3.up * moveDistance; break;
            case 2: offset = Vector3.right * moveDistance; break;
            case 3: offset = Vector3.back * moveDistance; break;
        }

        targetPosition = originPosition + offset;
    }
}