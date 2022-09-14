using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<QuestData> questDataList = new List<QuestData>(); // 각 NPC에게 해당 퀘스트 리스트 전달하는 방식으로 구현예정, 즉 public 수정요망

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    //int checkFirst = 0;
    //int curNum = 0; // 현재 읽고 있는 데이터가 몇번째 집합체의 데이터인지 그 정보를 저장함

    void Awake()
    {
        ReadData("QuestList");
        ReadData("Quest_Requirement");
        ReadData("Quest_Reward");
    }

    void ReadData(string s)
    {
        int i = 0;
        int curNum = 0;
        int checkFirst = 0;
        TextAsset textset = Resources.Load<TextAsset>(s);
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        string[] SplitLine;
        foreach (string line in Lines)
        {
            if (checkFirst == 0)
            {
                checkFirst++;
                //curNum++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            //int nextNum = SplitLine[0];
            if (Convert.ToString(curNum) != SplitLine[0])
            {
                if (questDataList.Count <= curNum) // 이번에 담을 데이터에 대한 객체 리스트에 추가
                {
                    QuestData qdata = new QuestData();
                    questDataList.Add(qdata);
                }
                curNum++;
                i++;
                if (i == 3) return;
            }
            //QuestData qdata = new QuestData();
            if(s == "QuestList")
            {
                questDataList[curNum - 1].Title = SplitLine[1];
                questDataList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[2]);
                questDataList[curNum - 1].NpcName = SplitLine[3];
                questDataList[curNum - 1].Info = SplitLine[4];
            }
            if(s == "Quest_Requirement")
            {
                questDataList[curNum - 1].Type.Add(SplitLine[1]);
                if(SplitLine[2] != "없음")questDataList[curNum - 1].Req_Id.Add(Convert.ToInt32(SplitLine[2]));
                questDataList[curNum - 1].Req_Name.Add(SplitLine[3]);
                Debug.Log(SplitLine[3]);
                questDataList[curNum - 1].Req_Num.Add(Convert.ToInt32(SplitLine[4]));
            }
            if(s == "Quest_Reward")
            {
                questDataList[curNum - 1].RewardType.Add(SplitLine[1]);
                questDataList[curNum - 1].Reward.Add(Convert.ToInt32(SplitLine[2]));
            }
        }
    }

    public List<QuestData> QuestDataList {
        get
        {
            return questDataList;
        }
    }
}
