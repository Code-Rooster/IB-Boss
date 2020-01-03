using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public EnemyHealth eH;
    private AmbushManager aM;

    public GameObject currentSquare;
    private Node currentlyConsidering;

    //Evaluated means adjacent to the currently chosen square
    //h(n) = node.distFromPlayer
    //g(n) = number of squares from start (Use alreadyVisited.Count?)
    //f(n) = g(n) + h(n) (Score, minimize this)
    //Up, Down, Left, Right

    public List<Node> bestPath = new List<Node>();

    public enum PersonalityTypes { Bold, Tough, Brave, Greedy, Hasty };

    public PersonalityTypes personality;

    public float baseSpeed;
    public float baseAttack;
    private float speed;
    private float attack;
    private float fearThreshold;
    private float maxWanderTimer;
    private float currentWanderTimer;
    private float maxWaitTimer;
    private float currentWaitTimer;

    public int nodeIndex;

    public bool inFormation = false;

    public GameObject player;

    public Vector3 target;

    private void Start()
    {
        eH = gameObject.GetComponent<EnemyHealth>();
        aM = GameObject.FindGameObjectWithTag("Manager").GetComponent<AmbushManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        DetSpeed();
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (aM.playerNode == aM.lastPlayerNode)
            {
                FindPath();
            }
        }

        if (aM.phase1)
        {
            switch (personality)
            {
                case PersonalityTypes.Bold:
                    Wander();
                    break;
                case PersonalityTypes.Tough:
                    StartFormation();
                    break;
                case PersonalityTypes.Brave:
                    Wander();
                    break;
                case PersonalityTypes.Greedy:
                    Wander();
                    break;
                case PersonalityTypes.Hasty:
                    Wander();
                    break;
            }
        }
        if (aM.phase2)
        {
            switch (personality)
            {
                case PersonalityTypes.Bold:
                    JoinFormation();
                    Chase();
                    break;
                case PersonalityTypes.Tough:
                    JoinFormation();
                    Chase();
                    break;
                case PersonalityTypes.Brave:
                    JoinFormation();
                    Chase();
                    break;
                case PersonalityTypes.Greedy:
                    JoinFormation();
                    Chase();
                    break;
                case PersonalityTypes.Hasty:
                    JoinFormation();
                    Chase();
                    break;
            }
        }
        if (aM.phase3)
        {
            switch (personality)
            {
                case PersonalityTypes.Bold:
                    break;
                case PersonalityTypes.Tough:
                    OrganizeFormation();
                    break;
                case PersonalityTypes.Brave:
                    break;
                case PersonalityTypes.Greedy:
                    break;
                case PersonalityTypes.Hasty:
                    break;
            }
        }
    }

    public void FindPath()
    {
        for (int i = 0; i < bestPath.Count; i++)
        {
            if (bestPath[i] != null)
            {
                bestPath[i].transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }
        }

        Node playerNode = aM.playerNode.GetComponent<Node>();

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        open.Add(currentSquare.GetComponent<Node>());

        bestPath.Clear();

        int loops = 0;

        while ((open.Count >= 1 || loops == 0))
        {
            List<Node> orderedOpen = new List<Node>();
            orderedOpen = open.OrderBy(x => x.distFromPlayer + x.g[nodeIndex]).ToList();

            currentlyConsidering = orderedOpen.First();

            if (open.Contains(playerNode))
            {
                break;
            }

            open.Remove(currentlyConsidering);
            closed.Add(currentlyConsidering);

            List<Node> currentAdjacents = new List<Node>();
            currentAdjacents = currentlyConsidering.adjacents;

            for (int c = 0; c < currentAdjacents.Count; c++)
            {
                Node currentAdjacent = currentAdjacents[c];

                bool newGShorter = false;
                if (closed.Contains(currentAdjacent) || currentAdjacent == null)
                {
                    continue;
                }

                if (open.IndexOf(currentAdjacents[c]) != -1)
                {
                    if (open[open.IndexOf(currentAdjacents[c])].g[nodeIndex] > currentAdjacent.g[nodeIndex])
                    {
                        newGShorter = true;
                    }
                    else
                    {
                        newGShorter = false;
                    }
                }

                if (!open.Contains(currentAdjacent) || newGShorter)
                {
                    currentAdjacent.g[nodeIndex] = currentlyConsidering.g[nodeIndex] + 1;
                    currentAdjacent.parentNode[nodeIndex] = currentlyConsidering;

                    if (!open.Contains(currentAdjacent))
                    {
                        open.Add(currentAdjacent);
                    }
                }
            }

            loops++;
        }

        Node findPathNode = open.Last();

        if (findPathNode != playerNode)
        {
            Debug.LogError("ERROR: ENDING NODE IS NOT THE PLAYER NODE.");
            //return;
        }

        int findPathLoops = 0;

        while (findPathNode != null && findPathLoops < closed.Count)
        {
            bestPath.Add(findPathNode);

            findPathNode = findPathNode.parentNode[nodeIndex];

            findPathLoops++;
        }

        for (int i = 0; i < bestPath.Count; i++)
        {
            float red = i / (float)bestPath.Count;
            float blue = 1 - i / (float)bestPath.Count;

            if (bestPath[i] != null)
            {
                bestPath[i].transform.Find("PlayerIndicator").gameObject.SetActive(true);


                Color32 spriteColor = new Color(red, 0, blue, 1);
                bestPath[i].transform.Find("PlayerIndicator").gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
            }
        }
    }

    private void DetSpeed()
    {
        speed = baseSpeed;

        switch (personality)
        {
            case PersonalityTypes.Bold:
                break;
            case PersonalityTypes.Tough:
                break;
            case PersonalityTypes.Brave:
                break;
            case PersonalityTypes.Greedy:
                break;
            case PersonalityTypes.Hasty:
                break;
        }
    }

    private void Wander()
    {
        speed = baseSpeed / 2;

        if (currentWanderTimer < maxWanderTimer)
        {
            currentWanderTimer += Time.deltaTime;
        }
        else if (currentWaitTimer < maxWaitTimer)
        {
            currentWaitTimer += Time.deltaTime;

            target = transform.position;
        }
        else
        {
            maxWanderTimer = Random.Range(1f, 5f);
            maxWaitTimer = Random.Range(1f, 5f);

            currentWaitTimer = 0f;
            currentWanderTimer = 0f;

            target = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
        }

    }

    private void Chase ()
    {
        if (bestPath.Count > 0)
        {
            for (int i = 0; i < bestPath.Count; i++)
            {
                if (bestPath[i] != null)
                {
                    target = bestPath[i].transform.position;
                }
            }
        }
    }

    private void StartFormation()
    {
        DetSpeed();

        aM.formationStarted = true;
    }

    private void OrganizeFormation()
    {
        DetSpeed();

        aM.flanks = aM.enemiesInFormation.OrderByDescending(x => x.GetComponent<EnemyAI>().speed).ToList();

        aM.flanks.RemoveRange(Mathf.RoundToInt(aM.flanks.Count / 2), aM.flanks.Count - Mathf.RoundToInt(aM.flanks.Count / 2));

        for (int i = 0; i < aM.enemiesInFormation.Count; i++)
        {
            if (!aM.flanks.Contains(aM.enemiesInFormation[i]))
            {
                aM.attackers.Add(aM.enemiesInFormation[i]);
            }
        }
    }

    private void FormationAttack()
    {
        DetSpeed();
    }

    private void JoinFormation()
    {
        DetSpeed();

        if (aM.formationStarted && !aM.enemiesInFormation.Contains(gameObject))
        {
            aM.enemiesInFormation.Add(gameObject);
        }

        target = transform.position;
    }
}
