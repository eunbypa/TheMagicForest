using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> questData = new List<QuestData>(); // �� NPC���� �ش� ����Ʈ ����Ʈ �����ϴ� ������� ��������, �� public �������

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    int checkfirst = 0;

    void Start()
    {
        //ReadData();
    }

    void ReadData()
    {
        TextAsset textset = Resources.Load<TextAsset>("QuestList");
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
            QuestData qdata = new QuestData();
            qdata.Title = SplitLine[1];
            qdata.NpcId = Convert.ToInt32(SplitLine[2]);
            qdata.NpcName = SplitLine[3];
            qdata.Type = SplitLine[4];
            qdata.Info = SplitLine[5];
            qdata.Requirement = Convert.ToInt32(SplitLine[6]);
            qdata.RewardType = SplitLine[7];
            qdata.Reward = Convert.ToInt32(SplitLine[8]);
            questData.Add(qdata);
        }
    }
}
