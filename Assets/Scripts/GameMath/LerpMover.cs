using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMover : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [SerializeField] float duration = 2f;
    [SerializeField] float t = 0f;

    // Update is called once per frame
    void Update()
    {
        t = Mathf.PingPong(Time.time / duration, 1f);
        //t += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
        //if (t > 1f)
        //{
        //    t += Time.deltaTime / duration;

        //    Vector3 a = startPos.position;
        //    Vector3 b = endPos.position;
        //    Vector3 p = (1f - t) * a + t * b;

        //    transform.position = p;
        //}
    }
}
