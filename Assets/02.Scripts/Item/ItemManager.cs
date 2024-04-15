using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public UnityEvent OnItemChanged; // 아이템 변경 이벤트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        OnItemChanged.Invoke();
    }

    // 1. 아이템 추가
    public void AddItem(ItemType itemType, int count)
    {
        bool itemFound = false;
        foreach (var item in ItemList)
        {
            if (item.ItemType == itemType)
            {
                item.Count += count;
                itemFound = true;
                break; 
            }
        }
        if (itemFound)
        {
            Debug.Log("Item added and event triggered.");
            OnItemChanged?.Invoke();
        }
        else
        {
            Debug.Log("Item type not found.");
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
