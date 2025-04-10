using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class CombatSimulator : MonoBehaviour
{
    int level = 1;
    float baseDamage => level * 20f;

    private int totalCrit;
    private int totalAttack;
    private bool pastCrit = false;
    private bool isCritical;

    Weapon currentWeapon;
    int attackCount = 0;
    float totalDamage = 0f;

    //데미지 ^이쁘게^ 출력용
    string damageText = "";
    Color damageTextColor = Color.black;
    float damageDisplayTime = 1.0f;
    float damageTimer = 0f;
    public GameObject dummy;


    Dictionary<string, Weapon> weaponData = new Dictionary<string, Weapon>
    {
        { "dagger", new Weapon("단검", 5f, 0.4f, 1.5f) },
        { "sword", new Weapon("장검", 10f, 0.3f, 2.0f) },
        { "axe",   new Weapon("도끼", 20f, 0.2f, 3.0f) }
    };

    void Start()
    {
        EquipWeapon("dagger");
    }
    void Update()
    {
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    void EquipWeapon(string weaponKey)
    {
        currentWeapon = weaponData[weaponKey];
        attackCount = 0;
        totalDamage = 0f;
    }

    float GenerateGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    void CritCall(ref float damage)
    {
        float currentRate = totalCrit / (float)totalAttack;
        //보정 적용
        if (pastCrit = true && currentRate < currentWeapon.critChance)
        {
            //무조건 발생
            isCritical = true;
        }
        else if (pastCrit = false && currentWeapon.critChance > currentRate)
        {
            //강제로 막기
            isCritical = false;
        }
        else
        {
            //정상적인 크리티컬
            isCritical = Random.value < currentWeapon.critChance;
        }
        if (isCritical)
        {
            damage *= currentWeapon.critMultiplier;
            pastCrit = true;
            totalCrit++;
        }
        else
        {
            pastCrit = false;
        }
    }
    void SimulateAttack()
    {
        float damage = GenerateGaussian(baseDamage, currentWeapon.stdDev);
        CritCall(ref damage);

        attackCount++;
        totalDamage += damage;

        //출력용 텍스트 설정
        damageText = $"{(int)damage}";
        damageTextColor = isCritical ? Color.red : Color.black;
        damageTimer = damageDisplayTime;

        Debug.Log($"[공격 {attackCount}회차] 피해량: {damage:F1} {(isCritical ? "(치명타!)" : "")}");
        if ((int)damage >= 1)
        {
            dummy.GetComponent<HitEffect>().PlayHitEffect();
        }
    }

    void OnGUI()
    {
        GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
        myLabelStyle.fontSize = 16;
        myLabelStyle.normal.textColor = Color.black;

        GUILayout.BeginArea(new Rect(20, 20, 300, 500));
        GUILayout.Label("<b><size=16>데미지 시뮬레이터</size></b>");
        GUILayout.Space(10);

        GUILayout.Label($"레벨: {level}");
        if (GUILayout.Button("Level Up")) level++;

        GUILayout.Space(10);
        GUILayout.Label("<b>무기 선택</b>");
        if (GUILayout.Button("단검")) EquipWeapon("dagger");
        if (GUILayout.Button("장검")) EquipWeapon("sword");
        if (GUILayout.Button("도끼")) EquipWeapon("axe");

        GUILayout.Space(10);
        GUILayout.Label($"현재 무기: {currentWeapon.name}", myLabelStyle);
        GUILayout.Label($"기본 데미지: {baseDamage}", myLabelStyle);
        GUILayout.Label($"치명타 확률: {currentWeapon.critChance * 100}%", myLabelStyle);
        GUILayout.Label($"치명타 배율: {currentWeapon.critMultiplier}배", myLabelStyle);

        GUILayout.Space(10);
        if (GUILayout.Button("공격")) SimulateAttack();

        GUILayout.Space(10);
        GUILayout.Label($"총 공격 횟수: {attackCount}", myLabelStyle);
        GUILayout.Label($"누적 데미지: {totalDamage:F1}", myLabelStyle);
        if (attackCount > 0)
            GUILayout.Label($"평균 데미지: {(totalDamage / attackCount):F2}", myLabelStyle);

        GUILayout.EndArea();

        if (damageTimer > 0)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 40;
            style.normal.textColor = damageTextColor;
            style.alignment = TextAnchor.MiddleCenter;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            GUI.Label(new Rect(screenWidth / 2 - 100, screenHeight / 2 - 50, 200, 100), damageText, style);
        }
    }
}