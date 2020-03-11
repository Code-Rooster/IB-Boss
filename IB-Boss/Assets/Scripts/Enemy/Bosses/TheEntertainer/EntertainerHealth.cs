using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntertainerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float iTime;

    public float loseSpeed;

    public bool canBeHit = true;

    public Slider healthBar;

    public TMPro.TMP_Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.value = Mathf.Lerp(healthBar.value, currentHealth / maxHealth, Time.deltaTime * loseSpeed);

        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void TakeDamage(float amount)
    {
        if (canBeHit)
        {
            currentHealth -= amount;

            StartCoroutine(InvincibilityTimer());
        }
    }

    private IEnumerator InvincibilityTimer()
    {
        canBeHit = false;

        yield return new WaitForSeconds(iTime);

        canBeHit = true;
    }
}
