using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item1 item;
    public SpriteRenderer image;

    public void SetItem(Item1 _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;

        image.sprite = item.itemImage;
    }
}
