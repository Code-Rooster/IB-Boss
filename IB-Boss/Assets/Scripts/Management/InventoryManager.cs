using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventory;

    public void AddItem(GameObject item, int amount)
    {
        if (inventory.Contains(item))
        {
            inventory[inventory.IndexOf(item)].GetComponent<Item>().count += amount;
        }
        else
        {
            item.GetComponent<Item>().count = amount;

            inventory.Add(item);
        }

        SortInventory();
    }

    public void SortInventory()
    {
        inventory.Sort((a, b) => a.GetComponent<Item>().itemName.CompareTo(b.GetComponent<Item>().itemName));
    }
}
