using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Health : MonoBehaviour
{
    public LoadScene lS;
    public YesNoEvents yNE;

    public GameObject player;

    public TMPro.TMP_Text healthText;
    public Image healthBar;

    public SpriteRenderer sR;

    public Color32 hurtColor;

    private string lastScene;

    public float invincibilityTime;

    [Range(0.0f, 10.0f)]
    public float maxHealth = 10;
    [Range(0.0f, 10.0f)]
    public float currentHealth;

    public float healthBarSpeed;

    public bool canTakeDamage = true;

    private bool isDying = false;

    public int deathCount;

    public int flashIters;

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

        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        //healthBar.fillAmount = (currentHealth / maxHealth);

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, healthBarSpeed);

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
        if (canTakeDamage)
        {
            currentHealth -= amount;

            StartCoroutine(InvincibilityTimer());
            StartCoroutine(InvincibilityAnimate());
        }
    }

    private IEnumerator InvincibilityTimer()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(invincibilityTime);

        canTakeDamage = true;
    }

    private IEnumerator InvincibilityAnimate()
    {
        for (int i = 0; i < flashIters; i++)
        {
            sR.color = hurtColor;

            yield return new WaitForSeconds(flashIters / (2 * invincibilityTime));

            sR.color = Color.white;

            yield return new WaitForSeconds(flashIters / (2 * invincibilityTime));
        }
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
