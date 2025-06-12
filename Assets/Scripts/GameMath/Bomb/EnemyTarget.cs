using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    int hp;

    void Awake() => hp = maxHP;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"{name} hit! HP = {hp}");

        if (hp <= 0)
            Destroy(gameObject);
    }
}