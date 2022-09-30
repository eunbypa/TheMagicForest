using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Normal_Npc
 * Description : �Ϲ� npc�� ����ϴ� Ŭ�����Դϴ�. �Ϲ� npc�� �÷��̾�� ����ϰ� ��ȭ�� �ϰų� ����Ʈ�� �� �� �ֽ��ϴ�. Npc Ŭ������ ��ӹ޽��ϴ�.
 */
public class Normal_Npc : Npc
{
    // npc�� ��ȭ ���¸� ���������� ǥ���ϱ� ���� enum�� ����߽��ϴ�.
    // normal : ����, questAsk : ����Ʈ ��û, questRefuse : ����Ʈ �ź�, qusetAccept : ����Ʈ ����, questDoing : ����Ʈ ������, questSuccess : ����Ʈ ����, questReply : ����Ʈ ����
    enum DialogueState
    {
        normal, questAsk, questRefuse, questAccept, questDoing, questSuccess, questReply
    };
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private GameObject gM; // ���� ������ GameManager
    [SerializeField] private GameObject dM; // ��ȭ ������ ������ DialogueManager
    [SerializeField] private GameObject qM; // ����Ʈ ������ ������ QuestManager
    [SerializeField] private GameObject questBallon; // ����Ʈ Ȱ��ȭ �� npc ���� �ߴ� ��ǳ��

    int diaIdx = 0; // ���� ������ ���� ��� ������ ��ġ
    DialogueState ds; // ��ȭ ����
    GameManager gm; // ���� ������ GameManager Ŭ���� ��ü
    DialogueManager dm; // ��ȭ ������ ������ DialogueManager Ŭ���� ��ü
    QuestManager qm; // ����Ʈ ������ ������ QuestManager Ŭ���� ��ü
    Animator ani; // ����Ƽ �ִϸ��̼� ������Ʈ
    DialogueData d; // npc�� ���� ���ؾ� �� ��� ������ ����

    void Start()
    {
        gm = gM.GetComponent<GameManager>(); // gM GameObject ��ü�� �Ҵ�� GameManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        dm = dM.GetComponent<DialogueManager>(); // dM GameObject ��ü�� �Ҵ�� DialogueManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        qm = qM.GetComponent<QuestManager>(); // qM GameObject ��ü�� �Ҵ�� QuestManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        ds = DialogueState.normal; // npc�� ��ȭ ���¸� �⺻������ �ʱ�ȭ�մϴ�.
        ani = questBallon.GetComponent<Animator>(); // questBallon GameObject ��ü���� Animator ������Ʈ�� �����ɴϴ�.
    }

    void Update()
    {
        if(gm.CurQuestNpcId == NpcId) // ���� �÷��̾ ���� ������ ����Ʈ�� ���� npc�� ���
        {
            if(!gm.Accept) questBallon.SetActive(true); // ���� �÷��̾ ����Ʈ�� ������ ���°� �ƴϸ� ����Ʈ ��ǳ�� Ȱ��ȭ
            else if(gm.Accept && !gm.Success) ani.SetTrigger("wait"); // �÷��̾ ����Ʈ�� ���������� �������� �ʾ����� �������� ��Ÿ���� �ִϸ��̼� ����
            else if(gm.Success) ani.SetTrigger("success"); // �÷��̾ ����Ʈ ���� �� ���� ���¸� ��Ÿ���� �ִϸ��̼� ����
        }
    }

    /* Method : DialogueReady
     * Description : �÷��̾���� ��ȭ�� �غ��ϴ� ������ �����ϴ� �޼����Դϴ�. ���� diaIdx�� 0�̸� �� ��ȭ ���� �����̶�� �ǹ��̱� ������ SetDiaState �޼���� ���� ���� ��ȭ ���¸� �����ϰ�
     * �� ��ȭ ���¸� �������� GetDiaData �޼��带 ���� ��ȭ �����͸� �����ɴϴ�. DialogueReady �ѹ� ȣ��� ������ ���� ������ ��簡 gm���� ���޵Ǹ� ���޵ǰ� ���� diaIdx�� 1 �����ϵ��� �����߽��ϴ�.
     * ����Ʈ ��û ������ ������ ��簡 ��µǸ� ����Ʈ ���� �Ǵ� ���� ��ư�� �ߵ��� �����ߴµ� �� �� �÷��̾��� ���� �Ǵ� ���� �Է��� ������ ������ ��ȭ�� ���߰� ��ٸ��� ������ �ϵ��� ���� Ŭ���� npc��
     * Wait�� ���� Wait�� true�� ���¸� �ٷ� �޼��带 ������������ �����߽��ϴ�. �� ����Ʈ ��û ���¿��� ���� ���·� ����Ʈ ���� �Ǵ� ������ �ݵ�� �Ѿ�Ƿ� diaIdx�� 0���� �缳���ؼ� �÷��̾��� �Է¿� ����
     * ��ȭ ������ ��簡 �� ���޵ǵ��� �����߽��ϴ�. ��ȭ ���� ������ diaIdx�� ���� ��ȭ ������ ��� ���� �Ѿ ���� �Ǵ��ؼ� �� ��� gm���� ��ȭ �����϶�� �ǹ̷� FinishTalk�� true�� �����ϰ� diaIdx�� 
     * 0���� �ʱ�ȭ�ϵ��� �����߽��ϴ�. �׸��� ���� �ۼ��� ����Ʈ �� npc�� ��ȭ�ϱ� ������ ���� ����Ʈ�� �־ ���� npc�� ��ȭ ���°� ����Ʈ ���� ���¸� gm�� QuestUpdate �޼��忡 "NPC��ȭ" �޽����� npc 
     * ���̵� �������� �����߽��ϴ�. ���� ��ȭ ���� �������� ����Ʈ ���� ��ȭ ���� ������ ��ȭ ���¸� �⺻������ �ʱ�ȭ�Ϸ��� SetDiaState�� ȣ���߰� ���� Ȱ��ȭ�� ��ǳ���� ��Ȱ��ȭ�ϰ� gm�� ����Ʈ 
     * �Ϸ� ���� ������ �����ϴ� �޼��带 ȣ���ϵ��� �����߽��ϴ�.
     * Return Value : bool
     */
    public override bool DialogueReady()
    {
        if (Wait) return false;
        if (diaIdx == 0)
        {
            SetDiaState(); 
            GetDiaData();
            gm.ChangeTalkNpc(NpcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count) 
        {
            gm.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count) 
            {
                Wait = true;
                gm.OpenYesOrNoButton();
                diaIdx = 0;
            }
        }
        else 
        {
            gm.FinishTalk = true;
            diaIdx = 0;
            if(ds == DialogueState.questReply) gm.QuestUpdate("NPC��ȭ", NpcId);
            if (ds == DialogueState.questSuccess)
            {
                SetDiaState();
                questBallon.SetActive(false);
                gm.QuestDone();
            }
        }
        return true;
    }

    /* Method : SetDiaState
     * Description : �÷��̾�� � ��縦 ���ؾ� �ϴ��� �� ��ȭ ���¸� �����ϴ� �޼����Դϴ�. �� ��Ȳ���� ds�� �˸��� DialogueState ������ ���õǵ��� �����߽��ϴ�.
     * Return Value : void
     */
    public override void SetDiaState()
    {
        if (gm.CurQuestNpcId == NpcId) 
        {
            if (ds == DialogueState.normal || ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!gm.Accept) ds = DialogueState.questRefuse; 
                else ds = DialogueState.questAccept;
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
        else if(gm.CurQuestNpcId != 0 && gm.Accept)
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
    }

    /* Method : GetDiaData
     * Description : ���� ��ȭ ���¸� �������� dm���Լ� �÷��̾�� ���� ��� �����͸� �����ɴϴ�. ���������� � ��Ȳ���� �ľ��ϱ� ���� �Ϸ��� switch-case���� ����߽��ϴ�.
     * Return Value : void
     */
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
                    if(gm.CurQuestNum == dm.QuestReplyDiaData[i].Item1) 
                    {
                        for(int j = 0; j < dm.QuestReplyDiaData[i].Item2.Count; j++)
                        {
                            if (NpcId == dm.QuestReplyDiaData[i].Item2[j].NpcId)
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
        if(Other.gameObject.tag == "Player") // �÷��̾ npc�� ������ ���
        {
            if (gm.npcTalk != null) gm.npcTalk = null; // delegate �ʱ�ȭ
            if (gm.unLockWait != null) gm.unLockWait = null; // delegate �ʱ�ȭ
            gm.npcTalk += DialogueReady; // delegate�� �޼��� �Ҵ�
            gm.unLockWait += UnLockWait; // delegate�� �޼��� �Ҵ�
        }
    }
}
