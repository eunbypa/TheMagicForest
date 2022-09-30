using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : QuestData
 * Description : ���� ����Ʈ �����͸� ����ϴ� Ŭ�����Դϴ�.
 */
public class QuestData
{
    int npcId; // ����Ʈ�� �� npc ���̵�
    string title; // ����Ʈ ����
    string npcName; // ����Ʈ�� �� npc �̸�
    string info; // ����Ʈ ����

    List<string> type = new List<string>(); // ����Ʈ �䱸������ Ÿ��(ex : ���� óġ)
    List<string> rewardType = new List<string>(); // ����Ʈ ���� �� �޴� ������ Ÿ��
    List<string> req_Name = new List<string>(); // ����Ʈ �䱸���׿� �ش��� �� �̸�(ex : ���� �̸�)
    List<int> req_Id = new List<int>(); // ����Ʈ �䱸���׿� �ش��� ���� �������� �ĺ��ϴ� id(ex : ���� A�� id�� 1)
    List<int> req_Num = new List<int>(); // ����Ʈ �䱸���׿� �ش��� ���� �󸶳� �����ؾ� �ϴ���(ex : ���� 5���� óġ)
    List<int> reward = new List<int>(); // ������ �󸶳� �޴���(ex : ����ġ 100)

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
