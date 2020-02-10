using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }
}
