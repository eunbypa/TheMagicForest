using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public delegate bool GetTalkData(); // npc���׼� ��縦 ���޹޴� �븮��
    public delegate void UnLockWaiting(); // ��ٸ��� �ִ� ��Ȳ�� ��ٸ��� �ʾƵ� �ǵ��� Ǯ���ִ� �븮��
    public delegate void SetMagic(GameObject sk); // ������ ���� �� �÷��̾� ���������� �̹��� �ٲٱ� & ��ų ����
    public delegate void SellRequestToMerchant(int idx, int num); // ������ �������� �Ǹ��ϵ��� SellItem �Լ� ȣ���ϴ� �븮��
    public delegate int GetItemId(int idx); // ������ �������� ��ġ�� ������� �ش� �������� ���̵� �������� ���ο��Լ� ������ �޴� �븮��
    public GetTalkData npcTalk;
    public UnLockWaiting unLockWait;
    public SetMagic setMagic;
    public SellRequestToMerchant sellRequest;
    public GetItemId getItemId;
    //serializefield �ؾߵ�
    //[SerializeField] private GameObject dM;
    [SerializeField] private GameObject qM;
    [SerializeField] private GameObject[] skills;
    [SerializeField] private GameObject yes;
    [SerializeField] private GameObject no;
    [SerializeField] private GameObject skillLocked;
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject progressingQuest;
    [SerializeField] private GameObject talkUI;
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject skillImage;
    [SerializeField] private GameObject inven;
    [SerializeField] private GameObject[] invenItemSpaces;
    [SerializeField] private GameObject[] invenItemQuantitySpaces;
    [SerializeField] private GameObject[] reqList;
    [SerializeField] private GameObject selectMagicStoneUI;
    [SerializeField] private GameObject shopBack;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject enterNumberUI;
    [SerializeField] private GameObject blackOut;
    [SerializeField] private TMP_InputField itemQuantityInput;
    [SerializeField] private TMPro.TMP_Text gold;
    [SerializeField] private Image hpGraph;
    [SerializeField] private TMPro.TMP_Text maxHp;
    [SerializeField] private TMPro.TMP_Text hp;
    [SerializeField] private Image mpGraph;
    [SerializeField] private TMPro.TMP_Text maxMp;
    [SerializeField] private TMPro.TMP_Text mp;
    [SerializeField] private Image expGraph;
    [SerializeField] private TMPro.TMP_Text maxExp;
    [SerializeField] private TMPro.TMP_Text exp;
    [SerializeField] private Image skill;
    [SerializeField] private TMPro.TMP_Text coolTime;
    [SerializeField] private Image npcImage;
    [SerializeField] private TMPro.TMP_Text npcName;
    [SerializeField] private TMPro.TMP_Text talk;
    [SerializeField] private TMPro.TMP_Text questT;
    [SerializeField] private TMPro.TMP_Text questTitle;
    [SerializeField] private TMPro.TMP_Text questNpc;
    [SerializeField] private TMPro.TMP_Text questNpcName;
    [SerializeField] private TMPro.TMP_Text questInfo;
    [SerializeField] private TMPro.TMP_Text[] questReqName;
    [SerializeField] private Image[] shopSelectedItem;
    [SerializeField] private Image[] invenItemList;
    [SerializeField] private TMPro.TMP_Text[] invenItemQuantityList;
    //[SerializeField] private TMPro.TMP_Text[] questReqCurNum;
    //[SerializeField] private TMPro.TMP_Text[] slash;
    //[SerializeField] private TMPro.TMP_Text[] questReqTotal;

    [SerializeField] private Sprite[] npcImages;
    [SerializeField] private Sprite[] skImages;
    [SerializeField] private Sprite[] itemImages;

    //public bool Istalking = false;
    //public bool FinishTalk = false;
    //public bool skDisable = false;
    // public bool accept = false;
    //public int curMapNum = 0;

    private IEnumerator talkAni;
    private IEnumerator waitForTeleport;
    private WaitForSeconds wfs;
    private WaitForSeconds wfs2;

    InventoryManager im;
    //DialogueManager dm;
    QuestManager qm;
    Animator ani;
    List<int> curQuestReqNum = new List<int>();
    int curMapNum = 0;
    int curNpcId = 0;
    //int i = 0;
    int curGold = 0;
    int curHp = 0;
    int curMaxHp = 0;
    int curMp = 0;
    int curMaxMp = 0;
    int curExp = 0;
    int curMaxExp = 0;
    int curQuestNum = 1;
    int curQuestNpcId; //���� ���� ������ Ȥ�� �������� ����Ʈ�� ������ �ִ� NPC�� ID
    int finishReqNum = 0; // ����Ʈ ���ǵ鿡�� �Ϸ��� ���� ��
    int selectedItemLoc = -1; // ���õ� �������� ĭ ��ġ(�κ��丮 or ����), -1 : ���õ� �������� ������ �ǹ�
    float percent = 0;
    string tmp = null;
    char[] textData = null;
    bool isTalking = false;
    bool teleportReady = false;
    bool success = false;
    bool finishTalk = false;
    bool skDisable = false;
    bool accept = false;
    //bool[] selectedMagicStone = new bool[3];
    bool waterSelected = false;
    bool dirtSelected = false;
    bool windSelected = false;
    bool noEmptySpace = false;
    bool shopExit = false;
    //bool doneQuest = false; // ��� ����Ʈ �Ϸ��� ���
    //bool startTalk = true;

    void Start()
    {
        //this.ani = questState.GetComponent<Animator>();
        this.ani = blackOut.GetComponent<Animator>();
        this.talkAni = TalkAnime();
        this.waitForTeleport = WaitForTeleport();
        this.wfs = new WaitForSeconds(0.05f);
        this.wfs2 = new WaitForSeconds(0.5f);
        //this.dm = dM.GetComponent<DialogueManager>();
        this.im = inven.GetComponent<InventoryManager>();
        this.qm = qM.GetComponent<QuestManager>();
        this.curGold = Convert.ToInt32(gold.text);
        this.curHp = Convert.ToInt32(hp.text);
        this.curMaxHp = Convert.ToInt32(maxHp.text);
        this.curMp = Convert.ToInt32(mp.text);
        this.curMaxMp = Convert.ToInt32(maxMp.text);
        this.curMaxExp = Convert.ToInt32(maxExp.text);
        this.curQuestNpcId = qm.QuestDataList[curQuestNum - 1].NpcId;
    }
    public int CurGold
    {
        get
        {
            return curGold;
        }
    }
    public int CurMp
    {
        get
        {
            return curMp;
        }
    }
    public int CurMapNum
    {
        get
        {
            return curMapNum;
        }
    }

    public int CurQuestNum
    {
        get
        {
            return curQuestNum;
        }
    }

    public int CurQuestNpcId
    {
        get
        {
            return curQuestNpcId;
        }
    }

    public int SelectedItemLoc
    {
        get
        {
            return selectedItemLoc;
        }
    }

    public bool Accept
    {
        get
        {
            return accept;
        }
    }

    public bool Success
    {
        get
        {
            return success;
        }
    }

    public bool SkDisable
    {
        get
        {
            return skDisable;
        }
    }

    public char[] TextData
    {
        set
        {
            textData = value;
        }
    }
    public bool FinishTalk
    {
        set
        {
            finishTalk = value;
        }
    }

    public bool WaterSelected
    {
        set
        {
            waterSelected = value;
        }
    }
    public bool DirtSelected
    {
        set
        {
            dirtSelected = value;
        }
    }
    public bool WindSelected
    {
        set
        {
            windSelected = value;
        }
    }
    public bool TeleportReady
    {
        get
        {
            return teleportReady;
        }
    }
    public bool ShopExit
    {
        get
        {
            return shopExit;
        }
    }
    public bool NoEmptySpace
    {
        get
        {
            return noEmptySpace;
        }
    }
    public InventoryManager InvenManager
    {
        get
        {
            return im;
        }
    }


    /*public void GetMessage(string s) // �޽����� �޾� �� ������ �����ϴ� ���? �����
    {
        if(s == "����Ʈ����")
        {

        }
    }*/
    public void WaitForTeleportReady()
    {
        blackOut.SetActive(true);
        ani.SetTrigger("blackOut");
        waitForTeleport = WaitForTeleport();
        StartCoroutine(waitForTeleport);
    }

    public void TeleportMap(int n)
    {
        curMapNum = n;
        teleportReady = false;
    }
    IEnumerator WaitForTeleport()
    {
        yield return wfs2;
        teleportReady = true;
        yield return wfs2;
        blackOut.SetActive(false);
        yield break;
    }
    public void SetSkillDisable()
    {
        skDisable = true;
        skillLocked.SetActive(true);
    }
    public void ResetSkillDisable()
    {
        skDisable = false;
        skillLocked.SetActive(false);
    }
    public void CheckCoolTime(float num)
    {
        coolTime.text = Convert.ToString(num);
        if(num == 0)
        {
            coolTime.text = null;
        }
    }
    public void GoldIncrease(int n)
    {
        curGold += n;
        gold.text = Convert.ToString(curGold);
    }

    public void GoldDecrease(int n)
    {
        curGold -= n;
        gold.text = Convert.ToString(curGold);
    }

    public void HpUp(int n) // �ִ� �ʰ� ���� �˻����� ���߿� �߰��ؾ���
    {
        curHp += n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxHp)));
        hpGraph.fillAmount += percent;
        hp.text = Convert.ToString(curHp);
    }

    public void HPDown(int n) // 0 ���� ���� �˻����� ���߿� �߰��ؾ���
    {
        curHp -= n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxHp)));
        hpGraph.fillAmount -= percent;
        hp.text = Convert.ToString(curHp);
    }

    public void MPUp(int n) // �ִ� �ʰ� ���� �˻����� ���߿� �߰��ؾ���
    {
        curMp += n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxMp)));
        mpGraph.fillAmount += percent;
        mp.text = Convert.ToString(curMp);
    }

    public void MPDown(int n)
    {
        curMp -= n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxMp)));
        mpGraph.fillAmount -= percent;
        mp.text = Convert.ToString(curMp);
    }
    
    public void ExpUp(int n)
    {
        curExp += n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxExp)));
        //percent = Convert.ToDouble(n) / Convert.ToDouble(maxexp);
        expGraph.fillAmount += percent;
        exp.text = Convert.ToString(curExp);
    }

    public void InventoryOn()
    {
        for(int i = 0; i < im.MaxSize; i++)
        {
            if(i < im.InvenItemList.Count)
            {
                invenItemList[i].sprite = itemImages[im.InvenItemList[i].ItemId - 1];
                invenItemQuantityList[i].text = Convert.ToString(im.InvenItemQuantityList[i]);
                invenItemSpaces[i].SetActive(true);
                invenItemQuantitySpaces[i].SetActive(true);
            }
            else
            {
                invenItemSpaces[i].SetActive(false);
                invenItemQuantitySpaces[i].SetActive(false);
            }
        }
        inven.SetActive(true);
    }
    public void InventoryOff()
    {
        inven.SetActive(false);
    }
    public void ShopOn()
    {
        shopExit = false;
        shopBack.SetActive(true);
        shopUI.SetActive(true);
    }
    public void ShopOff()
    {
        selectedItemLoc = -1;
        shopExit = true;
        unLockWait();
        ClearTalk();
        TalkEvent();
        foreach(Image i in shopSelectedItem)
        {
            Color color = i.color;
            color.a = 0f;
            i.color = color;
        }
        shopBack.SetActive(false);
        shopUI.SetActive(false);
    }
    public void BuyItemRequest()
    {
        if (itemQuantityInput.text == null) return;
        int num = Convert.ToInt32(itemQuantityInput.text);
        if (num <= 0) return;
        itemQuantityInput.text = null;
        enterNumberUI.SetActive(false);
        sellRequest(selectedItemLoc, num);
        unLockWait();
        ClearTalk();
        TalkEvent();
        //sellRequest(selectedItemLoc, ����);
    }
    public void GetItem(Item item, int num)
    {
        int idx = im.FindItem(item.ItemId);
        if (idx == -1) im.ItemInsert(item, num);
        else
        {
            im.ItemQuantityIncrease(idx, num);
        }
    }
    public void EnterItemQuantity() // ������ ������ ���� �Է¹޴� �Լ�
    {
        if (selectedItemLoc == -1)
        {
            unLockWait();
            ClearTalk();
            TalkEvent();
            return;
        }
        if (im.IsFull && im.FindItem(getItemId(selectedItemLoc)) == -1) noEmptySpace = true;
        else noEmptySpace = false;
        if(noEmptySpace)
        {
            unLockWait();
            ClearTalk();
            TalkEvent();
            return;
        }
        enterNumberUI.SetActive(true);
    }
    public void SelectedItem(int idx)
    {
        Color color;
        if (selectedItemLoc == idx)
        {
            color = shopSelectedItem[selectedItemLoc].color;
            color.a = 0f;
            shopSelectedItem[selectedItemLoc].color = color;
            selectedItemLoc = -1;
        }
        else
        {
            if(selectedItemLoc != -1)
            {
                color = shopSelectedItem[selectedItemLoc].color;
                color.a = 0f;
                shopSelectedItem[selectedItemLoc].color = color;
            }
            selectedItemLoc = idx;
            color = shopSelectedItem[selectedItemLoc].color;
            color.a = 0.5f;
            shopSelectedItem[selectedItemLoc].color = color;
        }
    }

    /*public void TalkOn(int num)
    {
        if(num == 1) TalkUI.SetActive(true);
        Talking(num);
        

    }

    void Talking(int n)
    {
        if (IsQuest)
        {
            if (!success)
            {
                if (n < dm.QuestAskDiaData[curquestnum].Dialogue.Count - 1)
                {
                    textdata = dm.QuestAskDiaData[curquestnum].Dialogue[n - 1].ToCharArray();
                    TalkAni = TalkAnime();
                    StartCoroutine(TalkAni);
                }
                if (n == dm.QuestAskDiaData[curquestnum].Dialogue.Count - 1)
                {
                    Answer = true;
                    textdata = dm.QuestAskDiaData[curquestnum].Dialogue[n - 1].ToCharArray();
                    TalkAni = TalkAnime();
                    StartCoroutine(TalkAni);
                }
                if(accept)
                {
                    if (n == dm.QuestAskDiaData[curquestnum].Dialogue.Count)
                    {
                        TalkOff();
                        ani.SetTrigger("wait");
                    }
                }
                
            }
            else
            {
                if (n <= dm.QuestSuccessDiaData[curquestnum].Dialogue.Count) 
                {
                    textdata = dm.QuestSuccessDiaData[curquestnum].Dialogue[n - 1].ToCharArray();
                    TalkAni = TalkAnime();
                    StartCoroutine(TalkAni);
                }
                else 
                {
                    TalkOff();
                    QuestReward();
                    ClearQuestInfo();
                    accept = false;
                    success = false;
                }
            }
        }
        else
        {
            if (n <= dm.DiaData[CurNpcid - 1].Dialogue.Count) 
            {
                textdata = dm.DiaData[CurNpcid - 1].Dialogue[n - 1].ToCharArray();
                TalkAni = TalkAnime(); 
                StartCoroutine(TalkAni);
                
            }
            else 
            {
                TalkOff();
            }
        }
        
    }

    public void SkipTalk(int n)
    {
        StopCoroutine(TalkAni); 
        tmp = null;
        Istalking = false;
        if (IsQuest)
        {
            if (!success)
            {
                if(n < (dm.QuestAskDiaData[curquestnum].Dialogue.Count - 1))
                {
                    talk.text = dm.QuestAskDiaData[curquestnum].Dialogue[n - 1];
                }
                if(n == (dm.QuestAskDiaData[curquestnum].Dialogue.Count - 1))
                {
                    if (accept)
                    {
                        talk.text = dm.QuestAskDiaData[curquestnum].Dialogue[n];
                    }
                    else
                    {
                        talk.text = dm.QuestAskDiaData[curquestnum].Dialogue[n - 1];
                    }
                }
            }
            else
            {
                talk.text = dm.QuestSuccessDiaData[curquestnum].Dialogue[n - 1];
            }
        }
        else
        {
            talk.text = dm.DiaData[CurNpcid - 1].Dialogue[n - 1];
        }
        if (Answer)
        {
            OpenYesOrNoButton();
            Answer = false;
        }
        
    }
    */
    public void TalkEvent() // �÷��̾�� npc �� ��ȭ �̺�Ʈ �߻� �� npc���Լ� ���� ��� �޾Ƽ� ��ȭâ UI�� �����
    {
        talkUI.SetActive(true);
        if(isTalking) // ��� ��� �߿� ȣ��Ǿ����� �ش� ��� ������ ����� ��ŵ�ϰ� �ѹ��� ���
        {
            SkipTalk();
        }
        else
        {
            if (!npcTalk()) return; // ��ȯ���� false�� ���� ��ȭ�� �� ���ʰ� ���� �ƴ϶�� �� �ǹ�
            if (finishTalk) // ��ȭ ����
            {
                TalkDone();
            }
            else
            {
                talkAni = TalkAnime();
                StartCoroutine(talkAni);
            }
        }
    }

    public void SkipTalk() // ��� ������ ����� ��ŵ�ϰ� ��� ��ü�� �ѹ��� ����ϰ� �ϴ� �Լ�
    {
        StopCoroutine(talkAni);
        tmp = null;
        talk.text = new string(textData);
        //talk.text = Convert.ToString(textdata);
        isTalking = false;
    }

    public void ClearTalk() // ���� ������� ��ȭ �ߴ��ϰ� ��ȭ ������ �� ���·� ���� �Լ�
    {
        isTalking = false;
        StopCoroutine(talkAni);
        tmp = null;
    }

    public void TalkDone() // ��ȭ �̺�Ʈ ����
    {
        talkUI.SetActive(false);
        finishTalk = false;
    }

    IEnumerator TalkAnime()
    {
        for (int i = 0; i < textData.Length; i++)
        {
            isTalking = true;
            tmp += textData[i];
            talk.text = tmp;
            yield return wfs;
        }
        tmp = null;
        isTalking = false;
        /*if(answer)
        {
            OpenYesOrNoButton();
            answer = false;
        }*/
        yield break;
    }
    
    /*public void TalkOff()
    {
        FinishTalk = true;
        TalkUI.SetActive(false);
    }*/

    public void OpenCurQuestList()
    {
        down.SetActive(false);
        up.SetActive(true);
        progressingQuest.SetActive(true);
    }
    public void CloseCurQuestList()
    {
        down.SetActive(true);
        up.SetActive(false);
        progressingQuest.SetActive(false);
    }
    public void ChangeTalkNpc(int id, string name)
    {
        if (curNpcId != id)
        {
            curNpcId = id;
            npcImage.sprite = npcImages[id - 1];
            npcName.text = name;
            /*if (qm.questData[curquestnum].npcid == id)
            {
                IsQuest = true;
            }
            else
            {
                IsQuest = false;
            }*/
        }
    }

    public void OpenYesOrNoButton()
    {
        yes.SetActive(true);
        no.SetActive(true);
    }

    public void CloseYesOrNoButton()
    {
        yes.SetActive(false);
        no.SetActive(false);
    }

    public void QuestAccept()
    {
        accept = true;
        unLockWait();
        CloseYesOrNoButton();
        ShowQuestInfo();
        ClearTalk();
        //isTalking = false;
        //StopCoroutine(talkAni);
        //tmp = null;
        TalkEvent();
        //textData = dm.QuestAskDiaData[curQuestNum].Dialogue[dm.QuestAskDiaData[curQuestNum].Dialogue.Count - 1].ToCharArray();
        //talkAni = TalkAnime(); 
        //StartCoroutine(talkAni);
    }
    public void QuestRefuse()
    {
        //Debug.Log("����Ʈ�ź�");
        accept = false; 
        unLockWait();
        CloseYesOrNoButton();
        ClearTalk();
        //isTalking = false;
        //StopCoroutine(talkAni);
        //tmp = null;
        TalkEvent();
    }
    public void ShowQuestInfo()
    {
        questUI.SetActive(true);
        questTitle.text = qm.QuestDataList[curQuestNum-1].Title;
        questNpcName.text = qm.QuestDataList[curQuestNum-1].NpcName;
        questInfo.text = qm.QuestDataList[curQuestNum-1].Info;
        questT.color = Color.white;
        questTitle.color = Color.white;
        questNpc.color = Color.white;
        questNpcName.color = Color.white;
        questInfo.color = Color.white;
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            questReqName[i].text = qm.QuestDataList[curQuestNum - 1].Req_Name[i] + " " + "0 / " + Convert.ToString(qm.QuestDataList[curQuestNum - 1].Req_Num[i]);
            //questReqCurNum[i].text = Convert.ToString(0);
            //questReqTotal[i].text = Convert.ToString(qm.QuestDataList[curQuestNum - 1].Req_Num[i]);
            reqList[i].SetActive(true);
            questReqName[i].color = Color.white;
            //questReqCurNum[i].color = Color.white;
            //slash[i].color = Color.white;
            //questReqTotal[i].color = Color.white;
            curQuestReqNum.Add(0);
        }
    }
    public void ClearQuestInfo()
    {
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Req_Id.Count; i++)
        {
            reqList[i].SetActive(false);
        }
        questUI.SetActive(false);
    }

    public void QuestUpdate(string type, int id = 0)
    {
        if (!accept) return; // ������ ����Ʈ�� ���� ��� Ż��
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            //Debug.Log(curQuestReqNum[i]);
            //Debug.Log(qm.QuestDataList[curQuestNum - 1].Req_Num[i]);
            if (curQuestReqNum[i] == qm.QuestDataList[curQuestNum - 1].Req_Num[i]) continue;
            //Debug.Log(qm.QuestDataList[curQuestNum - 1].Type[i]);
            if (qm.QuestDataList[curQuestNum - 1].Type[i] == type && (id == 0 || qm.QuestDataList[curQuestNum - 1].Req_Id[i] == id)) // id�� ����Ʈ ���� 0���� ���� ��� �ĺ��ؾ� �� ���̵� ������ ���ٴ� ���� �ǹ�
            {
                if(curQuestReqNum[i] < qm.QuestDataList[curQuestNum - 1].Req_Num[i])
                {
                    curQuestReqNum[i]++;
                    questReqName[i].text = qm.QuestDataList[curQuestNum - 1].Req_Name[i] + " " + Convert.ToString(curQuestReqNum[i]) + " / " + Convert.ToString(qm.QuestDataList[curQuestNum - 1].Req_Num[i]);
                }
                if(curQuestReqNum[i] == qm.QuestDataList[curQuestNum - 1].Req_Num[i]) finishReqNum++;
            }
        }
        if (finishReqNum == qm.QuestDataList[curQuestNum-1].Type.Count)
        {
            QuestSuccess();
        }
    }

    public void QuestSuccess()
    {
        finishReqNum = 0;
        success = true;
        //ani.SetTrigger("success");
        questT.color = Color.yellow;
        questTitle.color = Color.yellow;
        questNpc.color = Color.yellow;
        questNpcName.color = Color.yellow;
        questInfo.color = Color.yellow;
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            curQuestReqNum.RemoveAt(i);
            questReqName[i].color = Color.yellow;
            //questReqCurNum[i].color = Color.yellow;
            //slash[i].color = Color.yellow;
            //questReqTotal[i].color = Color.yellow;
        }
    }
    
    public void QuestDone()
    {
        questUI.SetActive(false);
        if (curQuestNum == 1) skillImage.SetActive(true);
        accept = false;
        success = false;
        QuestReward();
        curQuestNum++;
        if (curQuestNum <= qm.QuestDataList.Count) curQuestNpcId = qm.QuestDataList[curQuestNum - 1].NpcId;
        else
        {
            curQuestNpcId = 0;
        }
    }

    public void QuestReward()
    {
        /*questUI.SetActive(false);
        if(curQuestNum == 1) skillImage.SetActive(true);
        accept = false;
        success = false;*/
        for(int i = 0;  i < qm.QuestDataList[curQuestNum-1].RewardType.Count; i++)
        {
            if (qm.QuestDataList[curQuestNum - 1].RewardType[i] == "exp")
            {
                ExpUp(qm.QuestDataList[curQuestNum - 1].Reward[i]);
            } 
            if(qm.QuestDataList[curQuestNum - 1].RewardType[i] == "gold")
            {

            }
        }
        //QuestState.SetActive(false);
    }

    public void SelectMagicStoneUIOn()
    {
        if (curQuestNum == 1 && !accept) return; // 1�� ����Ʈ�� ���� �������� ���� ��� ��Ȱ��ȭ
        selectMagicStoneUI.SetActive(true);
    }
    public void SelectMagicStoneUIOff()
    {
        selectMagicStoneUI.SetActive(false);
    }

    public void SwitchingMagicStone()
    {
        if(waterSelected)
        {
            skill.sprite = skImages[0];
            setMagic(skills[0]);
        }
        else if(dirtSelected)
        {
            skill.sprite = skImages[1];
            setMagic(skills[1]);

        }
        else if(windSelected)
        {
            skill.sprite = skImages[2];
            setMagic(skills[2]);

        }
        //skillImage.SetActive(true);
        QuestUpdate("����������");
        SelectMagicStoneUIOff();
    }
}
