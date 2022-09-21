using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    enum DialogueState // 무슨 대화를 해야하는지 그 상태 나열
    {
        normal, questAsk, questRefuse, questAccept, questDoing, questSuccess, questReply
    };
    [SerializeField] private GameObject GM;
    [SerializeField] private GameObject DM;
    [SerializeField] private GameObject QM;
    [SerializeField] private GameObject questBallon;
    [SerializeField] private int npcId;
    //public int npcid;
    GameManager gm;
    DialogueManager dm;
    QuestManager qm;
    Animator ani;
    DialogueData d; // npc가 현재 말해야 할 대사 데이터 저장
    //int npcId;
    List<int> questNumList = new List<int>(); // npc가 갖고 있는 퀘스트 번호
    int diaIdx = 0; // 다음으로 보낼 대사 데이터 위치 정보
    DialogueState ds;
    bool wait = false; // 퀘스트 요청 시 플레이어의 수락여부를 알기 전까지 wait를 true로 설정하여 대화가 더 이상 진행되지 않도록 함
    void Start()
    {
        gm = GM.GetComponent<GameManager>();
        dm = DM.GetComponent<DialogueManager>();
        qm = QM.GetComponent<QuestManager>();
        ds = DialogueState.normal;
        ani = questBallon.GetComponent<Animator>();
        //d = GameObject.Find("DialogueManager").GetComponent<DialogueManager>().DiaData[npcId - 1];
    }

    void Update()
    {
        if(gm.CurQuestNpcId == this.npcId) // 플레이어가 퀘스트를 받아야 하는 경우
        {
            if(!gm.Accept) questBallon.SetActive(true); // 퀘스트 말풍선 활성화
            else if(gm.Accept && !gm.Success) ani.SetTrigger("wait");
            else if(gm.Success) ani.SetTrigger("success");
        }
    }

    public int NpcId
    {
        get
        {
            return npcId;
        }
    }
    
    public void unLockWait() // 기다림 상태 해제
    {
        wait = false;
    }
   
    public void DialogueReady()
    {
        //Debug.Log(wait);
        if (wait) return;
        if (diaIdx == 0)
        {
            SetDiaState(); // 대화 시작전 어떤 대화를 가져와야 하는지 그 상태 체크
            GetDiaData();
            gm.ChangeTalkNpc(npcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count) // 아직 다음으로 출력할 대사가 존재함
        {
            gm.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count) // 퀘스트 요청하는 상태에서 퀘스트 요청 대사들 중 마지막 순서 대사를 말하고 있는 경우
            {
                gm.OpenYesOrNoButton();
                wait = true;
                diaIdx = 0;
            }
        }
        else // 대사 끝까지 다 출력 완료한 경우
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            if(ds == DialogueState.questReply) gm.QuestUpdate("NPC대화", npcId);
            if (ds == DialogueState.questSuccess)
            {
                questBallon.SetActive(false);
                gm.QuestReward();
            }
        }
    }

    void SetDiaState()
    {
        if (gm.CurQuestNpcId == this.npcId) //현재 퀘스트 번호가 이 npc가 담당한 퀘스트인 경우
        {
            if (!gm.Accept) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!gm.Accept) ds = DialogueState.questRefuse; // 플레이어가 퀘스트를 수락하지 않은 상태인 경우
                else ds = DialogueState.questAccept; // 플레이어가 퀘스트를 수락한 경우
            }
            else if (ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (gm.Accept && !gm.Success) ds = DialogueState.questDoing;
            else if (gm.Success)
            {
                ds = DialogueState.questSuccess;
            }
        }
        else if(gm.CurQuestNpcId != 0 && gm.Accept) // 아직 수행하는 퀘스트가 있는 경우
        {
            for(int i = 0; i < qm.QuestDataList[gm.CurQuestNum - 1].Type.Count; i++)
            {
                if (qm.QuestDataList[gm.CurQuestNum - 1].Type[i] == "NPC대화" && qm.QuestDataList[gm.CurQuestNum - 1].Req_Id[i] == this.npcId) 
                {
                    ds = DialogueState.questReply;
                }
            }
        }
        else ds = DialogueState.normal;
        Debug.Log(ds);
    }

    void GetDiaData()
    {
        switch(ds)
        {
            case DialogueState.normal:
                d = dm.DiaData[npcId - 1];
                break;
            case DialogueState.questAsk:
                d = dm.QuestAskDiaData[gm.CurQuestNum - 1];
                break;
            case DialogueState.questAccept:
                d = dm.QuestAcceptDiaData[gm.CurQuestNum - 1];
                break;
            case DialogueState.questRefuse:
                d = dm.QuestRefuseDiaData[gm.CurQuestNum - 1];
                break;
            case DialogueState.questDoing:
                d = dm.QuestDoingDiaData[gm.CurQuestNum - 1];
                break;
            case DialogueState.questSuccess:
                d = dm.QuestSuccessDiaData[gm.CurQuestNum - 1];
                break;
            case DialogueState.questReply:
                for (int i = 0; i < dm.QuestReplyDiaData.Count; i++)
                {
                    if (this.npcId == dm.QuestReplyDiaData[i].NpcId)
                    {
                        d = dm.QuestReplyDiaData[i];
                    }
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "Player")
        {
            //DialogueReady();
            //gm.ChangeTalkNpc(npcId, d.NpcName);
            if (gm.npcTalk != null) gm.npcTalk = null;
            if (gm.unLockWait != null) gm.unLockWait = null;
            gm.npcTalk += DialogueReady;
            gm.unLockWait += unLockWait;
        }
    }
    /*void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            //DialogueReady();
        }
    }
    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            gm.npcTalk -= DialogueReady();
        }
    }*/
}
