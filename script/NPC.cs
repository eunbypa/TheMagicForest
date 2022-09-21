using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    enum DialogueState // ���� ��ȭ�� �ؾ��ϴ��� �� ���� ����
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
    DialogueData d; // npc�� ���� ���ؾ� �� ��� ������ ����
    //int npcId;
    List<int> questNumList = new List<int>(); // npc�� ���� �ִ� ����Ʈ ��ȣ
    int diaIdx = 0; // �������� ���� ��� ������ ��ġ ����
    DialogueState ds;
    bool wait = false; // ����Ʈ ��û �� �÷��̾��� �������θ� �˱� ������ wait�� true�� �����Ͽ� ��ȭ�� �� �̻� ������� �ʵ��� ��
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
        if(gm.CurQuestNpcId == this.npcId) // �÷��̾ ����Ʈ�� �޾ƾ� �ϴ� ���
        {
            if(!gm.Accept) questBallon.SetActive(true); // ����Ʈ ��ǳ�� Ȱ��ȭ
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
    
    public void unLockWait() // ��ٸ� ���� ����
    {
        wait = false;
    }
   
    public void DialogueReady()
    {
        //Debug.Log(wait);
        if (wait) return;
        if (diaIdx == 0)
        {
            SetDiaState(); // ��ȭ ������ � ��ȭ�� �����;� �ϴ��� �� ���� üũ
            GetDiaData();
            gm.ChangeTalkNpc(npcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count) // ���� �������� ����� ��簡 ������
        {
            gm.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count) // ����Ʈ ��û�ϴ� ���¿��� ����Ʈ ��û ���� �� ������ ���� ��縦 ���ϰ� �ִ� ���
            {
                gm.OpenYesOrNoButton();
                wait = true;
                diaIdx = 0;
            }
        }
        else // ��� ������ �� ��� �Ϸ��� ���
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            if(ds == DialogueState.questReply) gm.QuestUpdate("NPC��ȭ", npcId);
            if (ds == DialogueState.questSuccess)
            {
                questBallon.SetActive(false);
                gm.QuestReward();
            }
        }
    }

    void SetDiaState()
    {
        if (gm.CurQuestNpcId == this.npcId) //���� ����Ʈ ��ȣ�� �� npc�� ����� ����Ʈ�� ���
        {
            if (!gm.Accept) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!gm.Accept) ds = DialogueState.questRefuse; // �÷��̾ ����Ʈ�� �������� ���� ������ ���
                else ds = DialogueState.questAccept; // �÷��̾ ����Ʈ�� ������ ���
            }
            else if (ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (gm.Accept && !gm.Success) ds = DialogueState.questDoing;
            else if (gm.Success)
            {
                ds = DialogueState.questSuccess;
            }
        }
        else if(gm.CurQuestNpcId != 0 && gm.Accept) // ���� �����ϴ� ����Ʈ�� �ִ� ���
        {
            for(int i = 0; i < qm.QuestDataList[gm.CurQuestNum - 1].Type.Count; i++)
            {
                if (qm.QuestDataList[gm.CurQuestNum - 1].Type[i] == "NPC��ȭ" && qm.QuestDataList[gm.CurQuestNum - 1].Req_Id[i] == this.npcId) 
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
