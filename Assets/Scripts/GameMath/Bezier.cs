using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Transform p0;
    public Transform p3;

    [Header("Random Ranges")]
    public float p1Radius = 2f;     // p0 근처 제어점 거리
    public float p1Height = 2f;     // p1 Y 높이
    public float p2Radius = 2f;     // p3 근처 제어점 거리
    public float p2Height = 2f;     // p2 Y 높이

    [HideInInspector] public Vector3 p1;
    [HideInInspector] public Vector3 p2;

    private List<Vector3> points;
    float time = 0f;

    void Awake()
    {
        GenerateRandomControlPoints();
        points = new List<Vector3>() { p0.position, p1, p2, p3.position };
    }

    void Update()
    {
        time += Time.deltaTime / 2f;
        transform.position = DeCasteljau(points, time);
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0.position + new Vector3(rand1.x, p1Height, rand1.y); // p0 근처 랜덤 위치

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3.position + new Vector3(rand2.x, p2Height, rand2.y); // p3 근처 랜덤 위치
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;
            var next = new List<Vector3>(last);

            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }

            p = next;
        }

        return p[0];
    }
}