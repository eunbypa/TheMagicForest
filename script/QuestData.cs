using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    /*public string title;
    public int npcid;
    public string npcname;
    public string type;
    public string info;
    public int requirement;
    public string rewardtype;
    public int reward;*/
    int npcId;
    int reward;
    string title;
    string npcName;
    string type;
    string info;
    string rewardType;
    
    public int NpcId
    {
        get
        {
            return npcId;
        }
    }

    public int Reward
    {
        get
        {
            return reward;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }
    }

    public string NpcName
    {
        get
        {
            return npcName;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }
    }

    public string Info
    {
        get
        {
            return info;
        }
    }

    public string RewardType
    {
        get
        {
            return rewardType;
        }
    }
}
