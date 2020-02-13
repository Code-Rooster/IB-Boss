using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public LoadScene lS;
    public YesNoEvents yNE;

    public GameObject player;

    private string lastScene;

    public float maxHealth = 2;
    public float currentHealth;

    private bool isDying = false;

    public int deathCount;

    private void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player");

        if (GameObject.FindGameObjectWithTag("SceneLoader") != null)
        {
            lS = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadScene>();
        }
        yNE = GameObject.FindGameObjectWithTag("DM").GetComponent<YesNoEvents>();

        print("Test" + yNE);
    }

    private void Update()
    {
        if (yNE.responses[2] != 0)
        {
            if (yNE.responses[2] == -1)
            {
                SceneManager.LoadScene("StartScene");
            }
            else if (isDying)
            {
                Continue();
            }
        }

        if (currentHealth <= 0 && !isDying)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoseHealth(1);
        }
    }

    public void LoseHealth(float amount)
    {
        currentHealth -= amount;
    }

    public void Die()
    {
        isDying = true;

        lastScene = SceneManager.GetActiveScene().name;

        deathCount++;

        StartCoroutine(lS.LoadAScene("Death"));

        if (player != null)
        {
            player.SetActive(false);
        }
    }

    public void Continue()
    {
        yNE.responses[2] = 0;

        GameObject.Find("Mystery Voice").GetComponent<DeathCutscene>().started = false;

        isDying = false;

        print("Continuing.");

        currentHealth = maxHealth;

        StartCoroutine(lS.LoadAScene(lastScene.ToString()));

        if (lS != null)
        {
            lS = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadScene>();
        }

        player.SetActive(true);
    }
}
