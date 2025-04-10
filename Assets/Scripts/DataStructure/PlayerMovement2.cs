using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 5f;
    private Stack<Vector3> positionStack = new Stack<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDir = Vector3.forward;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDir = Vector3.back;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir = Vector3.right;
        }
        if (moveDir != Vector3.zero)
        {
            positionStack.Push(transform.position);
            transform.position += moveDir * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.B) && positionStack.Count > 0)
        {
            transform.position = positionStack.Pop();
        }
    }
}
