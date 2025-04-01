using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 90f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
        

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationInput * rotationSpeed * Time.deltaTime, 0);
            transform.rotation = transform.rotation * deltaRotation;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationInput * rotationSpeed * Time.deltaTime, 0);
            transform.rotation = transform.rotation * deltaRotation;
        }
    }
}
