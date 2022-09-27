using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPortion : Portion
{
    int recoverHp = 10;

    public HpPortion(int itemId)
    {
        ItemId = itemId;
    }
    public override void ShowItemInfo()
    {

    }
}
