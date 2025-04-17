using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CritRate : MonoBehaviour
{
    public TextMeshProUGUI critTMP;
    float critRate = 0.1f;

    private int totalCrit;
    private int totalAttack;
    private bool pastCrit = false;
    
    // Start is called before the first frame update
    public void TryHit()
    {
        totalAttack++;

        float currentRate = totalCrit / (float)totalAttack;

        bool isCritical;

        //보정 적용
        if (pastCrit == true && currentRate < critRate)
        {
            //무조건 발생
            isCritical = true;
        }
        else if (pastCrit == false && critRate > currentRate)
        {
            //강제로 막기
            isCritical = false;
        }
        else
        {
            //정상적인 크리티컬
            isCritical = Random.value < critRate;
        }
        if (isCritical)
        {
            pastCrit = true;
            totalCrit++;
        }
        else
        {
            pastCrit = false;
        }


        float actualCrit = (totalCrit == 0) ? 0f : (float)totalCrit / totalAttack;
        critTMP.text = $"치명타: {totalCrit}| 전체 공격: {totalAttack} | 치명타율: {actualCrit*100f:F2}% {critRate},{currentRate}";

    }
}
