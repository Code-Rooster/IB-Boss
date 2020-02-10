using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    public LoadScene lS;

    public string sceneToLoad;
    public string sceneTransitionName;

    private void Start()
    {
        lS = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadScene>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Entered trigger");

        StartCoroutine(lS.LoadAScene(sceneToLoad));
    }
}
