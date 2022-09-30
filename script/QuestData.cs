using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : QuestData
 * Description : 게임 퀘스트 데이터를 담당하는 클래스입니다.
 */
public class QuestData
{
    int npcId; // 퀘스트를 줄 npc 아이디
    string title; // 퀘스트 제목
    string npcName; // 퀘스트를 줄 npc 이름
    string info; // 퀘스트 정보

    List<string> type = new List<string>(); // 퀘스트 요구사항의 타입(ex : 몬스터 처치)
    List<string> rewardType = new List<string>(); // 퀘스트 성공 시 받는 보상의 타입
    List<string> req_Name = new List<string>(); // 퀘스트 요구사항에 해당한 것 이름(ex : 몬스터 이름)
    List<int> req_Id = new List<int>(); // 퀘스트 요구사항에 해당한 것이 무엇인지 식별하는 id(ex : 몬스터 A의 id는 1)
    List<int> req_Num = new List<int>(); // 퀘스트 요구사항에 해당한 것을 얼마나 수행해야 하는지(ex : 몬스터 5마리 처치)
    List<int> reward = new List<int>(); // 보상을 얼마나 받는지(ex : 경험치 100)

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
    public List<int> Reward
    {
        get
        {
            return reward;
        }
        set
        {
            reward = value;
        }
    }
    /* Property */
    public List<int> Req_Id
    {
        get
        {
            return req_Id;
        }
        set
        {
            req_Id = value;
        }
    }
    /* Property */
    public List<int> Req_Num
    {
        get
        {
            return req_Num;
        }
        set
        {
            req_Num = value;
        }
    }
    /* Property */
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
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
    public List<string> Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }
    /* Property */
    public string Info
    {
        get
        {
            return info;
        }
        set
        {
            info = value;
        }
    }
    /* Property */
    public List<string> RewardType
    {
        get
        {
            return rewardType;
        }
        set
        {
            rewardType = value;
        }
    }
    /* Property */
    public List<string> Req_Name
    {
        get
        {
            return req_Name;
        }
        set
        {
            req_Name = value;
        }
    }
}
