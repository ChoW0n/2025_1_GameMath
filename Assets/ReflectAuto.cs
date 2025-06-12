using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAuto : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, -3f, 0);

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col)
    {
        Vector3 normal = col.contacts[0].normal.normalized; //���� ���� ����ȭ
        Vector3 reflect = Vector3.Reflect(velocity, normal);    //�ݻ� ���� ���
        velocity = reflect;
    }
}
