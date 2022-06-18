using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private GameObject goCountImage;

    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemIcon.sprite = item.itemImage;
        goCountImage.SetActive(true);
        countText.text = itemCount.ToString(); 
    }
    private void SetColor(float _alpha)
    {
        Color color = itemIcon.color;
        color.a = _alpha;
        itemIcon.color = color;
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        if (item.itemType != Item.ItemType.weapon)
        {
            goCountImage.SetActive(true);
            itemCount++;
            countText.text = itemCount.ToString();
        }
        else
        {
            goCountImage.SetActive(false);
            countText.text = "0";
        }
        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        countText.text = itemCount.ToString();
        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemIcon.sprite = null;
        SetColor(0);
        goCountImage.SetActive(false);
        countText.text = "0";
    }
}
