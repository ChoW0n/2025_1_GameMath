using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AutoRotation : MonoBehaviour
{
    public float rotationSpeed = 90f;

    //y 축을 기준으로 1초 마다 45도 회전
    void Update()
    {
        //float input = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = deltaRotation * transform.rotation;


        //if (Mathf.Abs(input) > 0.01f)
        //{
        //    Quaternion deltaRotation = Quaternion.Euler(0f, input * rotationSpeed * Time.deltaTime, 0f);
        //    transform.rotation = deltaRotation * transform.rotation;
        //}
    }
}
