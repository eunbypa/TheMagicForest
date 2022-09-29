using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Potion
{
    int recoverHp = 10;

    public HpPotion(int itemId)
    {
        ItemId = itemId;
    }
    public override void ShowItemInfo()
    {

    }
}
