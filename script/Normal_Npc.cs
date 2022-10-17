using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Normal_Npc
 * Description : 일반 npc를 담당하는 클래스입니다. 일반 npc는 플레이어와 평범하게 대화를 하거나 퀘스트를 줄 수 있습니다. Npc 클래스를 상속받습니다.
 */
public class Normal_Npc : Npc
{
    // npc의 대화 상태를 직관적으로 표현하기 위해 enum을 사용했습니다.
    // normal : 평상시, questAsk : 퀘스트 요청, questRefuse : 퀘스트 거부, qusetAccept : 퀘스트 수락, questDoing : 퀘스트 진행중, questSuccess : 퀘스트 성공, questReply : 퀘스트 응답
    enum DialogueState
    {
        normal, questAsk, questRefuse, questAccept, questDoing, questSuccess, questReply
    };
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    //[SerializeField] private GameObject gM; // 게임 관리자 GameManager
    [SerializeField] private GameObject dM; // 대화 데이터 관리자 DialogueManager
    [SerializeField] private GameObject qM; // 퀘스트 데이터 관리자 QuestManager
    [SerializeField] private GameObject questBallon; // 퀘스트 활성화 시 npc 위에 뜨는 말풍선

    int diaIdx = 0; // 다음 순서에 말할 대사 데이터 위치
    DialogueState ds; // 대화 상태
    //GameManager gm; // 게임 관리자 GameManager 클래스 객체
    DialogueManager dm; // 대화 데이터 관리자 DialogueManager 클래스 객체
    QuestManager qm; // 퀘스트 데이터 관리자 QuestManager 클래스 객체
    Animator ani; // 유니티 애니메이션 컴포넌트
    DialogueData d; // npc가 현재 말해야 할 대사 데이터 저장

    void Start()
    {
        //gm = gM.GetComponent<GameManager>(); // gM GameObject 객체에 할당된 GameManager 클래스 컴포넌트를 가져옵니다.
        dm = dM.GetComponent<DialogueManager>(); // dM GameObject 객체에 할당된 DialogueManager 클래스 컴포넌트를 가져옵니다.
        qm = qM.GetComponent<QuestManager>(); // qM GameObject 객체에 할당된 QuestManager 클래스 컴포넌트를 가져옵니다.
        ds = DialogueState.normal; // npc의 대화 상태를 기본값으로 초기화합니다.
        ani = questBallon.GetComponent<Animator>(); // questBallon GameObject 객체에서 Animator 컴포넌트를 가져옵니다.
    }

    void Update()
    {
        if (GameManager.instance.CurQuestNpcId == NpcId) // 현재 플레이어가 진행 가능한 퀘스트를 가진 npc인 경우
        {
            if (!GameManager.instance.Accept) questBallon.SetActive(true); // 아직 플레이어가 퀘스트를 수락한 상태가 아니면 퀘스트 말풍선 활성화
            else if (GameManager.instance.Accept && !GameManager.instance.Success) ani.SetTrigger("wait"); // 플레이어가 퀘스트를 수락했지만 성공하지 않았으면 진행중을 나타내는 애니메이션 실행
            else if (GameManager.instance.Success) ani.SetTrigger("success"); // 플레이어가 퀘스트 성공 시 성공 상태를 나타내는 애니메이션 실행
        }
    }

    /* Method : DialogueReady
     * Description : 플레이어와의 대화를 준비하는 동작을 수행하는 메서드입니다. 현재 diaIdx가 0이면 새 대화 시작 시점이라는 의미이기 때문에 SetDiaState 메서드로 현재 말할 대화 상태를 설정하고
     * 그 대화 상태를 기준으로 GetDiaData 메서드를 통해 대화 데이터를 가져옵니다. DialogueReady 한번 호출될 때마다 현재 순서의 대사가 gm에게 전달되며 전달되고 나면 diaIdx를 1 증가하도록 구현했습니다.
     * 퀘스트 요청 상태의 마지막 대사가 출력되면 퀘스트 수락 또는 거절 버튼이 뜨도록 구현했는데 이 때 플레이어의 수락 또는 거절 입력이 들어오기 전까지 대화를 멈추고 기다리는 동작을 하도록 상위 클래스 npc에
     * Wait를 통해 Wait가 true인 상태면 바로 메서드를 빠져나오도록 구현했습니다. 또 퀘스트 요청 상태에서 다음 상태로 퀘스트 수락 또는 거절로 반드시 넘어가므로 diaIdx를 0으로 재설정해서 플레이어의 입력에 따른
     * 대화 상태의 대사가 잘 전달되도록 구현했습니다. 대화 종료 시점은 diaIdx가 현재 대화 상태의 대사 수를 넘어간 경우로 판단해서 이 경우 gm에게 대화 종료하라는 의미로 FinishTalk를 true로 설정하고 diaIdx는 
     * 0으로 초기화하도록 구현했습니다. 그리고 제가 작성한 퀘스트 중 npc와 대화하기 조건을 가진 퀘스트가 있어서 현재 npc의 대화 상태가 퀘스트 응답 상태면 gm의 QuestUpdate 메서드에 "NPC대화" 메시지와 npc 
     * 아이디를 보내도록 구현했습니다. 또한 대화 종료 시점에서 퀘스트 성공 대화 상태 였으면 대화 상태를 기본값으로 초기화하려고 SetDiaState를 호출했고 현재 활성화된 말풍선을 비활성화하고 gm의 퀘스트 
     * 완료 관련 동작을 수행하는 메서드를 호출하도록 구현했습니다.
     * Return Value : bool
     */
    public override bool DialogueReady()
    {
        if (Wait) return false;
        if (diaIdx == 0)
        {
            SetDiaState();
            GetDiaData();
            GameManager.instance.ChangeTalkNpc(NpcId, d.NpcName);
        }
        if (diaIdx < d.Dialogue.Count)
        {
            GameManager.instance.TextData = d.Dialogue[diaIdx].ToCharArray();
            diaIdx++;
            if (ds == DialogueState.questAsk && diaIdx == d.Dialogue.Count)
            {
                Wait = true;
                GameManager.instance.OpenYesOrNoButton();
                diaIdx = 0;
            }
        }
        else
        {
            GameManager.instance.FinishTalk = true;
            diaIdx = 0;
            if (ds == DialogueState.questReply) GameManager.instance.QuestUpdate("NPC대화", NpcId);
            if (ds == DialogueState.questSuccess)
            {
                SetDiaState();
                questBallon.SetActive(false);
                GameManager.instance.QuestDone();
            }
        }
        return true;
    }

    /* Method : SetDiaState
     * Description : 플레이어에게 어떤 대사를 말해야 하는지 그 대화 상태를 설정하는 메서드입니다. 각 상황에서 ds에 알맞은 DialogueState 변수가 세팅되도록 구현했습니다.
     * Return Value : void
     */
    public override void SetDiaState()
    {
        if (GameManager.instance.CurQuestNpcId == NpcId)
        {
            if (ds == DialogueState.normal || ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (ds == DialogueState.questAsk)
            {
                if (!GameManager.instance.Accept) ds = DialogueState.questRefuse;
                else ds = DialogueState.questAccept;
            }
            else if (ds == DialogueState.questRefuse) ds = DialogueState.questAsk;
            else if (GameManager.instance.Accept && !GameManager.instance.Success) ds = DialogueState.questDoing;
            else if (ds != DialogueState.questSuccess && GameManager.instance.Success)
            {
                ds = DialogueState.questSuccess;
            }
            else if (ds == DialogueState.questSuccess)
            {
                ds = DialogueState.normal;
            }
        }
        else if (GameManager.instance.CurQuestNpcId != 0 && GameManager.instance.Accept)
        {
            for (int i = 0; i < qm.QuestDataList[GameManager.instance.CurQuestNum - 1].Type.Count; i++)
            {
                if (qm.QuestDataList[GameManager.instance.CurQuestNum - 1].Type[i] == "NPC대화" && qm.QuestDataList[GameManager.instance.CurQuestNum - 1].Req_Id[i] == NpcId)
                {
                    ds = DialogueState.questReply;
                }
            }
        }
        else ds = DialogueState.normal;
    }

    /* Method : GetDiaData
     * Description : 현재 대화 상태를 기준으로 dm에게서 플레이어에게 말할 대사 데이터를 가져옵니다. 직관적으로 어떤 상황인지 파악하기 쉽게 하려고 switch-case문을 사용했습니다.
     * Return Value : void
     */
    public override void GetDiaData()
    {
        switch (ds)
        {
            case DialogueState.normal:
                d = dm.DiaData[NpcId - 1];
                break;
            case DialogueState.questAsk:
                d = dm.QuestAskDiaData[GameManager.instance.CurQuestNum - 1];
                break;
            case DialogueState.questAccept:
                d = dm.QuestAcceptDiaData[GameManager.instance.CurQuestNum - 1];
                break;
            case DialogueState.questRefuse:
                d = dm.QuestRefuseDiaData[GameManager.instance.CurQuestNum - 1];
                break;
            case DialogueState.questDoing:
                d = dm.QuestDoingDiaData[GameManager.instance.CurQuestNum - 1];
                break;
            case DialogueState.questSuccess:
                d = dm.QuestSuccessDiaData[GameManager.instance.CurQuestNum - 1];
                break;
            case DialogueState.questReply:
                for (int i = 0; i < dm.QuestReplyDiaData.Count; i++)
                {
                    if (GameManager.instance.CurQuestNum == dm.QuestReplyDiaData[i].Item1)
                    {
                        for (int j = 0; j < dm.QuestReplyDiaData[i].Item2.Count; j++)
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
        if (Other.gameObject.tag == "Player") // 플레이어가 npc와 접촉한 경우
        {
            if (GameManager.instance.npcTalk != null) GameManager.instance.npcTalk = null; // delegate 초기화
            if (GameManager.instance.unLockWait != null) GameManager.instance.unLockWait = null; // delegate 초기화
            GameManager.instance.npcTalk += DialogueReady; // delegate에 메서드 할당
            GameManager.instance.unLockWait += UnLockWait; // delegate에 메서드 할당
        }
    }
}
