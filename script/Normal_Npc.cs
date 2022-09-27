using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Normal_Npc : Npc
{
    enum DialogueState // 무슨 대화를 해야하는지 그 상태 나열
    {
        normal, questAsk, questRefuse, questAccept, questDoing, questSuccess, questReply
    };
    [SerializeField] private GameObject gM;
    [SerializeField] private GameObject dM;
    [SerializeField] private GameObject qM;
    [SerializeField] private GameObject questBallon;
    //[SerializeField] private int npcId;
 
    GameManager gm;
    DialogueManager dm;
    QuestManager qm;
    Animator ani;
    DialogueData d; // npc가 현재 말해야 할 대사 데이터 저장
    //int npcId;
    //List<int> questNumList = new List<int>(); // npc가 갖고 있는 퀘스트 번호
    int diaIdx = 0; // 다음으로 보낼 대사 데이터 위치 정보
    DialogueState ds;
    //bool wait = false; // 퀘스트 요청 시 플레이어의 수락여부를 알기 전까지 wait를 true로 설정하여 대화가 더 이상 진행되지 않도록 함
    void Start()
    {
        gm = gM.GetComponent<GameManager>();
        dm = dM.GetComponent<DialogueManager>();
        qm = qM.GetComponent<QuestManager>();
        ds = DialogueState.normal;
        ani = questBallon.GetComponent<Animator>();
        //d = GameObject.Find("DialogueManager").GetComponent<DialogueManager>().DiaData[npcId - 1];
    }

    void Update()
    {
        if(gm.CurQuestNpcId == NpcId) // 플레이어가 퀘스트를 받아야 하는 경우
        {
            if(!gm.Accept) questBallon.SetActive(true); // 퀘스트 말풍선 활성화
            else if(gm.Accept && !gm.Success) ani.SetTrigger("wait");
            else if(gm.Success) ani.SetTrigger("success");
        }
    }

    /*public int NpcId
    {
        get
        {
            return npcId;
        }
    }
    
    public void unLockWait() // 기다림 상태 해제
    {
        wait = false;
    }*/
   
    public override bool DialogueReady()
    {
        //Debug.Log(diaIdx);
        //Debug.Log(Wait);
        if (Wait) return false;
        if (diaIdx == 0)
        {
            SetDiaState(); // 대화 시작전 어떤 대화를 가져와야 하는지 그 상태 체크
            GetDiaData();
            gm.ChangeTalkNpc(NpcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count) // 아직 다음으로 출력할 대사가 존재함
        {
            gm.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count) // 퀘스트 요청하는 상태에서 퀘스트 요청 대사들 중 마지막 순서 대사를 말하고 있는 경우
            {
                Wait = true;
                gm.OpenYesOrNoButton();
                diaIdx = 0;
            }
        }
        else // 대사 끝까지 다 출력 완료한 경우
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            if(ds == DialogueState.questReply) gm.QuestUpdate("NPC대화", NpcId);
            if (ds == DialogueState.questSuccess)
            {
                //ds = DialogueState.normal;
                SetDiaState();
                questBallon.SetActive(false);
                gm.QuestDone();
            }
        }
        return true;
    }

    public override void SetDiaState()
    {
        if (gm.CurQuestNpcId == NpcId) //현재 퀘스트 번호가 이 npc가 담당한 퀘스트인 경우
        {
            if (ds == DialogueState.normal || ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!gm.Accept) ds = DialogueState.questRefuse; // 플레이어가 퀘스트를 수락하지 않은 상태인 경우
                else ds = DialogueState.questAccept; // 플레이어가 퀘스트를 수락한 경우
            }
            else if (ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (gm.Accept && !gm.Success) ds = DialogueState.questDoing;
            else if (ds != DialogueState.questSuccess && gm.Success)
            {
                ds = DialogueState.questSuccess;
            }
            else if(ds == DialogueState.questSuccess)
            {
                ds = DialogueState.normal;
            }
        }
        else if(gm.CurQuestNpcId != 0 && gm.Accept) // 아직 수행하는 퀘스트가 있는 경우
        {
            for(int i = 0; i < qm.QuestDataList[gm.CurQuestNum - 1].Type.Count; i++)
            {
                if (qm.QuestDataList[gm.CurQuestNum - 1].Type[i] == "NPC대화" && qm.QuestDataList[gm.CurQuestNum - 1].Req_Id[i] == NpcId) 
                {
                    ds = DialogueState.questReply;
                }
            }
        }
        else ds = DialogueState.normal;
        //Debug.Log(ds);
    }

    public override void GetDiaData()
    {
        switch(ds)
        {
            case DialogueState.normal:
                d = dm.DiaData[NpcId - 1];
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
                    if(gm.CurQuestNum == dm.QuestReplyDiaData[i].Item1) // 현재 퀘스트 번호와 일치하는 튜플 Item1(퀘스트 번호) 찾기
                    {
                        for(int j = 0; j < dm.QuestReplyDiaData[i].Item2.Count; j++)
                        {
                            if (NpcId == dm.QuestReplyDiaData[i].Item2[j].NpcId) // npcId가 같으면 해당 npc의 대사데이터가 저장된 DialogueData 객체를 찾은 것을 의미함
                            {
                                d = dm.QuestReplyDiaData[i].Item2[j];
                            }
                        }
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
            gm.unLockWait += UnLockWait;
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
