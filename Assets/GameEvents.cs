using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public int TurnsForNewLine = 5;
    // Start is called before the first frame update
    void Start()
    {
        BubbleBoard.Instance.AddTurnsUpdateEvent(CheckTurnsEvent);
        BubbleBoard.Instance.AddScoreUpdateEvent(CheckScoreEvent);
    }

    private void CheckScoreEvent(int currentScore)
    {
        if (currentScore == 9)
        {
            Debug.Log("YOUR SCORE IS OVER 9!!!");
        }
    }
    private void CheckTurnsEvent(int turnsLeft)
    {
        if (turnsLeft % TurnsForNewLine == 0)
        {
            BubbleBoard.Instance.CreateNewBubbleLine();
        }
        if (turnsLeft <= 0)
        {
            Debug.Log("Game Finished!");
            BubbleBoard.Instance.StartGame();
        }
    }

}
