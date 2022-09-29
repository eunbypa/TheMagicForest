using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : Potion
{
    int recoverMp = 10;

    public MpPotion(int itemId)
    {
        ItemId = itemId;
    }
    public override void ShowItemInfo()
    {

    }
}
