using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class Boost : MonoBehaviour
{
    public PlayerMovement pM;

    private Rigidbody2D rb;

    public Image dashBar;

    public ParticleSystem boostParticles;

    public float length;
    public float maxWaitTime;
    public float boostCamMagnitude;
    public float boostCamRoughness;
    public float boostCamFadeInTime;
    public float boostCamFadeOutTime;

    private float boostTimer;
    private float timeTilBoost;

    public bool canBoost = true;

    private void Start()
    {
        pM = this.gameObject.GetComponent<PlayerMovement>();

        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        dashBar.fillAmount = timeTilBoost / maxWaitTime;

        Mathf.Clamp(dashBar.fillAmount, 0, 1);

        if (dashBar.fillAmount == 1)
        {
            dashBar.color = new Color32(0, 236, 255, 112);
        }
        else
        {
            dashBar.color = new Color32(255, 0, 0, 112);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canBoost)
        {
            pM.isBoosting = true;
            canBoost = false;

            boostTimer = 0;

            boostParticles.Play();
            CameraShaker.Instance.ShakeOnce(boostCamMagnitude, boostCamRoughness, boostCamFadeInTime, boostCamFadeOutTime);
        }

        if (pM.isBoosting)
        {
            boostTimer += Time.fixedUnscaledDeltaTime;

            if (canBoost)
            {
                Debug.LogError("CanBoost while boosting");
            }

            if (boostTimer >= length)
            {
                pM.isBoosting = false;
                canBoost = false;
                timeTilBoost = 0;
            }
        }

        else
        {
            if (timeTilBoost < maxWaitTime)
            {
                timeTilBoost += Time.fixedDeltaTime;
            }
            else
            {
                canBoost = true;
            }
        }
    }
}
