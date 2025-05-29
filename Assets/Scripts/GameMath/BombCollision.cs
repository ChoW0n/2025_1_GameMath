using UnityEngine;

public class BombCollision : MonoBehaviour
{
    public LayerMask enemyLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log("폭탄 명중 대상: " + other.name);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}