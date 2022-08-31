using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> questData = new List<QuestData>(); // 각 NPC에게 해당 퀘스트 리스트 전달하는 방식으로 구현예정, 즉 public 수정요망

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
            qdata.title = SplitLine[1];
            qdata.npcid = Convert.ToInt32(SplitLine[2]);
            qdata.npcname = SplitLine[3];
            qdata.type = SplitLine[4];
            qdata.info = SplitLine[5];
            qdata.requirement = Convert.ToInt32(SplitLine[6]);
            qdata.rewardtype = SplitLine[7];
            qdata.reward = Convert.ToInt32(SplitLine[8]);
            questData.Add(qdata);
        }
    }
}
