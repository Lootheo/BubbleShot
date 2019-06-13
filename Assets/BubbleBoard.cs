using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BubbleBoard : MonoBehaviour
{
    [HideInInspector]
    public List<Bubble> GameBoard;
    public static BubbleBoard Instance { get; private set; }
    public int Score {
        get => score;
        private set {
            score = value;
            scoreChanged.Invoke(score);
        }
    }
    public int TurnsLeft {
        get => turnsLeft;
        private set {
            turnsLeft = value;
            turnsLeftChanged.Invoke(turnsLeft);
        }
    }
    private int score;
    private int turnsLeft;
    private Action<int> scoreChanged;
    private Action<int> turnsLeftChanged;
    

    public void StartGame()
    {
        DeleteBoard();
        GameBoard = BubbleBoardCreator.Instance.CreateBubbleBoard();
        foreach (Bubble bubble in GameBoard)
        {
            bubble.transform.parent = this.transform;
        }
        TurnsLeft = 24;
        BubbleShooter.Instance.CanShoot = true;
    }

    private void DeleteBoard()
    {
        foreach (Bubble bubble in GameBoard)
        {
            Destroy(bubble.gameObject);
        }
        GameBoard.Clear();
    }
    public void CreateNewBubbleLine()
    {
        foreach (Bubble bubble in GameBoard)
        {
            float bubbleSeparationY = BubbleBoardCreator.Instance.BubbleSeparation.y;
            Vector3 bubblePosition = bubble.transform.position;
            bubble.transform.position = new Vector3(bubblePosition.x, bubblePosition.y - bubbleSeparationY, bubblePosition.z);
        }
        GameBoard.AddRange(BubbleBoardCreator.Instance.CreateNewBubbleLine());
        foreach (Bubble bubble in GameBoard)
        {
            bubble.transform.parent = this.transform;
        }
    }
    public void DestroyMatchedBubbles(HashSet<Bubble> matchedBubbles)
    {
        //A match ocurred
        if (matchedBubbles.Count > 2)
        {
            StartCoroutine(DestroyMatchedBubblesCoroutine(matchedBubbles));
        }
        TurnsLeft--;
        BubbleShooter.Instance.CreateNextBubble();
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartGame();
    }
    private IEnumerator DestroyMatchedBubblesCoroutine(HashSet<Bubble> matchedBubbles)
    {
        foreach (Bubble matchedBubble in matchedBubbles)
        {
            yield return new WaitForSeconds(0.1f);
            if (matchedBubble != null)
            {
                GameBoard.Remove(matchedBubble);
                Destroy(matchedBubble.gameObject);
                Score++;
            }
        }
    }
    public void AddScoreUpdateEvent(Action<int> scoreChanged)
    {
        this.scoreChanged += scoreChanged;
    }
    public void RemoveScoreUpdateEvent(Action<int> scoreChanged)
    {
        this.scoreChanged += scoreChanged;
    }
    public void AddTurnsUpdateEvent(Action<int> turnsLeftChanged)
    {
        this.turnsLeftChanged += turnsLeftChanged;
    }
    public void RemoveTurnsUpdateEvent(Action<int> turnsLeftChanged)
    {
        this.scoreChanged += turnsLeftChanged;
    }

    
}
