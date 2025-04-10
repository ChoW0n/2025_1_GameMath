using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonEarth : MonoBehaviour
{
    public Transform centorObject;
    public float angle = 0f;    
    public float speed = 5f;    //공전 속도
    public float radius = 10f;  //센터 오브젝트와의 거리(반경)
    void Update()
    {
        angle += Time.deltaTime * speed;        //회전 각도 변경
        float radians = angle * Mathf.Rad2Deg;  //Degree to Radian
        //원을 그리는 방법 x = 코사인(0) * 반경 z = 사인(0) * 반경
        float x = Mathf.Cos(radians) * radius;
        float z = Mathf.Sin(radians) * radius;

        //중심으로 설정한 오브젝트를 기준으로 공전함
        transform.position = centorObject.position + new Vector3(x, 0, z);
    }
}
