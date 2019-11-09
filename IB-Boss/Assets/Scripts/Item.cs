using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private InventoryManager iM;

    public enum ItemTypes { Weapon, Potion, Food, Key };
    public enum ItemEffects { Heal, Damage }

    public ItemTypes itemType;
    public ItemEffects itemEffect;

    //This is a value shared by many items. For example, if this is an HP pot that heals 5 HP, you'd set it to 5,
    //if it was a damage Pot that took 3 HP off, you'd set it to 3.
    public float effectVal;

    public string itemName;
    [TextArea (3, 5)]
    public string itemDesc;

    public int count;

    private void Start()
    {
        iM = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CollectItem(2);
        }
    }

    public void CollectItem(int amount)
    {
        iM.AddItem(this.gameObject, amount);
    }
}
