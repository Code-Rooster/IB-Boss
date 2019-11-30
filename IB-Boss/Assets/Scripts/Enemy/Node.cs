using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private AmbushManager aM;

    public List<Node> adjacents = new List<Node>();

    public List<int> g = new List<int>();

    public List<Node> parentNode = new List<Node>();

    public Vector2 coords;

    public int distFromPlayer;

    private void Start()
    {
        aM = GameObject.FindGameObjectWithTag("Manager").GetComponent<AmbushManager>();

        GetAdjacents();
    }

    private void GetAdjacents()
    {
        adjacents.Clear();

        Vector2 above = new Vector2(coords.x, coords.y + 2);
        Vector2 below = new Vector2(coords.x, coords.y - 2);
        Vector2 left = new Vector2(coords.x - 2, coords.y);
        Vector2 right = new Vector2(coords.x + 2, coords.y);

        GameObject aboveSquare = GameObject.Find("Node (" + above.x + ", " + above.y + ")");
        GameObject belowSquare = GameObject.Find("Node (" + below.x + ", " + below.y + ")");
        GameObject leftSquare = GameObject.Find("Node (" + left.x + ", " + left.y + ")");
        GameObject rightSquare = GameObject.Find("Node (" + right.x + ", " + right.y + ")");

        if (aboveSquare != null)
        {
            adjacents.Add(aboveSquare.GetComponent<Node>());
        }
        if (belowSquare != null)
        {
            adjacents.Add(belowSquare.GetComponent<Node>());
        }
        if (leftSquare != null)
        {
            adjacents.Add(leftSquare.GetComponent<Node>());
        }
        if (rightSquare != null)
        {
            adjacents.Add(rightSquare.GetComponent<Node>());
        }
    }

    public void CalculateDistance()
    {
        Vector2 playerNodeCoords = aM.playerNode.GetComponent<Node>().coords;

        distFromPlayer = Mathf.RoundToInt(Mathf.Abs(coords.x - playerNodeCoords.x) + Mathf.Abs(coords.y - playerNodeCoords.y) - 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((GlobalManager.Instance.nodeCollision & 1 << col.gameObject.layer) != 0)
        {
            if (aM.nodes != null && aM.nodes.Contains(gameObject))
            {
                aM.nodes.Remove(gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            aM.playerNode = gameObject;
        }

        EnemyAI eA = col.GetComponent<EnemyAI>();

        if (col.tag == "Enemy" && eA != null)
        {
            eA.currentSquare = gameObject;
        }
    }
}
