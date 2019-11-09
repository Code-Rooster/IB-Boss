using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyHUD : MonoBehaviour
{
    public List<TMPro.TMP_Text> keyCounts = new List<TMP_Text>();

    private KeyManager kM;

    private void Start()
    {
        kM = this.gameObject.GetComponent<KeyManager>();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("KeyHUD").Length; i++)
        {
            keyCounts.Add(GameObject.FindGameObjectsWithTag("KeyHUD")[i].GetComponentInChildren<TMPro.TMP_Text>());

            keyCounts[i].text = "";
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    kM.CollectKey(KeyManager.keyType.BronzeKey);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    kM.CollectKey(KeyManager.keyType.SilverKey);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    kM.CollectKey(KeyManager.keyType.GoldKey);
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    kM.RemoveKey(KeyManager.keyType.BronzeKey);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    kM.RemoveKey(KeyManager.keyType.SilverKey);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    kM.RemoveKey(KeyManager.keyType.GoldKey);
        //}
    }

    public void UpdateKeyHUD(int bC, int sC, int gC)
    {
        if (bC != 0)
        {
            keyCounts[0].text = "<sprite=0> x" + bC;
        }
        else
        {
            keyCounts[0].text = "";
        }

        if (sC != 0)
        {
            if (bC == 0)
            {
                keyCounts[0].text = "<sprite=1> x" + sC;
            }
            else
            {
                keyCounts[1].text = "<sprite=1> x" + sC;
            }
        }
        else
        {
            if (bC == 0)
            {
                keyCounts[0].text = "";
            }
            else
            {
                keyCounts[1].text = "";
            }
        }
        if (gC != 0)
        {
            if (bC != 0 && sC != 0)
            {
                keyCounts[2].text = "<sprite=2> x" + gC;
            }
            else if (bC != 0 || sC != 0)
            {
                keyCounts[1].text = "<sprite=2> x" + gC;
                keyCounts[2].text = "";
            }
            else
            {
                keyCounts[0].text = "<sprite=2> x" + gC;
            }
        }
        else
        {
            if (bC == 0 && sC == 0)
            {
                keyCounts[0].text = "";
            }
            else if (bC == 0 || sC == 0)
            {
                keyCounts[1].text = "";
                keyCounts[1].text = "";
            }
            else
            {
                keyCounts[2].text = "";
            }
        }
    }
}
