using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFovDetectector : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;       //시야각
    public float viewDistance = 10f;    //시야거리

    void Update()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float distance = Vector3.Distance(transform.position, player.position);

        if (angle < viewAngle / 2 && distance < viewDistance)
        {
            Debug.Log("플레이어가 시야 안에 있음!");
            transform.localScale = Vector3.one * 2f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
