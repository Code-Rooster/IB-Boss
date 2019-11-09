using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    private InventoryManager iM;

    private GameObject inventoryContent;

    public bool isOpen;

    public GameObject countText;

    Animator anim;

    private void Start()
    {
        iM = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        inventoryContent = GameObject.Find("InventoryContent");

        anim = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        foreach (Transform item in inventoryContent.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < iM.inventory.Count; i++)
        {
            //iM.inventory[i].transform.parent = inventoryContent.transform;
            Instantiate(iM.inventory[i], inventoryContent.transform);
        }

        for (int j = 0; j < inventoryContent.transform.childCount; j++)
        {
            Instantiate(countText, inventoryContent.transform.GetChild(j));

            inventoryContent.transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = iM.inventory[j].GetComponent<Item>().count.ToString();
        }

        isOpen = true;
        anim.Play("OpenInventory");
    }

    public void Close() {
        isOpen = false;
        anim.Play("CloseInventory");
    }
}
