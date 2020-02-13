using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitSpawner : MonoBehaviour
{
    public GameObject rabbit;
    private GameObject lastRabbit = null;

    private bool trackHand = true;

    private void Update()
    {
        if (trackHand)
        {
            if (lastRabbit != null)
            {
                lastRabbit.transform.position = transform.Find("HandParent").transform.Find("RightHand").transform.position;
            }
        }
    }

    public void SpawnRabbit()
    {
        lastRabbit = Instantiate(rabbit);

        trackHand = true;
    }

    public void StopTrackingHand()
    {
        trackHand = false;
    }
}
