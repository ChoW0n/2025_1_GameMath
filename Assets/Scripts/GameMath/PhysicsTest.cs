using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    public float forcePower = 10f;
    private Rigidbody rb;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //rb.AddForce(Vector3.forward * forcePower);
            rb.AddForce(Vector3.forward * forcePower, ForceMode.Force);
        }
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
    }
}
