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
            Debug.Log(r);   //한 자리 씩 출력
        }


        //Q2
        //시간 복잡도 O(N)
        // N은 숫자의 자리 수, 각 자리마다 연산을 1회씩 수행을 함
        int num2 = 987654321;       //9자리 숫자 -> N = 9
        int count = 0;

        while (num2 > 0)
        {
            int r = num2 % 10;
            num2 = num2 / 10;
            count++;        //반복 횟 수 체크
        }

        Debug.Log("자릿수(N): " + count);
        Debug.Log($"시간 복잡도: O({count})");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
