using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public GameObject GM;

    //public int npcid;
    GameManager gm;
    DialogueData d; // npc�� �Ϲ� ��� ������ ����
    int npcId;
    int idx = 0; // �������� ���� ��� ������ ��ġ ����

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
        if (idx < d.Dialogue.Count) // ���� ����� ��簡 ������
        {
            gm.TextData = d.Dialogue[idx].ToCharArray();
            idx++;
        }
        else // ��� ������ �� ��� �Ϸ��� ���
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
