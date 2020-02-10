using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float frequency = 10;
    public float seed;
    public float recoverySpeed = 1.5f;
    public float trauma = 1;
    public float traumaExp;
    public float intensity = 1;

    public bool startShake;

    private void Start()
    {
        seed = Random.value;
    }

    private void Update()
    {
        if (startShake)
        {
            StartShake(1, intensity);
            startShake = false;
        }

        float shake = Mathf.Pow(trauma, traumaExp) * intensity;

        transform.localPosition = new Vector3(
            Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1,
            Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1,
            0) * shake;

        transform.localRotation = Quaternion.Euler(new Vector3(
            0, 0,
            Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
            * shake);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }

    public void StartShake(float freq, float shakeIntensity)
    {
        seed = Random.value;

        trauma = 1;

        frequency = freq;

        intensity = shakeIntensity;
    }
}
