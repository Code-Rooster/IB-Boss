using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private CameraShake camShake;

    public float maxHealth;
    public float currentHealth;
    private float lastHealth;
    public float defense = 1f;

    public float killMagnitude;
    public int killIterations;
    public float killTimePerShakeCycle;
    public float killRoughness;

    public float hurtMagnitude;
    public int hurtIterations;
    public float hurtTimePerShakeCycle;
    public float hurtRoughness;

    public Rigidbody2D rb;

    Animator anim;

    public ParticleSystem deathParticles;

    public Image healthBar;

    void Start()
    {
        camShake = Camera.main.GetComponent<CameraShake>();

        currentHealth = maxHealth;
        lastHealth = maxHealth;

        healthBar = transform.Find("HealthCanvas").Find("HealthBar").Find("HealthFill").GetComponent<Image>();

        rb = gameObject.GetComponent<Rigidbody2D>();

        anim = transform.Find(gameObject.name + "Renderer").GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= (amount / defense);
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth < lastHealth)
        {
            //camShake.StopAllCoroutines();
            camShake.StartCoroutine(camShake.ShakeCam(hurtMagnitude, hurtIterations, hurtTimePerShakeCycle, hurtMagnitude));

            anim.Play(gameObject.name + "Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        lastHealth = currentHealth;
    }

    public void Knockback(Vector2 direction)
    {
        rb.velocity = direction;
    }

    public void Die()
    {
        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        transform.Find("HealthCanvas").gameObject.SetActive(false);

        //camShake.StopAllCoroutines();
        camShake.StartCoroutine(camShake.ShakeCam(killMagnitude, killIterations, killTimePerShakeCycle, killRoughness));

        anim.Play(gameObject.name + "Death");
        Destroy(gameObject, 0.5f);
    }
}
