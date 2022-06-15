using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1: MonoBehaviour
{
    public enum ItemType { Food, ingredient, weapon}
    public ItemType itemType;
    public int value;
    public Sprite itemImage;
    public string itemName;

}
