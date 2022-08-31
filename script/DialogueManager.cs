using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //public List<DialogueData> DiaData = new List<DialogueData>();
    //public List<DialogueData> QuestAskDiaData = new List<DialogueData>();
    //public List<DialogueData> QuestSuccessDiaData = new List<DialogueData>();

    List<DialogueData> diaData = new List<DialogueData>();
    List<DialogueData> questAskDiaData = new List<DialogueData>();
    List<DialogueData> questSuccessDiaData = new List<DialogueData>();

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    int checkfirst = 0;
    int npcnum = 0;
    int questnum = 0;

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
            if (checkfirst == 0)
            {
                checkfirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkfirst == 1) //첫번째 npc의 첫번째 대사를 읽고 있는지 체크
            {
                checkfirst++;
                npcnum++;
                DialogueData d = new DialogueData();
                d.npcid = Convert.ToInt32(SplitLine[0]);
                d.name = SplitLine[1];
                SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                d.Dialogue.Add(SplitLine[2]);
                DiaData.Add(d);
            }
            else
            {
                if (Convert.ToString(DiaData[npcnum - 1].npcid) == SplitLine[0])
                {
                    SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                    SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                    DiaData[npcnum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    npcnum++;
                    DialogueData d = new DialogueData();
                    d.npcid = Convert.ToInt32(SplitLine[0]);
                    d.name = SplitLine[1];
                    SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                    SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                    d.Dialogue.Add(SplitLine[2]);
                    DiaData.Add(d);
                }
            }

        }

        textset = Resources.Load<TextAsset>("QuestScript_ask");
        Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        checkfirst = 0;

        foreach (string line in Lines)
        {
            if (checkfirst == 0)
            {
                checkfirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkfirst == 1)
            {
                checkfirst++;
                questnum++;
                DialogueData d = new DialogueData();
                d.name = SplitLine[1];
                d.Dialogue.Add(SplitLine[2]);
                QuestAskDiaData.Add(d);
            }
            else
            {
                if (Convert.ToString(questnum) == SplitLine[0])
                {
                    QuestAskDiaData[questnum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    questnum++;
                    DialogueData d = new DialogueData();
                    d.name = SplitLine[1];
                    d.Dialogue.Add(SplitLine[2]);
                    QuestAskDiaData.Add(d);
                }
            }

        }

        questnum = 0;
        checkfirst = 0;
        textset = Resources.Load<TextAsset>("QuestScript_Success");
        Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in Lines)
        {
            if (checkfirst == 0)
            {
                checkfirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkfirst == 1)
            {
                checkfirst++;
                questnum++;
                DialogueData d = new DialogueData();
                d.name = SplitLine[1];
                d.Dialogue.Add(SplitLine[2]);
                QuestSuccessDiaData.Add(d);
            }
            else
            {
                if (Convert.ToString(questnum) == SplitLine[0])
                {
                    QuestSuccessDiaData[questnum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    questnum++;
                    DialogueData d = new DialogueData();
                    d.name = SplitLine[1];
                    d.Dialogue.Add(SplitLine[2]);
                    QuestSuccessDiaData.Add(d);
                }
            }

        }
    }

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
