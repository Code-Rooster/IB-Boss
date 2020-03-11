using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainerAI : MonoBehaviour
{
    public EntertainerHealth health;

    public Transform leftHand;
    public Transform rightHand;

    //public GameObject hand;
    public GameObject missile;

    private Transform player;

    public float fingerGunDuration;
    public float fingerGunMoveSpeed;
    public float fingerGunTimer = 0;
    public int fingerGunAmount;
    public float fingerGunVelocityFactor = 1.5f;
    public float cardBulletForce = 10;

    public GameObject cardBullet;

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

        leftHand = transform.Find("HandParent").transform.Find("LeftHand1").transform;
        rightHand = transform.Find("HandParent").transform.Find("RightHand1").transform;

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
                StartCoroutine(AimFingerGuns());
                StartCoroutine(ShootFingerGuns());
                break;
            default:
                StartCoroutine(AimFingerGuns());
                StartCoroutine(ShootFingerGuns());
                break;
        }
    }

    private IEnumerator AimFingerGuns()
    {
        fingerGunTimer = 0;

        while (fingerGunTimer < fingerGunDuration)
        {
            float leftAngle = Mathf.Atan2((player.position.y + player.GetComponent<Rigidbody2D>().velocity.y / fingerGunVelocityFactor)
                - leftHand.position.y, 
                (player.position.x + player.GetComponent<Rigidbody2D>().velocity.x / fingerGunVelocityFactor)
                - leftHand.position.x) * Mathf.Rad2Deg;
            float rightAngle = Mathf.Atan2((player.position.y - player.GetComponent<Rigidbody2D>().velocity.y / fingerGunVelocityFactor)
                - rightHand.position.y,
                (player.position.x - player.GetComponent<Rigidbody2D>().velocity.x / fingerGunVelocityFactor)
                - rightHand.position.x) * Mathf.Rad2Deg;

            rightHand.transform.rotation = Quaternion.Lerp(rightHand.transform.rotation,
                Quaternion.Euler(new Vector3(0, 0, rightAngle + 90)), 
                fingerGunMoveSpeed * Time.deltaTime);

            leftHand.transform.rotation = Quaternion.Lerp(leftHand.transform.rotation,
                Quaternion.Euler(new Vector3(0, 0, leftAngle + 90)),
                fingerGunMoveSpeed * Time.deltaTime);

            fingerGunTimer += Time.deltaTime;

            yield return null;
        }

        print("Time's up!");
    }

    private IEnumerator ShootFingerGuns()
    {
        float amount = fingerGunAmount;

        bool leftHandShotLast = false;

        while (fingerGunTimer < fingerGunDuration)
        {
            Transform handPos = null;

            if (!leftHandShotLast)
            {
                if (GameObject.Find("LeftHand1") != null)
                {
                    handPos = GameObject.Find("LeftHand1").transform;

                    leftHandShotLast = true;
                }
                else if (GameObject.Find("RightHand1") != null)
                {
                    handPos = GameObject.Find("RightHand1").transform;
                }
            }
            else
            {
                if (GameObject.Find("RightHand1") != null)
                {
                    handPos = GameObject.Find("RightHand1").transform;

                    leftHandShotLast = false;
                }
                else if (GameObject.Find("LeftHand1") != null)
                {
                    handPos = GameObject.Find("LeftHand1").transform;
                }
            }

            GameObject shotBullet = Instantiate(cardBullet, handPos.position, handPos.rotation);

            shotBullet.GetComponent<Rigidbody2D>().AddForce(-shotBullet.transform.up * cardBulletForce);

            yield return new WaitForSeconds(fingerGunDuration / amount);
        }
    }

    public void StartHandMissile()
    {
        //Vector3 handPos = hand.transform.position;

        //GameObject handMissile = Instantiate(missile, handPos, Quaternion.identity);

        //hand.SetActive(false);
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
