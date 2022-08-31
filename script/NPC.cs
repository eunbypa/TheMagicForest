using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public GameObject GM;

    //public int npcid;
    GameManager gm;
    DialogueData d; // npc의 일반 대사 데이터 저장
    int npcId;
    int idx = 0; // 다음으로 보낼 대사 데이터 위치 정보

    void Start()
    {
        gm = GM.GetComponent<GameManager>();
        d = GameObject.Find("DialogueManager").GetComponent<DialogueManager>().DiaData[npcid - 1];
    }

    public int NpcId
    {
        get
        {
            return npcId;
        }
    }
   
    public void DialogueReady()
    {
        //gm.ChangeTalkNpc(npcid);
        if (idx < d.Dialogue.Count) // 아직 출력할 대사가 존재함
        {
            gm.TextData = d.Dialogue[idx].ToCharArray();
            idx++;
        }
        else // 대사 끝까지 다 출력 완료한 경우
        {
            gm.FinishTalk = true;
            idx = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "Player")
        {
            //DialogueReady();
            gm.ChangeTalkNpc(npcId);
            if (gm.npcTalk != null) gm.npcTalk = null;
            gm.npcTalk += DialogueReady;
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
