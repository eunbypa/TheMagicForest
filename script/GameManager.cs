using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public delegate void GetTalkData(); // npc한테서 대사를 전달받는 대리자
    public GetTalkData npcTalk;

    //serializefield 해야됨
    public GameObject DM;
    public GameObject QM;
    public GameObject QuestState;
    public GameObject Yes;
    public GameObject No;
    public GameObject skillLocked;
    public GameObject Up;
    public GameObject Down;
    public GameObject QuestList;
    public GameObject TalkUI;
    public GameObject QuestUI;
    public GameObject Inven;
    public Image hpGraph;
    public TMPro.TMP_Text MAXhp;
    public TMPro.TMP_Text hp;
    public Image mpGraph;
    public TMPro.TMP_Text MAXmp;
    public TMPro.TMP_Text mp;
    public Image expGraph;
    public TMPro.TMP_Text MAXexp;
    public TMPro.TMP_Text exp;
    public Image skill;
    public TMPro.TMP_Text coolTime;
    public Image npcImage;
    public TMPro.TMP_Text npcName;
    public TMPro.TMP_Text talk;
    public TMPro.TMP_Text questTitle;
    public TMPro.TMP_Text questNpc;
    public TMPro.TMP_Text questNpcName;
    public TMPro.TMP_Text questInfo;

    public Sprite[] sp;

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
    int curQuestNum = 0;
    int checkNum = 0;
    float percent = 0;
    string tmp = null;
    char[] textData = null;
    bool isTalking = false;
    bool isQuest = false;
    bool answer = false;
    bool success = false;
    bool finishTalk = false;
    bool skDisable = false;
    bool accept = false;
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
    }

    public int CurMapNum
    {
        get
        {
            return curMapNum;
        }
    }

    public bool Accept
    {
        get
        {
            return accept;
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
        if(answer)
        {
            OpenYesOrNoButton();
            answer = false;
        }
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
    public void ChangeTalkNpc(int id)
    {
        if (curNpcId != id)
        {
            curNpcId = id;
            npcImage.sprite = sp[id - 1];
            npcName.text = dm.DiaData[id - 1].NpcName;
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
        if (!accept)
        {
            //TalkOff();
            TalkDone();
        }
    }

    public void QuestAccept()
    {
        accept = true;
        CloseYesOrNoButton();
        ShowQuestInfo();
        textData = dm.QuestAskDiaData[curQuestNum].Dialogue[dm.QuestAskDiaData[curQuestNum].Dialogue.Count - 1].ToCharArray();
        talkAni = TalkAnime(); 
        StartCoroutine(talkAni);
    }
    public void ShowQuestInfo()
    {
        QuestUI.SetActive(true);
        questTitle.text = qm.questData[curQuestNum].Title;
        questNpcName.text = qm.questData[curQuestNum].NpcName;
        questInfo.text = qm.questData[curQuestNum].Info;
    }
    public void ClearQuestInfo()
    {
        QuestUI.SetActive(false);
    }

    public void QuestUpdate(string type)
    {
        if(qm.questData[curQuestNum].Type == type)
        {
            checkNum++;
            if(checkNum == qm.questData[curQuestNum].Requirement)
            {
                QuestSuccess();
            }
        }
    }

    public void QuestSuccess()
    {
        checkNum = 0;
        success = true;
        ani.SetTrigger("success");
        questTitle.color = Color.yellow;
        questNpc.color = Color.yellow;
        questNpcName.color = Color.yellow;
        questInfo.color = Color.yellow;
    }

    public void QuestReward()
    {
        if(qm.questData[curQuestNum].RewardType == "경험치")
        {
            ExpUp(qm.questData[curQuestNum].Reward);
        }
        curQuestNum++;
        QuestState.SetActive(false);
    }

}
