using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool isTab = false;
    [SerializeField]
    private GameObject inventoryBase;
    [SerializeField]
    private GameObject SlotParent;

    public Slot[] slot;

    void Start()
    {
        slot = SlotParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTab = !isTab;
            inventoryBase.SetActive(isTab);
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(_item.itemType != Item.ItemType.weapon)
        {
            for (int i = 0; i < slot.Length; i++)
            {
                if (slot[i].item != null)
                {
                    if (slot[i].item.itemName == _item.itemName)
                    {
                        slot[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {
            if (_item != null)
            {
                if (slot[i].item == null)
                {
                    slot[i].AddItem(_item,_count);
                    return;
                }
            }
        }
    }
}
