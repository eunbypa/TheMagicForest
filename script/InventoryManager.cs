using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    List<Item> invenItemList = new List<Item>(); // 인벤토리 각 칸에 위치한 아이템 나타냄
    List<int> invenItemQuantityList = new List<int>(); // 인벤토리 각 칸의 아이템 수량을 나타냄
    int maxSize = 28;
    bool isFull = false; // 가방이 빈 공간 없이 다 찼는지 여부
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
    public int MaxSize
    {
        get
        {
            return maxSize;
        }
    }
    public bool IsFull
    {
        get
        {
            return isFull;
        }
    }
    public List<Item> InvenItemList
    {
        get
        {
            return invenItemList;
        }
    }
    public List<int> InvenItemQuantityList
    {
        get
        {
            return invenItemQuantityList;
        }
    }
    public void ItemInsert(Item item, int num)
    {
        invenItemList.Add(item);
        invenItemQuantityList.Add(num);
        if (invenItemList.Count == maxSize) isFull = true;
    }
    public void ItemDelete(int idx)
    {
        invenItemList.RemoveAt(idx);
        invenItemQuantityList.RemoveAt(idx);
        if (isFull) isFull = false;
    }
    public void ItemQuantityIncrease(int idx, int num)
    {
        invenItemQuantityList[idx] += num;
    }
    public void ItemQuantityDecrease(int idx, int num)
    {
        if (invenItemQuantityList[idx] - num <= 0)
        {
            ItemDelete(idx);
            return;
        }
        invenItemQuantityList[idx] -= num;
    }
    public int FindItem(int itemId) // 인벤토리 내 해당 아이템을 보유하고 있으면 위치를 반환하고 보유중이 아니면 -1 반환
    {
        for(int i = 0; i < invenItemList.Count; i++)
        {
            if(invenItemList[i].ItemId == itemId)
            {
                return i;
            }
        }
        return -1;
    }
}
