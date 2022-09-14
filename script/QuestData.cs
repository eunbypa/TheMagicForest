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
    string title;
    string npcName;
    string info;
    List<string> type = new List<string>(); // ����Ʈ �䱸������ Ÿ��(ex : ���� óġ)
    List<string> rewardType = new List<string>(); // ����Ʈ ���� �� �޴� ������ Ÿ��
    List<string> req_Name = new List<string>(); // ����Ʈ �䱸���׿� �ش��� �� �̸�(ex : ���� �̸�)
    int npcId;
    List<int> req_Id = new List<int>(); // ����Ʈ �䱸���׿� �ش��� ���� �������� �ĺ��ϴ� id(ex : ���� A�� id�� 1)
    List<int> req_Num = new List<int>(); // ����Ʈ �䱸���׿� �ش��� ���� �󸶳� �����ؾ� �ϴ���(ex : ���� 5���� óġ)
    List<int> reward = new List<int>(); // ������ �󸶳� �޴���
    

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
