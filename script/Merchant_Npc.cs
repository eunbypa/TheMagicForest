using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Npc : Npc
{
    enum DialogueState // 무슨 대화를 해야하는지 그 상태 나열
    {
        enter, sellSuccess, nothingSelected, noSpaceRemained, noEnoughGoldNoSell, exit
    };
    [SerializeField] private GameObject gM;
    [SerializeField] private GameObject dM;

    GameManager gm;
    DialogueManager dm;
    DialogueData d; // npc가 현재 말해야 할 대사 데이터 저장
    int diaIdx = 0; // 다음으로 보낼 대사 데이터 위치 정보
    int priceSum = 0; // 판매 금액 총 합
    int[] sellingItemId = new int[2] { 1, 2 }; // 위치 index 기준 판매중인 아이템 id
    int[] sellingItemPrice = new int[2] { 100, 100 }; // 위치 index 기준 판매중인 아이템 가격
    string dialogue;
    string[] sellingItemType = new string[2] { "hpPortion", "mpPortion" }; // 위치 index 기준 판매중인 아이템 타입 
    bool getMoney = false; // 플레이어에게서 플레이어가 구매한 물품 가격만큼 돈을 받았는지 여부
    DialogueState ds;

    // Start is called before the first frame update
    void Start()
    {
        gm = gM.GetComponent<GameManager>();
        dm = dM.GetComponent<DialogueManager>();
        ds = DialogueState.enter;
        d = dm.DiaData[NpcId - 1];
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public override bool DialogueReady()
    {
        if(Wait) return false;
        if(diaIdx == 0)
        {
            OpenShop();
        }
        if (diaIdx < d.Dialogue.Count)
        {
            SetDiaState();
            GetDiaData();
            diaIdx = (int)ds + 1;
            gm.ChangeTalkNpc(NpcId, d.NpcName);
            gm.TextData = dialogue.ToCharArray();
            if(ds != DialogueState.exit) Wait = true;
        }
        else
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            Wait = false;
        }
        return true;
    }

    public override void SetDiaState()
    {
        if (diaIdx == 0) ds = DialogueState.enter;
        else if (!gm.ShopExit) {
            if (gm.SelectedItemLoc == -1) ds = DialogueState.nothingSelected;
            else if (gm.NoEmptySpace) ds = DialogueState.noSpaceRemained;
            else if (!getMoney) ds = DialogueState.noEnoughGoldNoSell;
            else if (getMoney) ds = DialogueState.sellSuccess;
        }
        else ds = DialogueState.exit;

    }

    public override void GetDiaData()
    {
        dialogue = d.Dialogue[(int)ds];
    }

    public void OpenShop()
    {
        gm.ShopOn();
    }

    public int GetSellingItemId(int idx)
    {
        return sellingItemId[idx];
    }

    public void SellItem(int idx, int num)
    {
        priceSum = sellingItemPrice[idx] * num;
        if (gm.CurGold < priceSum)
        {
            getMoney = false;
            return;
        }
        getMoney = true;
        gm.GoldDecrease(priceSum);
        if(sellingItemType[idx] == "hpPortion")
        {
            gm.GetItem(new HpPortion(sellingItemId[idx]), num);
        }
        if (sellingItemType[idx] == "mpPortion")
        {
            gm.GetItem(new MpPortion(sellingItemId[idx]), num);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            if (gm.npcTalk != null) gm.npcTalk = null;
            if (gm.unLockWait != null) gm.unLockWait = null;
            if (gm.sellRequest != null) gm.sellRequest = null;
            if (gm.getItemId != null) gm.getItemId = null;
            gm.npcTalk += DialogueReady;
            gm.unLockWait += UnLockWait;
            gm.sellRequest += SellItem;
            gm.getItemId += GetSellingItemId;
        }
    }
}
