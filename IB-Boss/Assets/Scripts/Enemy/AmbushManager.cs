using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushManager : MonoBehaviour
{
    public Transform player;

    public GameObject node;
    public GameObject playerNode;
    public GameObject lastPlayerNode;

    public List<GameObject> enemiesInFormation = new List<GameObject>();
    public List<GameObject> flanks = new List<GameObject>();
    public List<GameObject> attackers = new List<GameObject>();

    public List<GameObject> nodes = new List<GameObject>();

    public float enemyPathfindDelay;

    public int sqrtTotalSquares;

    public bool formationStarted;

    public bool phase1 = true;
    public bool phase2;
    public bool phase3;
    private bool startedPathfinding = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        GenerateNodes();
        AssignNodeIndices();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateNodes();
        }

        if (playerNode != null && !startedPathfinding)
        {
            StartCoroutine(PathfindManager());
        }

        if (playerNode != lastPlayerNode && lastPlayerNode != null)
        {
            playerNode.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            lastPlayerNode.transform.Find("PlayerIndicator").gameObject.SetActive(false);

            for (int i = 0; i < nodes.Count; i++)
            {
                Node currentNode = nodes[i].GetComponent<Node>();
                if (currentNode != null)
                {
                    //print("Calculating Distance");

                    currentNode.CalculateDistance();
                }
            }
        }

        lastPlayerNode = playerNode;
    }

    public void GenerateNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Destroy(nodes[i]);
        }

        nodes.Clear();

        Vector3 playerPos = player.position;

        for (int x = Mathf.RoundToInt(transform.Find("UpperLeftBound").transform.position.x); x < transform.Find("LowerRightBound").transform.position.x; x++)
        {
            if (x % 2 == 0)
            {
                for (int y = Mathf.RoundToInt(transform.Find("LowerRightBound").transform.position.y); y < transform.Find("UpperLeftBound").transform.position.y; y++)
                {
                    if (y % 2 == 0)
                    {
                        GameObject newNode = Instantiate(node, new Vector3(x, y, 0), node.transform.rotation);

                        newNode.name = "Node (" + x + ", " + y + ")";
                        newNode.GetComponent<Node>().coords = new Vector2(x, y);

                        newNode.transform.parent = transform;
                        nodes.Add(newNode);
                    }
                }
            }
        }
    }

    private void AssignNodeIndices()
    {
        for (int a = 0; a < GameObject.FindGameObjectsWithTag("Enemy").Length; a++)
        {
            GameObject.FindGameObjectsWithTag("Enemy")[a].GetComponent<EnemyAI>().nodeIndex = a;

            for (int b = 0; b < nodes.Count; b++)
            {
                nodes[b].GetComponent<Node>().g.Add(0);
                nodes[b].GetComponent<Node>().parentNode.Add(null);
            }
        }
    }

    private IEnumerator PathfindManager()
    {
        startedPathfinding = true;

        List<EnemyAI> enemies = new List<EnemyAI>();
        for (int e = 0; e < GameObject.FindGameObjectsWithTag("Enemy").Length; e++)
        {
            enemies.Add(GameObject.FindGameObjectsWithTag("Enemy")[e].GetComponent<EnemyAI>());
        }

        while (true)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].FindPath();

                yield return new WaitForSeconds(enemyPathfindDelay);
            }
        }
    }
}
