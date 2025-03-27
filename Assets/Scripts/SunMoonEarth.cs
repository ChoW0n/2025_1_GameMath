using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonEarth : MonoBehaviour
{
    public Transform centorObject;
    public float angle = 0f;    
    public float speed = 5f;    //���� �ӵ�
    public float radius = 10f;  //���� ������Ʈ���� �Ÿ�(�ݰ�)
    void Update()
    {
        angle += Time.deltaTime * speed;        //ȸ�� ���� ����
        float radians = angle * Mathf.Rad2Deg;  //Degree to Radian
        //���� �׸��� ��� x = �ڻ���(0) * �ݰ� z = ����(0) * �ݰ�
        float x = Mathf.Cos(radians) * radius;
        float z = Mathf.Sin(radians) * radius;

        //�߽����� ������ ������Ʈ�� �������� ������
        transform.position = centorObject.position + new Vector3(x, 0, z);
    }
}
