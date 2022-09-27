using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Normal_Npc : Npc
{
    enum DialogueState // ���� ��ȭ�� �ؾ��ϴ��� �� ���� ����
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
    DialogueData d; // npc�� ���� ���ؾ� �� ��� ������ ����
    //int npcId;
    //List<int> questNumList = new List<int>(); // npc�� ���� �ִ� ����Ʈ ��ȣ
    int diaIdx = 0; // �������� ���� ��� ������ ��ġ ����
    DialogueState ds;
    //bool wait = false; // ����Ʈ ��û �� �÷��̾��� �������θ� �˱� ������ wait�� true�� �����Ͽ� ��ȭ�� �� �̻� ������� �ʵ��� ��
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
        if(gm.CurQuestNpcId == NpcId) // �÷��̾ ����Ʈ�� �޾ƾ� �ϴ� ���
        {
            if(!gm.Accept) questBallon.SetActive(true); // ����Ʈ ��ǳ�� Ȱ��ȭ
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
    
    public void unLockWait() // ��ٸ� ���� ����
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
            SetDiaState(); // ��ȭ ������ � ��ȭ�� �����;� �ϴ��� �� ���� üũ
            GetDiaData();
            gm.ChangeTalkNpc(NpcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count) // ���� �������� ����� ��簡 ������
        {
            gm.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count) // ����Ʈ ��û�ϴ� ���¿��� ����Ʈ ��û ���� �� ������ ���� ��縦 ���ϰ� �ִ� ���
            {
                Wait = true;
                gm.OpenYesOrNoButton();
                diaIdx = 0;
            }
        }
        else // ��� ������ �� ��� �Ϸ��� ���
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            if(ds == DialogueState.questReply) gm.QuestUpdate("NPC��ȭ", NpcId);
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
        if (gm.CurQuestNpcId == NpcId) //���� ����Ʈ ��ȣ�� �� npc�� ����� ����Ʈ�� ���
        {
            if (ds == DialogueState.normal || ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!gm.Accept) ds = DialogueState.questRefuse; // �÷��̾ ����Ʈ�� �������� ���� ������ ���
                else ds = DialogueState.questAccept; // �÷��̾ ����Ʈ�� ������ ���
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
        else if(gm.CurQuestNpcId != 0 && gm.Accept) // ���� �����ϴ� ����Ʈ�� �ִ� ���
        {
            for(int i = 0; i < qm.QuestDataList[gm.CurQuestNum - 1].Type.Count; i++)
            {
                if (qm.QuestDataList[gm.CurQuestNum - 1].Type[i] == "NPC��ȭ" && qm.QuestDataList[gm.CurQuestNum - 1].Req_Id[i] == NpcId) 
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
                    if(gm.CurQuestNum == dm.QuestReplyDiaData[i].Item1) // ���� ����Ʈ ��ȣ�� ��ġ�ϴ� Ʃ�� Item1(����Ʈ ��ȣ) ã��
                    {
                        for(int j = 0; j < dm.QuestReplyDiaData[i].Item2.Count; j++)
                        {
                            if (NpcId == dm.QuestReplyDiaData[i].Item2[j].NpcId) // npcId�� ������ �ش� npc�� ��絥���Ͱ� ����� DialogueData ��ü�� ã�� ���� �ǹ���
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
