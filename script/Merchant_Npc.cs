using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Merchant_Npc
 * Description : ���� npc�� ����ϴ� Ŭ�����Դϴ�. ���� npc�� �Ϲ� npc�� �޸� �÷��̾ ���ϴ� �ൿ(������ ����, ���� ������ ��)�� ���� ��ȭ�� �մϴ�. Npc Ŭ������ ��ӹ޽��ϴ�.
 */
public class Merchant_Npc : Npc
{
    // npc�� ��ȭ ���¸� ���������� ǥ���ϱ� ���� enum�� ����߽��ϴ�.
    // enter : ���� ����, sellSuccess : �Ǹ� ����, nothingSelected : ���õ� �������� ����, noSpaceRemained : �÷��̾��� �κ��丮�� �� ��, noEnoughGoldNoSell : �÷��̾��� ��ȭ�� ���ſ��ϴ� ���ݺ��� ����, exit : ���� ����
    enum DialogueState
    {
        enter, sellSuccess, nothingSelected, noSpaceRemained, noEnoughGoldNoSell, exit
    };
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private GameObject gM; // ���� ������ GameManager
    [SerializeField] private GameObject dM; // ��ȭ ������ ������ DialogueManager

    int diaIdx = 0; // �������� ���� ��� ������ ��ġ ����
    int priceSum = 0; // �Ǹ� �ݾ� �� ��
    int[] sellingItemId = new int[2] { 1, 2 }; // ��ġ index ���� �Ǹ����� ������ id
    int[] sellingItemPrice = new int[2] { 100, 100 }; // ��ġ index ���� �Ǹ����� ������ ����
    string dialogue;
    string[] sellingItemType = new string[2] { "hpPotion", "mpPotion" }; // ��ġ index ���� �Ǹ����� ������ Ÿ�� 
    bool getMoney = false; // �÷��̾�Լ� �÷��̾ ������ ��ǰ ���ݸ�ŭ ���� �޾Ҵ��� ����

    GameManager gm; // ���� ������ GameManager Ŭ���� ��ü
    DialogueManager dm; // ��ȭ ������ ������ DialogueManager Ŭ���� ��ü
    DialogueData d; // npc�� ���� ���ؾ� �� ��� ������ ����
    DialogueState ds; // ��ȭ ����

    void Start()
    {
        gm = gM.GetComponent<GameManager>(); // gM GameObject ��ü�� �Ҵ�� GameManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        dm = dM.GetComponent<DialogueManager>(); // dM GameObject ��ü�� �Ҵ�� DialogueManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        ds = DialogueState.enter; // ��ȭ ���¸� �⺻������ �ʱ�ȭ�մϴ�.
        d = dm.DiaData[NpcId - 1]; // ���� npc�� ��� ��� ��簡 dm�� DiaData�� �ְ� �� ���徿 ������ ��Ȳ�� �°� ��ġ�Ǿ��ֽ��ϴ�. �׷��� ���� �ܰ迡 �̸� ��� �������� �ٷ� ������ �� Ŭ������ DialogueData ��ü��
                                   // �Ҵ����־����ϴ�.
    }

    /* Method : DialogueReady
     * Description : �÷��̾���� ��ȭ�� �غ��ϴ� ������ �����ϴ� �޼����Դϴ�. ���� diaIdx�� 0�̸� ���� �� ������ �鷶�ٴ� �ǹ��̹Ƿ� ���� UI�� ���� OpenShop �޼��带 ȣ���մϴ�.
     * �׸��� ���� �������� ������ ������ ���� �ൿ�� ���� SetDiaState �޼���� ���� ���� ��ȭ ���¸� �����ϰ� GetDiaData�� ���� ��縦 ������ gm�� �����ϰ� ���� �ൿ�� ���� ������ Wait�� true�� �Ͽ� ����ϵ��� 
     * �����߽��ϴ�. �׸��� diaIdx�� �غ��ص� ��� ���� �Ѿ�� ��, �� ���� ������ ���� ��� gm���� ��ȭ ���� �ǹ̷� FinishTalk�� true�� �����ϰ� diaIdx�� 0���� �ʱ�ȭ, Wait�� false�� �ʱ�ȭ�մϴ�.
     * Return Value : bool
     */
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

    /* Method : SetDiaState
     * Description : �÷��̾�� � ��縦 ���ؾ� �ϴ��� �� ��ȭ ���¸� �����ϴ� �޼����Դϴ�. �� ��Ȳ���� ds�� �˸��� DialogueState ������ ���õǵ��� �����߽��ϴ�.
     * Return Value : void
     */
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

    /* Method : GetDiaData
     * Description : ���� ��ȭ ���¸� �������� ���� ��縦 dialogue�� �Ҵ��մϴ�. 
     * Return Value : void
     */
    public override void GetDiaData()
    {
        dialogue = d.Dialogue[(int)ds];
    }

    /* Method : OpenShop
     * Description : ������ ���� ������ �����ϴ� �޼����Դϴ�. 
     * Return Value : void
     */
    public void OpenShop()
    {
        gm.ShopOn();
    }

    /* Method : GetSellingItemId
     * Description : �Ǹ����� ������ ����� ��ġ�� �������� �ش� �������� ���̵� ��ȯ�ϴ� �޼����Դϴ�.
     * Parameter : int idx - ��ġ
     * Return Value : void
     */
    public int GetSellingItemId(int idx)
    {
        return sellingItemId[idx];
    }

    /* Method : SellItem
     * Description : ������ �Ǹ� ������ �����ϴ� �޼����Դϴ�. gm���Լ� �÷��̾ ���� �������� ����� ���� �ľ��ϰ� �ش� ���� �÷��̾ �����ϰ��� �ϴ� ������ ������ �պ��� ������ getMoney�� false�� �����ϰ�
     * �޼��带 �������ɴϴ�. �� ��찡 �ƴϸ� �ǸŰ� ���������� ����Ǿ� gm�� �ش� ���ݸ�ŭ �÷��̾��� ��带 �����ϴ� �Լ��� ȣ���ϰ� ��� ��ġ�� ���������� �ľ� �� �ش� ������ ��ü�� �����ؼ� ������ �Բ� gm��
     * GetItem�Լ��� �Ű������� �����ϵ��� �����߽��ϴ�.
     * Return Value : void
     */
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
        if(sellingItemType[idx] == "hpPotion")
        {
            gm.GetItem(new HpPotion(sellingItemId[idx]), num);
        }
        if (sellingItemType[idx] == "mpPotion")
        {
            gm.GetItem(new MpPotion(sellingItemId[idx]), num);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // �÷��̾ npc�� ������ ���
        {
            if (gm.npcTalk != null) gm.npcTalk = null; // delegate �ʱ�ȭ
            if (gm.unLockWait != null) gm.unLockWait = null; // delegate �ʱ�ȭ
            if (gm.sellRequest != null) gm.sellRequest = null; // delegate �ʱ�ȭ
            if (gm.getItemId != null) gm.getItemId = null; // delegate �ʱ�ȭ
            gm.npcTalk += DialogueReady; // delegate�� �޼��� �Ҵ�
            gm.unLockWait += UnLockWait; // delegate�� �޼��� �Ҵ�
            gm.sellRequest += SellItem; // delegate�� �޼��� �Ҵ�
            gm.getItemId += GetSellingItemId; // delegate�� �޼��� �Ҵ�
        }
    }
}
