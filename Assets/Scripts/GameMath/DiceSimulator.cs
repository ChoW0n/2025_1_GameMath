using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.TextCore;
using UnityEngine;

public class DiceSimulator : MonoBehaviour
{
    public int sides = 6;
    int[] counts = new int[6];
    public int trials = 100;
    public TextMeshProUGUI[] labels = new TextMeshProUGUI[6];
    public void RollDice()
    {
        counts = new int[sides];
        for (int i = 0; i < trials; i++)
        {
            int result = Random.Range(1, sides + 1);
            counts[result - 1]++;
        }

        for (int i = 0; i < counts.Length; i++)
        {
            float percent = (float)counts[i] / trials * 100f;
            string result = ($"{i + 1}: {counts[i]} ({percent:F2}%)");
            labels[i].text = result;

        }
    }
}
