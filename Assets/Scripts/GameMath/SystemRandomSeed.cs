using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRandomSeed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Net. 스타일 시드 고정 난수
        System.Random rnd = new System.Random(1234);    //항상 같은 순서로 출력됨
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(rnd.Next(1, 7));  //1~6 사이의 정수
        }

        //Unity 에서의 시드 고정 난수
        Random.InitState(1234);     //Unity 난수 시드 고정
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(Random.Range(1, 7));  //1~6 사이의 난수
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
