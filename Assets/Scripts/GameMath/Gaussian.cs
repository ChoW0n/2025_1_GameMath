using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaussian : MonoBehaviour
{
    public float mean;
    public float stdDev;
    float GaussianCall(float mean, float stdDev)        //정규 분포 계산 (가우시안 방식)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);  //표준 정규 분포
        return mean + stdDev * randStdNormal;       //원하는 평균과 표준편차로 변환
    }
    public void GaussianButton()
    {
        float gaussian = GaussianCall(mean, stdDev);
        Debug.Log($"Gaussian: {gaussian}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
