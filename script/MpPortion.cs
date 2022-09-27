using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPortion : Portion
{
    int recoverMp = 10;

    public MpPortion(int itemId)
    {
        ItemId = itemId;
    }
    public override void ShowItemInfo()
    {

    }
}
