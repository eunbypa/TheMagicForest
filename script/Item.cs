using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    [SerializeField] private int itemId;

    public int ItemId
    {
        get
        {
            return itemId;
        }
        set
        {
            itemId = value;
        }
    }
    public virtual void ShowItemInfo()
    {

    }
}
