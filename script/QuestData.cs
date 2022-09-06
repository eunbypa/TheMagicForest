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
    int requirement;
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
        set
        {
            npcId = value;
        }
    }

    public int Reward
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

    public int Requirement
    {
        get
        {
            return requirement;
        }
        set
        {
            requirement = value;
        }
    }

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

    public string Type
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

    public string RewardType
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
}
