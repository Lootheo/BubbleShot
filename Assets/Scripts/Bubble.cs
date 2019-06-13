using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum BubbleColor { Green, Yellow, Red, Blue }
public class Bubble : MonoBehaviour
{
    [FormerlySerializedAs("Color")]
    [SerializeField]
    BubbleColor BubbleColor = BubbleColor.Red;

    public bool Shooting { get; private set; }

    public void StartShooting()
    {
        Shooting = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void SetColor(BubbleColor colorToSet, Material colorMaterial)
    {
        BubbleColor = colorToSet;
        GetComponent<MeshRenderer>().material = colorMaterial;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Shooting)
        {
            if (other.gameObject.GetComponent<Bubble>())
            {
                StartMatching();
            }
        }
    }

    private void StartMatching()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        HashSet<Bubble> matchedBubbles = GetNearMatches();
        BubbleBoard.Instance.GameBoard.Add(this);
        BubbleBoard.Instance.DestroyMatchedBubbles(matchedBubbles);
        Shooting = false;
    }

    private HashSet<Bubble> GetNearMatches(HashSet<Bubble> currentAccountedBubbles = null)
    {
        if (currentAccountedBubbles == null) currentAccountedBubbles = new HashSet<Bubble>();
        if (!currentAccountedBubbles.Contains(this)) currentAccountedBubbles.Add(this);
        var overlap = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider collidedBubble in overlap)
        {
            Bubble bubbleToCheck = collidedBubble.GetComponent<Bubble>();
            //collidedBubble.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
            if (bubbleToCheck==null || currentAccountedBubbles.Contains(bubbleToCheck)) continue;

            if (bubbleToCheck.BubbleColor == BubbleColor)
            {
                currentAccountedBubbles.UnionWith(bubbleToCheck.GetNearMatches(currentAccountedBubbles));
            }
        }
        return currentAccountedBubbles;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Wall")
        {
            Rigidbody bubbleRb = GetComponent<Rigidbody>();
            bubbleRb.velocity = new Vector3(-bubbleRb.velocity.x, bubbleRb.velocity.y);
        }
        else if (other.gameObject.name == "DefeatWall")
        {
            Debug.Log("Defeat!!");
            BubbleBoard.Instance.StartGame();
        }
    }
}
