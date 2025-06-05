using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    private float yaw = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        yaw += input * 100 * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0, 6, -7);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
