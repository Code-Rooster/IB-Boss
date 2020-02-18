using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntertainerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float loseSpeed;

    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, currentHealth / maxHealth, Time.deltaTime * loseSpeed);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }
}
