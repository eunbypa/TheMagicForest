using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //public List<DialogueData> DiaData = new List<DialogueData>();
    //public List<DialogueData> QuestAskDiaData = new List<DialogueData>();
    //public List<DialogueData> QuestSuccessDiaData = new List<DialogueData>();

    List<DialogueData> diaData = new List<DialogueData>(); // �⺻ ���
    List<DialogueData> questAskDiaData = new List<DialogueData>(); // �÷��̾�� ����Ʈ ��û �� ���
    List<DialogueData> questAcceptDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���
    List<DialogueData> questRefuseDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���
    List<DialogueData> questDoingDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� �϶� ���
    List<DialogueData> questSuccessDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    int checkFirst = 0;
    int npcNum = 0;
    int questNum = 0;

    void Awake()
    {
        ReadDiaData();
    }

    void ReadDiaData()
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

    public List<DialogueData> QuestSuccessDiaData
    {
        get
        {
            return questSuccessDiaData;
        }
    }
}
