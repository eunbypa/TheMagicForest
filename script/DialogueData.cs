using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : DialogueData
 * Description : npc의 대화 데이터를 담당하는 클래스입니다.
 */
public class DialogueData
{
    int npcId; // npc를 식별하는 아이디
    string npcName; // npc의 이름
    List<string> dialogue = new List<string>(); // 대화 시 대사 데이터 순서대로 저장된 리스트, index : 대화 순서

    /* Property */
    public int NpcId
    {
        get
        {
            return npcId;
        }
        set
        {
            npcId = value;
        }
    }
    /* Property */
    public string NpcName
    {
        get
        {
            return npcName;
        }
        set
        {
            npcName = value;
        }
    }
    /* Property */
    public List<string> Dialogue
    {
        get
        {
            return dialogue;
        }
        set
        {
            dialogue = value;
        }
    }

}
