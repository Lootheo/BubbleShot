using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleBoardCreator : MonoBehaviour
{
    [Header("Board Config")]
    public GameObject BubblePrefab;
    public int BubbleHeight = 10;
    public int BubbleWidth = 10;
    public Vector2 StartingSpawnZone;
    public Vector2 BubbleSeparation;
    [Header("Bubble Materials")]
    public Material RedMaterial;
    public Material YellowMaterial;
    public Material BlueMaterial;
    public Material GreenMaterial;


    public static BubbleBoardCreator Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public List<Bubble> CreateBubbleBoard()
    {
        List<Bubble> bubbleBoard = new List<Bubble>();
        for (int j = 0; j < BubbleHeight; j++)
        {
            for (int i = 0; i < BubbleWidth; i++)
            {
                Vector2 bubbleSpawnPosition = new Vector2(StartingSpawnZone.x + BubbleSeparation.x * i, StartingSpawnZone.y - BubbleSeparation.y * j);
                if (j % 2 == 0)
                {
                    bubbleSpawnPosition = new Vector2(bubbleSpawnPosition.x + BubbleSeparation.x / 2, bubbleSpawnPosition.y);
                }
                bubbleBoard.Add(CreateRandomBubble(bubbleSpawnPosition));
            }
        }
        return bubbleBoard;
    }

    private bool oddLine = false;
    public List<Bubble> CreateNewBubbleLine()
    {
        List<Bubble> bubbleBoard = new List<Bubble>();
        for (int i = 0; i < BubbleWidth; i++)
        {
            Vector2 bubbleSpawnPosition = new Vector2(StartingSpawnZone.x + BubbleSeparation.x * i, StartingSpawnZone.y);
            if (oddLine)
                bubbleSpawnPosition = new Vector2(bubbleSpawnPosition.x + BubbleSeparation.x / 2, bubbleSpawnPosition.y);
            bubbleBoard.Add(CreateRandomBubble(bubbleSpawnPosition));
        }
        oddLine = !oddLine;
        return bubbleBoard;
    }

    public Bubble CreateRandomBubble(Vector2 bubbleSpawnPosition)
    {
        Bubble boardBubble = Instantiate(BubblePrefab, bubbleSpawnPosition, Quaternion.identity).GetComponent<Bubble>();
        BubbleColor randomBubbleColor = (BubbleColor)Random.Range(0, 4);
        boardBubble.SetColor(randomBubbleColor, GetBubbleMaterial(randomBubbleColor));
        return boardBubble;
    }

    public Material GetBubbleMaterial(BubbleColor bubbleColor)
    {
        switch (bubbleColor)
        {
            case BubbleColor.Green:
                return GreenMaterial;
            case BubbleColor.Yellow:
                return YellowMaterial;
            case BubbleColor.Red:
                return RedMaterial;
            case BubbleColor.Blue:
                return BlueMaterial;
            default:
                throw new ArgumentOutOfRangeException(nameof(bubbleColor), bubbleColor, null);
        }
    }


}
