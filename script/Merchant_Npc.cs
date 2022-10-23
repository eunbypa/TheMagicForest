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

    int diaIdx = 0; // �������� ���� ��� ������ ��ġ ����
    int priceSum = 0; // �Ǹ� �ݾ� �� ��
    int[] sellingItemId = new int[2] { 1, 2 }; // ��ġ index ���� �Ǹ����� ������ id
    int[] sellingItemPrice = new int[2] { 100, 100 }; // ��ġ index ���� �Ǹ����� ������ ����
    string dialogue;
    string[] sellingItemType = new string[2] { "hpPotion", "mpPotion" }; // ��ġ index ���� �Ǹ����� ������ Ÿ�� 
    bool getMoney = false; // �÷��̾�Լ� �÷��̾ ������ ��ǰ ���ݸ�ŭ ���� �޾Ҵ��� ����

    DialogueData d; // npc�� ���� ���ؾ� �� ��� ������ ����
    DialogueState ds; // ��ȭ ����

    void Start()
    {
        ds = DialogueState.enter; // ��ȭ ���¸� �⺻������ �ʱ�ȭ�մϴ�.
        d = DialogueManager.instance.DiaData[NpcId - 1]; // ���� npc�� ��� ��� ��簡 dm�� DiaData�� �ְ� �� ���徿 ������ ��Ȳ�� �°� ��ġ�Ǿ��ֽ��ϴ�. �׷��� ���� �ܰ迡 �̸� ��� �������� �ٷ� ������ �� Ŭ������ DialogueData ��ü�� �Ҵ����־����ϴ�.
    }

    /* Method : DialogueReady
     * Description : �÷��̾���� ��ȭ�� �غ��ϴ� ������ �����ϴ� �޼����Դϴ�. ���� diaIdx�� 0�̸� ���� �� ������ �鷶�ٴ� �ǹ��̹Ƿ� ���� UI�� ���� OpenShop �޼��带 ȣ���մϴ�.
     * �׸��� ���� �������� ������ ������ ���� �ൿ�� ���� SetDiaState �޼���� ���� ���� ��ȭ ���¸� �����ϰ� GetDiaData�� ���� ��縦 ������ gm�� �����ϰ� ���� �ൿ�� ���� ������ Wait�� true�� �Ͽ� ����ϵ��� 
     * �����߽��ϴ�. �׸��� diaIdx�� �غ��ص� ��� ���� �Ѿ�� ��, �� ���� ������ ���� ��� gm���� ��ȭ ���� �ǹ̷� FinishTalk�� true�� �����ϰ� diaIdx�� 0���� �ʱ�ȭ, Wait�� false�� �ʱ�ȭ�մϴ�.
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
     * Description : �÷��̾�� � ��縦 ���ؾ� �ϴ��� �� ��ȭ ���¸� �����ϴ� �޼����Դϴ�. �� ��Ȳ���� ds�� �˸��� DialogueState ������ ���õǵ��� �����߽��ϴ�.
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
        GameManager.instance.ShopOn();
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
        if (Other.gameObject.tag == "Player") // �÷��̾ npc�� ������ ���
        {
            if (GameManager.instance.npcTalk != null) GameManager.instance.npcTalk = null; // delegate �ʱ�ȭ
            if (GameManager.instance.unLockWait != null) GameManager.instance.unLockWait = null; // delegate �ʱ�ȭ
            if (GameManager.instance.sellRequest != null) GameManager.instance.sellRequest = null; // delegate �ʱ�ȭ
            if (GameManager.instance.getItemId != null) GameManager.instance.getItemId = null; // delegate �ʱ�ȭ
            GameManager.instance.npcTalk += DialogueReady; // delegate�� �޼��� �Ҵ�
            GameManager.instance.unLockWait += UnLockWait; // delegate�� �޼��� �Ҵ�
            GameManager.instance.sellRequest += SellItem; // delegate�� �޼��� �Ҵ�
            GameManager.instance.getItemId += GetSellingItemId; // delegate�� �޼��� �Ҵ�
        }
    }
}
