using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    //REMEMBER: ONLY HAVE ONE ANIMATION ALLOWED TO PLAY ON "ENTRY"

    public float waitTime = 2;

    public float maxTime = 2;
    public float modifier = 0.5f;

    public GameObject player;

    private Animator sceneTransition;

    public Animator screenWipe;

    public GameObject loadingScreen;

    public Slider slider;

    public bool loading = true;

    private void Start()
    {
        screenWipe = transform.Find("ScreenWipe").GetComponent<Animator>();

        player = GameObject.Find("Player");

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("SceneTransition").Length; i++)
        {
            sceneTransition = GameObject.FindGameObjectsWithTag("SceneTransition")[i].GetComponent<Animator>();
        }
    }

    public IEnumerator LoadAScene(string sceneName)
    {
        if (!loading)
        {
            sceneTransition.SetTrigger("StartLoading");
        }

        print("Played start animation.");

        while (!loading)
        {
            yield return null;
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01((loadOperation.progress) / 0.9f);

            slider.value = progress;

            if (progress >= 1)
            {
                GameObject playerPoint = GameObject.Find("PlayerPoint");

                if (playerPoint != null)
                {
                    player.transform.position = playerPoint.transform.position;
                }
                else
                {
                    player.transform.position = Vector3.zero;
                }

                sceneTransition.SetTrigger("EndLoading");

                print("Played end animation.");

                Time.timeScale = 1;
            }

            yield return null;
        }
    }
}
