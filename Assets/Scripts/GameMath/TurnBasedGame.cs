using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnBasedGameUI : MonoBehaviour
{
    [Header("전투 설정")]
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI resultTMP;     // 로그 출력 TMP
    [SerializeField] TextMeshProUGUI summaryTMP;    // 전투 종료 요약 TMP
    [SerializeField] Image summaryIMG;    // 전투 종료 요약 TMP

    [Header("전투 정보")]
    [SerializeField] int turn = 0;
    [SerializeField] float rareItemChance = 0.2f;
    bool rareItemObtained = false;
    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };
    [SerializeField] List<string> resultLog = new List<string>();

    // 전투 통계
    int totalHits = 0;
    int totalSuccessHits = 0;
    int totalCrits = 0;
    int totalEnemies = 0;
    int totalKills = 0;
    float maxDamage = float.MinValue;
    float minDamage = float.MaxValue;

    // 아이템 획득 통계
    Dictionary<string, int> rewardCount = new Dictionary<string, int>();
    int rareWeapon = 0;
    int rareArmor = 0;

    public void StartSimulation()
    {
        resultTMP.text = "";
        summaryTMP.text = "";
        summaryTMP.gameObject.SetActive(false);
        summaryIMG.gameObject.SetActive(false);

        resultLog.Clear();
        rareItemObtained = false;
        turn = 0;
        rareItemChance = 0.2f;

        totalHits = 0;
        totalSuccessHits = 0;
        totalCrits = 0;
        totalEnemies = 0;
        totalKills = 0;
        maxDamage = float.MinValue;
        minDamage = float.MaxValue;

        rewardCount.Clear();
        rareWeapon = 0;
        rareArmor = 0;

        StartCoroutine(SimulationRoutine());
    }

    IEnumerator SimulationRoutine()
    {
        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;
            rareItemChance += 0.1f;
            yield return StartCoroutine(DisplayResults());
        }

        resultLog.Add($"<color=#FFFF00><b>레어 아이템을 {turn}턴 만에 획득했습니다!</b></color>");
        yield return StartCoroutine(DisplayResults());

        ShowSummary();
    }

    void SimulateTurn()
    {
        resultLog.Add($"<color=#FFD700><b>===== 턴 {turn + 1} =====</b></color>");

        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemies += enemyCount;
        resultLog.Add($"등장한 적 수: {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalHits += maxHitsPerTurn;
            totalSuccessHits += hits;

            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCrits++;
                    resultLog.Add($"<color=#FF3B3B>크리티컬 공격! {damage:F1} 피해</color>");
                }
                else
                {
                    resultLog.Add($"<color=#AAAAAA>일반 공격: {damage:F1} 피해</color>");
                }

                maxDamage = Mathf.Max(maxDamage, damage);
                minDamage = Mathf.Min(minDamage, damage);

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                totalKills++;
                resultLog.Add($"<color=#00BFFF>적 {i + 1} 처치 완료 (총 피해량: {totalDamage:F1})</color>");

                string reward = rewards[Random.Range(0, rewards.Length)];
                if (!rewardCount.ContainsKey(reward))
                    rewardCount[reward] = 0;

                rewardCount[reward]++;
                resultLog.Add($"<color=#6AFF00>보상 획득: {reward}</color>");

                if (reward == "Weapon" && Random.value < rareItemChance)
                {
                    rareItemObtained = true;
                    rareWeapon++;
                    resultLog.Add($"<color=#FFA500><b>레어 무기 획득!</b></color>");
                }
                else if (reward == "Armor" && Random.value < rareItemChance)
                {
                    rareItemObtained = true;
                    rareArmor++;
                    resultLog.Add($"<color=#FFA500><b>레어 방어구 획득!</b></color>");
                }
            }
        }
    }

    IEnumerator DisplayResults()
    {
        foreach (string line in resultLog)
        {
            resultTMP.text = line;
            yield return new WaitForSeconds(1f);
        }
        resultLog.Clear();
    }

    void ShowSummary()
    {
        float hitRate = totalHits == 0 ? 0f : totalSuccessHits / (float)totalHits;
        float critRate = totalSuccessHits == 0 ? 0f : totalCrits / (float)totalSuccessHits;

        string summary =
            $"<color=#FFFF00><b>전투 결과</b></color>\n" +
            $"총 진행 턴 수 : {turn}\n" +
            $"발생한 적 : {totalEnemies}\n" +
            $"처치한 적 : {totalKills}\n" +
            $"공격 명중률 결과 : {(hitRate * 100f):F2}%\n" +
            $"발생한 치명타율 결과 : {(critRate * 100f):F2}%\n" +
            $"최대 데미지 : {maxDamage:F2}\n" +
            $"최소 데미지 : {minDamage:F2}\n\n" +
            $"<color=#FFFF00><b>획득한 아이템</b></color>\n";

        foreach (var pair in rewardCount)
        {
            bool isRare = pair.Key == "Weapon" || pair.Key == "Armor";
            int rareCount = pair.Key == "Weapon" ? rareWeapon : (pair.Key == "Armor" ? rareArmor : 0);
            int normalCount = pair.Value - rareCount;

            summary += $"{pair.Key} - 일반: {normalCount}개 / 레어: {rareCount}개\n";
        }

        summaryTMP.text = summary;
        summaryTMP.gameObject.SetActive(true);
        summaryIMG.gameObject.SetActive(true);
    }

    #region 분포 샘플 함수
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
        return mean + stdDev * z;
    }
    #endregion
}