using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProduct : MonoBehaviour
{
    public Transform player;        //�÷��̾�
    public Transform target;        //Ÿ��
    public Transform startPotion;        //������ ������Ʈ
    public float viewAngle = 60f;   //�þ߰�
    public float viewDistance = 5f;     //�þ߹���
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = (player.position - target.position).normalized;
        Vector3 toTarget = (target.position - player.position).normalized;
        Vector3 forward = transform.forward;

        float dot = DotProduction(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;  //������ ������ ��ȯ
        float distance = Vector3.Distance(transform.position, player.position);

        if (IsLeft(forward, toTarget, Vector3.up))
        {
            Debug.Log("Ÿ���� �÷��̾� ���� ���ʿ� ����");
        }
        else
        {
            Debug.Log("Ÿ���� �÷��̾� ���� �����ʿ� ����");
        }

        if (angle < viewAngle / 2 && distance < viewDistance)
        {
            Debug.Log("�÷��̾ �þ� �ȿ� ����");
            player.position = startPotion.position;
        }
    }

    float DotProduction(Vector3 A, Vector3 B)
    {
        return A.x * B.x + A.y * B.y + A.z * B.z;
    }
    Vector3 CrossProduction(Vector3 A, Vector3 B)
    {
        Vector3 cross = new Vector3
            (
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x
            );
        return cross;
    }
    bool IsLeft(Vector3 foward, Vector3 targetDirection, Vector3 up)
    {
        Vector3 cross = CrossProduction(foward, targetDirection);
        return DotProduction(cross, up) < 0;
    }
}
