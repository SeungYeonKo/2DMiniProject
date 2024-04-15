using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,
    Attack,
    Key
}

public class Item:MonoBehaviour
{
    public ItemType ItemType;
    public int Count;

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }
}
