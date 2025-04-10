using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GotchaSystem : MonoBehaviour
{
    private int RollCount;
    public TextMeshProUGUI resultText;
    public enum Rarity
    {
        C,      //40%
        B,      //30%
        A,      //20%
        S,      //10%
    }

    public void DrawOne()
    {
        RollCount++;
        Rarity result;
        if (RollCount < 10)
        {
            result = RollGotcha();
        }
        else
        {
            result = RollAorS();
            RollCount = 0;
        }
        resultText.text = $"[1È¸ »Ì±â °á°ú]\n{result}";

    }

    public void DrawTen()
    {
        List<Rarity> results = new List<Rarity>();
        for (int i = 0; i < 9; i++)
        {
            RollCount++;
            if (RollCount < 10)
            {
                results.Add(RollGotcha());
            }
            else
            {
                results.Add(RollAorS());
                RollCount = 0;
            }
            string output = "[10È¸ »Ì±â °á°ú]\n";
            foreach (var r in results)
            {
                output += $"{r} ";
            }
            resultText.text = output;
        }
    }

    private Rarity RollGotcha()
    {
        float rand = Random.value;

        if (rand < 0.4f) return Rarity.C;
        else if (rand < 0.7f) return Rarity.B;
        else if (rand < 0.9f) return Rarity.A;
        else return Rarity.S;
    }

    private Rarity RollAorS()
    {
        float rand = Random.value;
        return rand < 0.75f ? Rarity.A : Rarity.S;
    }
}
