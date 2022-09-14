using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public delegate void GetTalkData(); // npc한테서 대사를 전달받는 대리자
    public delegate void UnLockWaiting(); // 기다리고 있는 상황을 기다리지 않아도 되도록 풀어주는 대리자
    public GetTalkData npcTalk;
    public UnLockWaiting unLockWait;
    //serializefield 해야됨
    [SerializeField] private GameObject DM;
    [SerializeField] private GameObject QM;
    [SerializeField] private GameObject QuestState;
    [SerializeField] private GameObject Yes;
    [SerializeField] private GameObject No;
    [SerializeField] private GameObject skillLocked;
    [SerializeField] private GameObject Up;
    [SerializeField] private GameObject Down;
    [SerializeField] private GameObject QuestList;
    [SerializeField] private GameObject TalkUI;
    [SerializeField] private GameObject QuestUI;
    [SerializeField] private GameObject Inven;
    [SerializeField] private GameObject[] reqList;
    [SerializeField] private Image hpGraph;
    [SerializeField] private TMPro.TMP_Text MAXhp;
    [SerializeField] private TMPro.TMP_Text hp;
    [SerializeField] private Image mpGraph;
    [SerializeField] private TMPro.TMP_Text MAXmp;
    [SerializeField] private TMPro.TMP_Text mp;
    [SerializeField] private Image expGraph;
    [SerializeField] private TMPro.TMP_Text MAXexp;
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
    [SerializeField] private TMPro.TMP_Text[] questReqCurNum;
    [SerializeField] private TMPro.TMP_Text[] slash;
    [SerializeField] private TMPro.TMP_Text[] questReqTotal;
    [SerializeField] private Sprite[] sp;

    //public bool Istalking = false;
    //public bool FinishTalk = false;
    //public bool skDisable = false;
   // public bool accept = false;
    //public int curMapNum = 0;

    private IEnumerator talkAni;
    private WaitForSeconds wfs;

    DialogueManager dm;
    QuestManager qm;

    int curMapNum = 0;
    int curNpcId = 0;
    int i = 0;
    int curHp = 0;
    int maxHp = 0;
    int curMp = 0;
    int maxMp = 0;
    int curExp = 0;
    int maxExp = 0;
    int curQuestNum = 1;
    int curQuestNpcId; //현재 진행 가능한 혹은 진행중인 퀘스트를 가지고 있는 NPC의 ID
    int finishReqNum = 0; // 퀘스트 조건들에서 완료한 조건 수
    float percent = 0;
    string tmp = null;
    char[] textData = null;
    bool isTalking = false;
   // bool isQuest = false;
   // bool answer = false;
    bool success = false;
    bool finishTalk = false;
    bool skDisable = false;
    bool accept = false;
    //bool doneQuest = false; // 모든 퀘스트 완료한 경우
    //bool startTalk = true;

    Animator ani;

    void Start()
    {
        this.ani = QuestState.GetComponent<Animator>();
        this.talkAni = TalkAnime();
        this.wfs = new WaitForSeconds(0.05f);
        this.dm = DM.GetComponent<DialogueManager>();
        this.qm = QM.GetComponent<QuestManager>();
        this.curHp = Convert.ToInt32(MAXhp.text);
        this.maxHp = Convert.ToInt32(MAXhp.text);
        this.curMp = Convert.ToInt32(MAXmp.text);
        this.maxMp = Convert.ToInt32(MAXmp.text);
        this.maxExp = Convert.ToInt32(MAXexp.text);
        this.curQuestNpcId = qm.QuestDataList[curQuestNum - 1].NpcId;
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

    /*public void GetMessage(string s) // 메시지를 받아 각 동작을 수행하는 방식? 고민중
    {
        if(s == "퀘스트수락")
        {

        }
    }*/

    public void TeleportMap(int n)
    {
        curMapNum = n;
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
    public int GetCurMp()
    {
        return curMp;
    }
    public void MPDown(int n)
    {
        curMp -= n;
        //percent = Convert.ToDouble(n) / Convert.ToDouble(maxmp);
        percent = (float)((float)(n) * (1.0 / (float)(maxMp)));
        mpGraph.fillAmount -= percent;
        mp.text = Convert.ToString(curMp);
    }
    public void HPDown(int n)
    {
        curHp -= n;
        percent = (float)((float)(n) * (1.0 / (float)(maxHp)));
        //percent = Convert.ToDouble(n) / Convert.ToDouble(maxhp);
        hpGraph.fillAmount -= percent;
        hp.text = Convert.ToString(curHp);
    }
    public void ExpUp(int n)
    {
        curExp += n;
        percent = (float)((float)(n) * (1.0 / (float)(maxExp)));
        //percent = Convert.ToDouble(n) / Convert.ToDouble(maxexp);
        expGraph.fillAmount += percent;
        exp.text = Convert.ToString(curExp);
    }

    public void InventoryOn()
    {
        Inven.SetActive(true);
    }
    public void InventoryOff()
    {
        Inven.SetActive(false);
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
    public void TalkEvent() // 플레이어와 npc 간 대화 이벤트 발생
    {
        TalkUI.SetActive(true);
        if(isTalking) // 대사 출력 중에 호출되었으면 해당 대사 순차적 출력을 스킵하고 한번에 출력
        {
            SkipTalk();
        }
        else
        {
            npcTalk();
            if (finishTalk) // 대화 종료
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

    public void SkipTalk() // 대사 순차적 출력을 스킵하고 대사 전체를 한번에 출력하게 하는 함수
    {
        StopCoroutine(talkAni);
        tmp = null;
        talk.text = new string(textData);
        //talk.text = Convert.ToString(textdata);
        isTalking = false;
    }

    public void TalkDone() // 대화 이벤트 종료
    {
        TalkUI.SetActive(false);
        finishTalk = false;
    }

    IEnumerator TalkAnime()
    {
        for (i = 0; i < textData.Length; i++)
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
        Down.SetActive(false);
        Up.SetActive(true);
        QuestList.SetActive(true);
    }
    public void CloseCurQuestList()
    {
        Down.SetActive(true);
        Up.SetActive(false);
        QuestList.SetActive(false);
    }
    public void ChangeTalkNpc(int id, string name)
    {
        if (curNpcId != id)
        {
            curNpcId = id;
            npcImage.sprite = sp[id - 1];
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
        Yes.SetActive(true);
        No.SetActive(true);
    }

    public void CloseYesOrNoButton()
    {
        Yes.SetActive(false);
        No.SetActive(false);
        unLockWait();
    }

    public void QuestAccept()
    {
        accept = true;
        CloseYesOrNoButton();
        ShowQuestInfo();
        TalkEvent();
        //textData = dm.QuestAskDiaData[curQuestNum].Dialogue[dm.QuestAskDiaData[curQuestNum].Dialogue.Count - 1].ToCharArray();
        //talkAni = TalkAnime(); 
        //StartCoroutine(talkAni);
    }
    public void QuestRefuse()
    {
        accept = false;
        CloseYesOrNoButton();
        TalkEvent();
    }
    public void ShowQuestInfo()
    {
        QuestUI.SetActive(true);
        questTitle.text = qm.QuestDataList[curQuestNum-1].Title;
        questNpcName.text = qm.QuestDataList[curQuestNum-1].NpcName;
        questInfo.text = qm.QuestDataList[curQuestNum-1].Info;
        Debug.Log(qm.QuestDataList[curQuestNum - 1].Type.Count);
        for(int i = 0; i < qm.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            questReqName[i].text = qm.QuestDataList[curQuestNum - 1].Req_Name[i];
            //questReqName[i].SetActive(true);
            questReqCurNum[i].text = Convert.ToString(0);
            //questReqCurNum[i].SetActive(true);
            //slash[i].SetActive(true);
            questReqTotal[i].text = Convert.ToString(qm.QuestDataList[curQuestNum - 1].Req_Num[i]);
            //questReqTotal[i].SetActive(true);
            reqList[i].SetActive(true);
        }
    }
    public void ClearQuestInfo()
    {
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Req_Id.Count; i++)
        {
            reqList[i].SetActive(false);
        }
        QuestUI.SetActive(false);
    }

    public void QuestUpdate(string type, int id = 0)
    {
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Req_Id.Count; i++)
        {
            if (Convert.ToInt32(questReqCurNum[i]) == qm.QuestDataList[curQuestNum - 1].Req_Num[i]) continue;
            if (qm.QuestDataList[curQuestNum - 1].Type[i] == type && (id == 0 || qm.QuestDataList[curQuestNum - 1].Req_Id[i] == id)) // id가 디폴트 값인 0으로 들어온 경우 식별해야 할 아이디 정보가 없다는 것을 의미
            {
                if(Convert.ToInt32(questReqCurNum[i]) < qm.QuestDataList[curQuestNum - 1].Req_Num[i])
                {
                    questReqCurNum[i].text = Convert.ToString(Convert.ToInt32(questReqCurNum[i]) + 1);
                }
                if(Convert.ToInt32(questReqCurNum[i]) == qm.QuestDataList[curQuestNum - 1].Req_Num[i]) finishReqNum++;
            }
        }
        if (finishReqNum == qm.QuestDataList[curQuestNum-1].Req_Id.Count)
        {
            QuestSuccess();
        }
    }

    public void QuestSuccess()
    {
        finishReqNum = 0;
        success = true;
        ani.SetTrigger("success");
        questT.color = Color.yellow;
        questTitle.color = Color.yellow;
        questNpc.color = Color.yellow;
        questNpcName.color = Color.yellow;
        questInfo.color = Color.yellow;
        for (int i = 0; i < qm.QuestDataList[curQuestNum - 1].Req_Id.Count; i++)
        {
            questReqName[i].color = Color.yellow;
            questReqCurNum[i].color = Color.yellow;
            slash[i].color = Color.yellow;
            questReqTotal[i].color = Color.yellow;
        }
    }

    public void QuestReward()
    {
        accept = false;
        success = false;
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
        curQuestNum++;
        if (curQuestNum <= qm.QuestDataList.Count) curQuestNpcId = qm.QuestDataList[curQuestNum - 1].NpcId;
        else
        {
            curQuestNpcId = 0;
        }
        QuestState.SetActive(false);
    }

}
