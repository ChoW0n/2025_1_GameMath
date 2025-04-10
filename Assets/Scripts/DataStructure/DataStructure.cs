using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStructure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //Q1
        int num = 12340;

        while (num > 0)
        {
            int r = num % 10;
            num = num / 10;
            Debug.Log(r);   //�� �ڸ� �� ���
        }


        //Q2
        //�ð� ���⵵ O(N)
        // N�� ������ �ڸ� ��, �� �ڸ����� ������ 1ȸ�� ������ ��
        int num2 = 987654321;       //9�ڸ� ���� -> N = 9
        int count = 0;

        while (num2 > 0)
        {
            int r = num2 % 10;
            num2 = num2 / 10;
            count++;        //�ݺ� Ƚ �� üũ
        }

        Debug.Log("�ڸ���(N): " + count);
        Debug.Log($"�ð� ���⵵: O({count})");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
