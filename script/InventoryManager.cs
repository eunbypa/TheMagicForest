using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    List<Item> invenItemList = new List<Item>(); // �κ��丮 �� ĭ�� ��ġ�� ������ ��Ÿ��
    List<int> invenItemQuantityList = new List<int>(); // �κ��丮 �� ĭ�� ������ ������ ��Ÿ��
    int maxSize = 28;
    bool isFull = false; // ������ �� ���� ���� �� á���� ����
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
    public int FindItem(int itemId) // �κ��丮 �� �ش� �������� �����ϰ� ������ ��ġ�� ��ȯ�ϰ� �������� �ƴϸ� -1 ��ȯ
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
