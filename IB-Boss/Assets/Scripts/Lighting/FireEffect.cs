using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public float maxReduction;
    public float maxIncrease;
    public float rateDamping;
    public float strength;
    private float baseIntesity;

    private Light source;
    
    private bool flickering;

    private void Start()
    {
        source = GetComponent<Light>();
        if (source == null)
        {
            Debug.LogError("Not attatched to a light source, bro.");
            return;
        }

        baseIntesity = source.intensity;

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true) {
            source.intensity = Mathf.Lerp(source.intensity, Random.Range(baseIntesity - maxReduction, baseIntesity + maxIncrease), strength * Time.deltaTime);

            yield return new WaitForSeconds(rateDamping);
        }
    }
}
