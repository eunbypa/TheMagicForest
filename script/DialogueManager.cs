using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //public List<DialogueData> DiaData = new List<DialogueData>();
    //public List<DialogueData> QuestAskDiaData = new List<DialogueData>();
    //public List<DialogueData> QuestSuccessDiaData = new List<DialogueData>();

    List<DialogueData> diaData = new List<DialogueData>(); // npc�� �⺻ ���, index : npcId
    List<DialogueData> questAskDiaData = new List<DialogueData>(); // �÷��̾�� ����Ʈ ��û �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questAcceptDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questRefuseDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questDoingDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� �϶� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questSuccessDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questReplyDiaData = new List<DialogueData>(); // ���� ����Ʈ �䱸���� Ÿ���� npc�� ��ȭ�� �� �ű⿡ �ش��ϴ� npc�� �� ���

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    int checkFirst = 0;
    int curNum = 0; // ���� �а� �ִ� �����Ͱ� ���° ����ü�� ���������� �� ������ ������
    //TextAsset textSet;

    void Awake()
    {
        ReadDiaData("NpcScript", ref diaData);
        ReadDiaData("QuestScript_Ask", ref questAskDiaData);
        ReadDiaData("QuestScript_Accept", ref questAcceptDiaData);
        ReadDiaData("QuestScript_Refuse", ref questRefuseDiaData);
        ReadDiaData("QuestScript_Doing", ref questDoingDiaData);
        ReadDiaData("QuestScript_Success", ref questSuccessDiaData);
    }

    void ReadDiaData(string s, ref List<DialogueData> dList) // �� ù��° column�� �����͸� ������ ������ �ȴ�.
    {
        TextAsset textset = Resources.Load<TextAsset>(s);
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        string[] SplitLine;
        foreach (string line in Lines)
        {
            if (checkFirst == 0) // ù���� �����ϴ� �뵵
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            int nextNum = Convert.ToInt32(SplitLine[0]);
            if (curNum != nextNum)
            {
                curNum++;
                DialogueData d = new DialogueData();
                dList.Add(d);
            }
            if (s == "NpcScript") {
                dList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[0]);
                dList[curNum - 1].NpcName = SplitLine[1];
                SplitLine[2] = SplitLine[2].Replace("�г���", "�κ�"); // ���߿� Ÿ��Ʋ ������ �Է¹��� �г��� ������ �����ϴ� �ڵ�� �ٲ�� ��. ������ ���ÿ�
                SplitLine[2] = SplitLine[2].Replace("��ǥ", ",");
                dList[curNum - 1].Dialogue.Add(SplitLine[2]);
            }
            else {
                dList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[1]);
                dList[curNum - 1].NpcName = SplitLine[2];
                SplitLine[3] = SplitLine[3].Replace("�г���", "�κ�"); // ���߿� Ÿ��Ʋ ������ �Է¹��� �г��� ������ �����ϴ� �ڵ�� �ٲ�� ��. ������ ���ÿ�
                SplitLine[3] = SplitLine[3].Replace("��ǥ", ",");
                dList[curNum - 1].Dialogue.Add(SplitLine[3]);
            }
        }
        curNum = 0;
        checkFirst = 0;
    }
    /*void ReadDiaData()
    {
        TextAsset textset = Resources.Load<TextAsset>("NpcScript");
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        string[] SplitLine;
        foreach (string line in Lines)
        {
            if (checkFirst == 0) // ù���� �����ϴ� �뵵
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkFirst == 1) //ù��° npc�� ù��° ��縦 �а� �ִ��� üũ
            {
                checkFirst++;
                npcNum++;
                DialogueData d = new DialogueData();
                d.NpcId = Convert.ToInt32(SplitLine[0]);
                d.NpcName = SplitLine[1];
                SplitLine[2] = SplitLine[2].Replace("�г���", "�κ�");
                SplitLine[2] = SplitLine[2].Replace("��ǥ", ",");
                d.Dialogue.Add(SplitLine[2]);
                diaData.Add(d);
            }
            else
            {
                if (Convert.ToString(DiaData[npcNum - 1].NpcId) == SplitLine[0])
                {
                    SplitLine[2] = SplitLine[2].Replace("�г���", "�κ�");
                    SplitLine[2] = SplitLine[2].Replace("��ǥ", ",");
                    diaData[npcNum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    npcNum++;
                    DialogueData d = new DialogueData();
                    d.NpcId = Convert.ToInt32(SplitLine[0]);
                    d.NpcName = SplitLine[1];
                    SplitLine[2] = SplitLine[2].Replace("�г���", "�κ�");
                    SplitLine[2] = SplitLine[2].Replace("��ǥ", ",");
                    d.Dialogue.Add(SplitLine[2]);
                    diaData.Add(d);
                }
            }

        }

        textset = Resources.Load<TextAsset>("QuestScript_ask");
        Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        checkFirst = 0;

        foreach (string line in Lines)
        {
            if (checkFirst == 0)
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkFirst == 1)
            {
                checkFirst++;
                questNum++;
                DialogueData d = new DialogueData();
                d.NpcName = SplitLine[1];
                d.Dialogue.Add(SplitLine[2]);
                questAskDiaData.Add(d);
            }
            else
            {
                if (Convert.ToString(questNum) == SplitLine[0])
                {
                    QuestAskDiaData[questNum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    questNum++;
                    DialogueData d = new DialogueData();
                    d.NpcName = SplitLine[1];
                    d.Dialogue.Add(SplitLine[2]);
                    questAskDiaData.Add(d);
                }
            }

        }

        questNum = 0;
        checkFirst = 0;
        textset = Resources.Load<TextAsset>("QuestScript_Success");
        Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in Lines)
        {
            if (checkFirst == 0)
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkFirst == 1)
            {
                checkFirst++;
                questNum++;
                DialogueData d = new DialogueData();
                d.NpcName = SplitLine[1];
                d.Dialogue.Add(SplitLine[2]);
                questSuccessDiaData.Add(d);
            }
            else
            {
                if (Convert.ToString(questNum) == SplitLine[0])
                {
                    questSuccessDiaData[questNum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    questNum++;
                    DialogueData d = new DialogueData();
                    d.NpcName = SplitLine[1];
                    d.Dialogue.Add(SplitLine[2]);
                    questSuccessDiaData.Add(d);
                }
            }

        }
    }*/

    public List<DialogueData> DiaData
    {
        get
        {
            return diaData;
        }
    }

    public List<DialogueData> QuestAskDiaData
    {
        get
        {
            return questAskDiaData;
        }
    }

    public List<DialogueData> QuestAcceptDiaData
    {
        get
        {
            return questAcceptDiaData;
        }
    }
    public List<DialogueData> QuestRefuseDiaData
    {
        get
        {
            return questRefuseDiaData;
        }
    }
    public List<DialogueData> QuestDoingDiaData
    {
        get
        {
            return questDoingDiaData;
        }
    }
    public List<DialogueData> QuestSuccessDiaData
    {
        get
        {
            return questSuccessDiaData;
        }
    }
    public List<DialogueData> QuestReplyDiaData
    {
        get
        {
            return questReplyDiaData;
        }
    }
}
