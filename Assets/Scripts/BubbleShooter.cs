using System;
using System.Collections;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public Bubble NextBubble;
    public float ShootSpeed;
    public static BubbleShooter Instance;

    public bool CanShoot = true;
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        CreateNextBubble();
    }

    public void CreateNextBubble()
    {
        NextBubble = BubbleBoardCreator.Instance.CreateRandomBubble(this.transform.position);
    }

    private void Update()
    {
        if (CanShoot && Input.GetMouseButtonDown(0))
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        if (NextBubble != null)
        {
            Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 normalizedDirection = new Vector3(screenToWorldPoint.x, screenToWorldPoint.y, this.transform.position.z).normalized;
            if (normalizedDirection.y > 0) //Avoids shooting below!
            {
                NextBubble.StartShooting();
                Rigidbody bubbleRb = NextBubble.GetComponent<Rigidbody>();
                bubbleRb.velocity = normalizedDirection * ShootSpeed;
                NextBubble = null;
            }
        }
        else
        {
            Debug.Log("Waiting for turn tu finish");
        }
    }

}
