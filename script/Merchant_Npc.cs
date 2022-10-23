using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Merchant_Npc
 * Description : 상인 npc를 담당하는 클래스입니다. 상인 npc는 일반 npc와 달리 플레이어가 행하는 행동(아이템 구매, 상점 나가기 등)에 맞춰 대화를 합니다. Npc 클래스를 상속받습니다.
 */
public class Merchant_Npc : Npc
{
    // npc의 대화 상태를 직관적으로 표현하기 위해 enum을 사용했습니다.
    // enter : 상점 입장, sellSuccess : 판매 성공, nothingSelected : 선택된 아이템이 없음, noSpaceRemained : 플레이어의 인벤토리가 꽉 참, noEnoughGoldNoSell : 플레이어의 재화가 구매원하는 가격보다 부족, exit : 상점 퇴장
    enum DialogueState
    {
        enter, sellSuccess, nothingSelected, noSpaceRemained, noEnoughGoldNoSell, exit
    };

    int diaIdx = 0; // 다음으로 보낼 대사 데이터 위치 정보
    int priceSum = 0; // 판매 금액 총 합
    int[] sellingItemId = new int[2] { 1, 2 }; // 위치 index 기준 판매중인 아이템 id
    int[] sellingItemPrice = new int[2] { 100, 100 }; // 위치 index 기준 판매중인 아이템 가격
    string dialogue;
    string[] sellingItemType = new string[2] { "hpPotion", "mpPotion" }; // 위치 index 기준 판매중인 아이템 타입 
    bool getMoney = false; // 플레이어에게서 플레이어가 구매한 물품 가격만큼 돈을 받았는지 여부

    DialogueData d; // npc가 현재 말해야 할 대사 데이터 저장
    DialogueState ds; // 대화 상태

    void Start()
    {
        ds = DialogueState.enter; // 대화 상태를 기본값으로 초기화합니다.
        d = DialogueManager.instance.DiaData[NpcId - 1]; // 상인 npc의 경우 모든 대사가 dm의 DiaData에 있고 한 문장씩 각각의 상황에 맞게 배치되어있습니다. 그래서 시작 단계에 미리 대사 데이터을 바로 가져와 이 클래스의 DialogueData 객체에 할당해주었습니다.
    }

    /* Method : DialogueReady
     * Description : 플레이어와의 대화를 준비하는 동작을 수행하는 메서드입니다. 현재 diaIdx가 0이면 고객이 막 상점에 들렀다는 의미이므로 상점 UI를 여는 OpenShop 메서드를 호출합니다.
     * 그리고 고객이 상점에서 나가기 전까지 고객의 행동에 따라 SetDiaState 메서드로 현재 말할 대화 상태를 설정하고 GetDiaData로 말할 대사를 가져와 gm에 전달하고 다음 행동이 있을 때까지 Wait를 true로 하여 대기하도록 
     * 구현했습니다. 그리고 diaIdx가 준비해둔 대사 수를 넘어갔을 때, 즉 고객이 상점을 나간 경우 gm에게 대화 종료 의미로 FinishTalk을 true로 설정하고 diaIdx를 0으로 초기화, Wait를 false로 초기화합니다.
     * Return Value : bool
     */
    public override bool DialogueReady()
    {
        if (Wait) return false;
        if (diaIdx == 0)
        {
            OpenShop();
        }
        if (diaIdx < d.Dialogue.Count)
        {
            SetDiaState();
            GetDiaData();
            diaIdx = (int)ds + 1;
            GameManager.instance.ChangeTalkNpc(NpcId, d.NpcName);
            GameManager.instance.TextData = dialogue.ToCharArray();
            if (ds != DialogueState.exit) Wait = true;
        }
        else
        {
            GameManager.instance.FinishTalk = true;
            diaIdx = 0;
            Wait = false;
        }
        return true;
    }

    /* Method : SetDiaState
     * Description : 플레이어에게 어떤 대사를 말해야 하는지 그 대화 상태를 설정하는 메서드입니다. 각 상황에서 ds에 알맞은 DialogueState 변수가 세팅되도록 구현했습니다.
     * Return Value : void
     */
    public override void SetDiaState()
    {
        if (diaIdx == 0) ds = DialogueState.enter;
        else if (!GameManager.instance.ShopExit)
        {
            if (GameManager.instance.SelectedItemLoc == -1) ds = DialogueState.nothingSelected;
            else if (GameManager.instance.NoEmptySpace) ds = DialogueState.noSpaceRemained;
            else if (!getMoney) ds = DialogueState.noEnoughGoldNoSell;
            else if (getMoney) ds = DialogueState.sellSuccess;
        }
        else ds = DialogueState.exit;

    }

    /* Method : GetDiaData
     * Description : 현재 대화 상태를 기준으로 말할 대사를 dialogue에 할당합니다. 
     * Return Value : void
     */
    public override void GetDiaData()
    {
        dialogue = d.Dialogue[(int)ds];
    }

    /* Method : OpenShop
     * Description : 상점을 여는 동작을 수행하는 메서드입니다. 
     * Return Value : void
     */
    public void OpenShop()
    {
        GameManager.instance.ShopOn();
    }

    /* Method : GetSellingItemId
     * Description : 판매중인 아이템 목록의 위치를 기준으로 해당 아이템의 아이디를 반환하는 메서드입니다.
     * Parameter : int idx - 위치
     * Return Value : void
     */
    public int GetSellingItemId(int idx)
    {
        return sellingItemId[idx];
    }

    /* Method : SellItem
     * Description : 아이템 판매 동작을 수행하는 메서드입니다. gm에게서 플레이어가 현재 보유중인 골드의 값을 파악하고 해당 값이 플레이어가 구매하고자 하는 아이템 가격의 합보다 작으면 getMoney를 false로 설정하고
     * 메서드를 빠져나옵니다. 이 경우가 아니면 판매가 성공적으로 진행되어 gm에 해당 가격만큼 플레이어의 골드를 차감하는 함수를 호출하고 어느 위치의 아이템인지 파악 후 해당 아이템 객체를 생성해서 수량과 함께 gm의
     * GetItem함수에 매개변수로 전달하도록 구현했습니다.
     * Return Value : void
     */
    public void SellItem(int idx, int num)
    {
        priceSum = sellingItemPrice[idx] * num;
        if (GameManager.instance.CurGold < priceSum)
        {
            getMoney = false;
            return;
        }
        getMoney = true;
        GameManager.instance.GoldDecrease(priceSum);
        if (sellingItemType[idx] == "hpPotion")
        {
            GameManager.instance.GetItem(new HpPotion(sellingItemId[idx]), num);
        }
        if (sellingItemType[idx] == "mpPotion")
        {
            GameManager.instance.GetItem(new MpPotion(sellingItemId[idx]), num);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // 플레이어가 npc와 접촉한 경우
        {
            if (GameManager.instance.npcTalk != null) GameManager.instance.npcTalk = null; // delegate 초기화
            if (GameManager.instance.unLockWait != null) GameManager.instance.unLockWait = null; // delegate 초기화
            if (GameManager.instance.sellRequest != null) GameManager.instance.sellRequest = null; // delegate 초기화
            if (GameManager.instance.getItemId != null) GameManager.instance.getItemId = null; // delegate 초기화
            GameManager.instance.npcTalk += DialogueReady; // delegate에 메서드 할당
            GameManager.instance.unLockWait += UnLockWait; // delegate에 메서드 할당
            GameManager.instance.sellRequest += SellItem; // delegate에 메서드 할당
            GameManager.instance.getItemId += GetSellingItemId; // delegate에 메서드 할당
        }
    }
}
