using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public GameObject inventory;
    public bool isTab = false;

    public Slot[] slots;
    public Transform slotHolder;
    Inventory inven;
    void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.SetActive(isTab);
        inven.onSlotItem += RedrawSlotUI;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isTab = !isTab;
            inventory.SetActive(isTab);
        }
    }

    void RedrawSlotUI()
    {
        for(int i = 0; i < inven.item.Count; i++)
        {
            slots[i].item = inven.item[i];
            slots[i].UpdateSlotUI();
        }
    }
}
