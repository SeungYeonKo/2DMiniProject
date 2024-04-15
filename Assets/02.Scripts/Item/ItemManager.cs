using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public UnityEvent OnDataChanged;
    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public List<Item> ItemList = new List<Item>();
    private void Start()
    {
        ItemList.Add(new Item(ItemType.Attack, 0));
        ItemList.Add(new Item(ItemType.Key, 0));

        OnDataChanged.Invoke();
    }

    // 1. 아이템 추가
    public void AddItem(ItemType itemType, int count)
    {
        foreach (var item in ItemList)
        {
            if (item.ItemType == itemType)
            {
                item.Count += count;
                OnDataChanged?.Invoke();
                return; 
            }
        }
    }
    // 아이템 개수 조회
    public int GetItemCount(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].Count;
            }
        }
        return 0;
    }
/*
    public bool TryUseItem(ItemType itemType)
    {

    }*/
}
