using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private TextMeshProUGUI TurnsText;


    public void Start()
    {
        BubbleBoard.Instance.AddScoreUpdateEvent(UpdateScore);
        BubbleBoard.Instance.AddTurnsUpdateEvent(UpdateTurnsLeft);
    }

    public void UpdateTurnsLeft(int turnsLeft)
    {
        TurnsText.text = "Turns: " + turnsLeft;
    }
    public void UpdateScore(int currentScore)
    {
        ScoreText.text = "Score: " + currentScore;
    }

    public void OnDestroy()
    {
        BubbleBoard.Instance.RemoveScoreUpdateEvent(UpdateScore);
        BubbleBoard.Instance.RemoveTurnsUpdateEvent(UpdateTurnsLeft);
    }
}
