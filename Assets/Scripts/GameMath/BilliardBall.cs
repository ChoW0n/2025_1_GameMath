using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BilliardBall : MonoBehaviour
{
    public BallOwner owner = BallOwner.None;
    public bool isCue = false;               // 큐볼 여부

    void OnCollisionEnter(Collision col)
    {
        var gm = BilliardGameManager.Instance;
        if (gm == null) return;

        BilliardBall other = col.collider.GetComponent<BilliardBall>();
        if (other == null) return;

        gm.RegisterCollision(other);
    }
}