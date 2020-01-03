using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private KeyHUD kHUD;

    public int bronzeCount;
    public int silverCount;
    public int goldCount;

    public enum keyType { Bronze, Silver, Gold };

    private void Start()
    {
        kHUD = this.gameObject.GetComponent<KeyHUD>();
    }

    public void CollectKey(keyType kT)
    {
        switch (kT)
        {
            case (keyType.Bronze):
                bronzeCount++;
                break;
            case (keyType.Silver):
                silverCount++;
                break;
            case (keyType.Gold):
                goldCount++;
                break;
        }

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }

    public void CollectBronzeKey()
    {
        bronzeCount++;

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }

    public void CollectSilverKey()
    {
        silverCount++;

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }

    public void CollectGoldKey()
    {
        goldCount++;

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }

    public void RemoveKey(keyType kT)
    {
        switch (kT)
        {
            case (keyType.Bronze):
                if (bronzeCount > 0)
                {
                    bronzeCount--;
                }
                break;
            case (keyType.Silver):
                if (silverCount > 0)
                {
                    silverCount--;
                }
                break;
            case (keyType.Gold):
                if (goldCount > 0)
                {
                    goldCount--;
                }
                break;
        }

        kHUD.UpdateKeyHUD(bronzeCount, silverCount, goldCount);
    }
}
