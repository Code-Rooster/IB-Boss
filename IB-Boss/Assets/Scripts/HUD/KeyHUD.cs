using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyHUD : MonoBehaviour
{
    public int bronzeCount;
    public int silverCount;
    public int goldCount;

    public List<TMPro.TMP_Text> keyCounts = new List<TMP_Text>();

    public enum keyType { BronzeKey, SilverKey, GoldKey };

    private void Start()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("KeyHUD").Length; i++)
        {
            keyCounts.Add(GameObject.FindGameObjectsWithTag("KeyHUD")[i].GetComponentInChildren<TMPro.TMP_Text>());

            keyCounts[i].text = "";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CollectKey(keyType.BronzeKey);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CollectKey(keyType.SilverKey);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectKey(keyType.GoldKey);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            RemoveKey(keyType.BronzeKey);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveKey(keyType.SilverKey);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveKey(keyType.GoldKey);
        }
    }

    public void CollectKey(keyType kT)
    {
        switch (kT)
        {
            case (keyType.BronzeKey):
                bronzeCount++;
                break;
            case (keyType.SilverKey):
                silverCount++;
                break;
            case (keyType.GoldKey):
                goldCount++;
                break;
        }

        UpdateKeyHUD();
    }

    public void RemoveKey(keyType kT)
    {
        switch (kT)
        {
            case (keyType.BronzeKey):
                if (bronzeCount > 0)
                {
                    bronzeCount--;
                }
                break;
            case (keyType.SilverKey):
                if (silverCount > 0)
                {
                    silverCount--;
                }
                break;
            case (keyType.GoldKey):
                if (silverCount > 0)
                {
                    goldCount--;
                }
                break;
        }

        UpdateKeyHUD();
    }

    private void UpdateKeyHUD()
    {
        if (bronzeCount != 0)
        {
            keyCounts[0].text = "<sprite=0> x" + bronzeCount;
        }
        else
        {
            keyCounts[0].text = "";
        }
        if (silverCount != 0)
        {
            if (keyCounts[0].text == "")
            {
                keyCounts[0].text = "<sprite=1> x" + silverCount;
            }
            else
            {
                if (goldCount != 0 && bronzeCount == 0)
                {
                    keyCounts[0].text = "<sprite=1> x" + silverCount;
                }
                else
                {
                    keyCounts[1].text = "<sprite=1> x" + silverCount;
                }
            }
        }
        else
        {
            if (bronzeCount == 0)
            {
                keyCounts[0].text = "";
            }
            else
            {
                keyCounts[1].text = "";
            }
        }
        if (goldCount != 0)
        {
            if (keyCounts[0].text == "")
            {
                keyCounts[0].text = "<sprite=2> x" + goldCount;
            }
            else if (keyCounts[1].text == "")
            {
                keyCounts[1].text = "<sprite=2> x" + goldCount;
            }
            else
            {
                keyCounts[2].text = "<sprite=2> x" + goldCount;
            }
        }
        else
        {
            if (bronzeCount == 0 && silverCount == 0)
            {
                keyCounts[0].text = "";
            }
            else if (bronzeCount == 0 || silverCount == 0)
            {
                keyCounts[1].text = "";
            }
            else
            {
                keyCounts[2].text = "";
            }
        }
    }
}
