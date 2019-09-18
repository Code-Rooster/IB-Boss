using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private KeyHUD kHUD;

    public int bronzeCount;
    public int silverCount;
    public int goldCount;

    public enum keyType { BronzeKey, SilverKey, GoldKey };

    private void Start()
    {
        kHUD = this.gameObject.GetComponent<KeyHUD>();
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

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
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
                if (goldCount > 0)
                {
                    goldCount--;
                }
                break;
        }

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }
}
