using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainerAI : MonoBehaviour
{
    public EntertainerHealth health;

    public HandAI leftHandAI;
    public HandAI rightHandAI;

    private bool startedBattle = false;

    public bool phaseOne = false;
    public bool phaseTwo = false;
    public bool phaseThree = false;

    private void Start()
    {
        health = gameObject.GetComponent<EntertainerHealth>();

        StartBattle();
    }

    public void StartBattle()
    {
        startedBattle = true;
        phaseOne = true;
        phaseTwo = false;
        phaseThree = false;
    }

    private void Update()
    {
        if (health.currentHealth <= health.maxHealth / 2 && startedBattle && !phaseTwo && !phaseThree)
        {
            StartPhaseTwo();
        }

        if (health.currentHealth <= health.maxHealth / 4 && startedBattle && !phaseThree)
        {
            StartPhaseThree();
        }
    }

    private void StartPhaseTwo()
    {
        phaseOne = false;
        phaseTwo = true;
        phaseThree = false;
    }

    private void StartPhaseThree()
    {
        phaseOne = false;
        phaseTwo = false;
        phaseThree = true;

        leftHandAI.BeefUp();
        rightHandAI.BeefUp();
    }
}
