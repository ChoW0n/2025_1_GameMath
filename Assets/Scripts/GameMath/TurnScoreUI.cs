using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text p1ScoreText;
    [SerializeField] TMP_Text p2ScoreText;

    void Update()
    {
        var gm = BilliardGameManager.Instance;
        if (gm == null) return;

        turnText.text = gm.CurrentTurn == BallOwner.Player1 ? "Turn :  Player 1" : "Turn :  Player 2";
        p1ScoreText.text = $"P1  {gm.P1Score}";
        p2ScoreText.text = $"P2  {gm.P2Score}";
    }
}
