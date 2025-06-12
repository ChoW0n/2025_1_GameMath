using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MouseFollow3D : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 targetPosition;
    private Vector3 targetPos;


    void Update()
    {
        //마우스 왼쪽 버튼 클릭
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                targetPosition = new Vector3(targetPosition.x, targetPosition.y + 1f, targetPosition.z);
            }
        }
        
        if ((targetPosition - transform.position).magnitude > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 move = direction * speed * Time.deltaTime;
            transform.position += move;
        }
        
        



    }
}
