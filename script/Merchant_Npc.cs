using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Npc : Npc
{
    enum DialogueState // 무슨 대화를 해야하는지 그 상태 나열
    {
        normal, nothingSelected, noEnoughGoldNoSell, sellSuccess
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DialogueReady()
    {

    }

    public override void SetDiaState()
    {

    }

    public override void GetDiaData()
    {
 
    }
}
