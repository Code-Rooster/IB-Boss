using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneBools : MonoBehaviour
{
    private LoadScene lS;

    private void Start()
    {
        lS = transform.parent.GetComponent<LoadScene>();
    }

    public void StartLoading()
    {
        lS.loading = true;
    }

    public void EndLoading()
    {
        lS.loading = false;
    }
}
