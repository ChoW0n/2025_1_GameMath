using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StandardDeviation : MonoBehaviour
{
    public int sampleCount = 0;
    public float minRange;
    public float maxRange;

    private void Start()
    {
        StandardDeviationCall();
    }
    void StandardDeviationCall()
    {
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Random.Range(minRange, maxRange);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / sampleCount);

        Debug.Log($"평균: {mean}, 표준편차: {stdDev}");
    }
}
