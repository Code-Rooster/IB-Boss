using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainerAI : MonoBehaviour
{
    public EntertainerHealth health;

    public Transform leftHand;
    private Transform rightHand;

    public GameObject hand;
    public GameObject missile;

    private Transform player;

    public float fingerGunDuration;

    public int fingerGunAmount;

    private bool startedBattle = false;

    public bool phaseOne = false;
    public bool phaseTwo = false;
    public bool phaseThree = false;

    public bool isAttacking = false;
    public bool rabbitWallsOut = false;

    public List<string> possibleAttacks = new List<string>();
    private List<string> closedAttacks = new List<string>();

    private enum allAttacks {FingerGuns, Teleport, RabbitWall, HandMissiles};

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        leftHand = transform.Find("HandParent").transform.Find("LeftHand").transform;
        rightHand = transform.Find("HandParent").transform.Find("RightHand").transform;

        health = gameObject.GetComponent<EntertainerHealth>();

        StartBattle();
    }

    public void StartBattle()
    {
        startedBattle = true;
        phaseOne = true;

        possibleAttacks.Add(allAttacks.FingerGuns.ToString());
        possibleAttacks.Add(allAttacks.Teleport.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DecideAttack();
        }

        if (health.currentHealth <= health.maxHealth / 2 && startedBattle && !phaseTwo && !phaseThree)
        {
            phaseTwo = true;

            possibleAttacks.Add(allAttacks.RabbitWall.ToString());
        }

        if (health.currentHealth <= health.maxHealth / 4 && startedBattle && !phaseThree)
        {
            phaseThree = true;
        }
    }

    public void DecideAttack()
    {
        string decidedAttack = allAttacks.HandMissiles.ToString();
        int attackIndex = -1;

        if (possibleAttacks.Count > 0)
        {
            attackIndex = Random.Range(0, possibleAttacks.Count);
            decidedAttack = possibleAttacks[attackIndex];

            closedAttacks.Add(decidedAttack);

            possibleAttacks.RemoveAt(attackIndex);
        }
        else
        {
            for (int i = 0; i < closedAttacks.Count; i++)
            {
                possibleAttacks.Add(closedAttacks[i]);
            }

            closedAttacks.Clear();
        }

        switch (decidedAttack)
        {
            case "FingerGuns":
                StartCoroutine(FingerGuns());
                break;
            default:
                print("Default");
                break;
        }
    }

    private IEnumerator FingerGuns()
    {
        float timer = 0;

        while (timer < fingerGunDuration)
        {
            float leftAngle = Mathf.Atan2(player.position.y - leftHand.position.y, player.position.x - leftHand.position.y) * Mathf.Rad2Deg;
            float rightAngle = Mathf.Atan2(player.position.y - rightHand.position.y, player.position.x - rightHand.position.y) * Mathf.Rad2Deg;

            print("Left angle: " + leftAngle + ", Right angle: " + rightAngle);

            
            rightHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rightAngle + 90));

            leftHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, leftAngle + 90));

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public void StartHandMissile()
    {
        Vector3 handPos = hand.transform.position;

        GameObject handMissile = Instantiate(missile, handPos, Quaternion.identity);

        hand.SetActive(false);
    }

    public void SetIsAttackingTrue()
    {
        isAttacking = true;
    }

    public void SetIsAttackingFalse()
    {
        isAttacking = false;
    }
}
