using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMath3 : MonoBehaviour
{

    public float degrees;
    public float radianValue;

    public float speed = 5f;
    public float angle = 30f;

    void Start()
    {
        float radians = degrees * Mathf.Deg2Rad;
        Debug.Log("Degree to radian : " + radians);
        float degreeValue = radianValue * Mathf.Rad2Deg;
        Debug.Log("Radian to degree : " + degreeValue);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed += 1f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            speed -= 1f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            angle += 15f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            angle -= 15f;
        }
        float radians = angle * Mathf.Rad2Deg;

        Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));

        transform.position += direction * speed * Time.deltaTime;
    }
}
