using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //public List<DialogueData> DiaData = new List<DialogueData>();
    //public List<DialogueData> QuestAskDiaData = new List<DialogueData>();
    //public List<DialogueData> QuestSuccessDiaData = new List<DialogueData>();

    List<DialogueData> diaData = new List<DialogueData>(); // npc의 기본 대사, index : npcId
    List<DialogueData> questAskDiaData = new List<DialogueData>(); // 플레이어에게 퀘스트 요청 시 대사, index : 퀘스트 번호
    List<DialogueData> questAcceptDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 수락 시 대사, index : 퀘스트 번호
    List<DialogueData> questRefuseDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 거절 시 대사, index : 퀘스트 번호
    List<DialogueData> questDoingDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 진행 중 일때 대사, index : 퀘스트 번호
    List<DialogueData> questSuccessDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 성공 시 대사, index : 퀘스트 번호
    List<DialogueData> questReplyDiaData = new List<DialogueData>(); // 현재 퀘스트 요구사항 타입이 npc와 대화일 때 거기에 해당하는 npc일 때 대사

    char[] LineSeperate = new char[] { '\n' };
    char[] CSVSeperate = new char[] { ',' };

    int checkFirst = 0;
    int curNum = 0; // 현재 읽고 있는 데이터가 몇번째 집합체의 데이터인지 그 정보를 저장함
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

    void ReadDiaData(string s, ref List<DialogueData> dList) // 맨 첫번째 column이 데이터를 나누는 기준이 된다.
    {
        TextAsset textset = Resources.Load<TextAsset>(s);
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        string[] SplitLine;
        foreach (string line in Lines)
        {
            if (checkFirst == 0) // 첫줄은 생략하는 용도
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
                SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈"); // 나중에 타이틀 씬에서 입력받은 닉네임 정보를 저장하는 코드로 바꿔야 함. 지금은 예시용
                SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                dList[curNum - 1].Dialogue.Add(SplitLine[2]);
            }
            else {
                dList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[1]);
                dList[curNum - 1].NpcName = SplitLine[2];
                SplitLine[3] = SplitLine[3].Replace("닉네임", "로빈"); // 나중에 타이틀 씬에서 입력받은 닉네임 정보를 저장하는 코드로 바꿔야 함. 지금은 예시용
                SplitLine[3] = SplitLine[3].Replace("쉼표", ",");
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
            if (checkFirst == 0) // 첫줄은 생략하는 용도
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (checkFirst == 1) //첫번째 npc의 첫번째 대사를 읽고 있는지 체크
            {
                checkFirst++;
                npcNum++;
                DialogueData d = new DialogueData();
                d.NpcId = Convert.ToInt32(SplitLine[0]);
                d.NpcName = SplitLine[1];
                SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                d.Dialogue.Add(SplitLine[2]);
                diaData.Add(d);
            }
            else
            {
                if (Convert.ToString(DiaData[npcNum - 1].NpcId) == SplitLine[0])
                {
                    SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                    SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                    diaData[npcNum - 1].Dialogue.Add(SplitLine[2]);
                }
                else
                {
                    npcNum++;
                    DialogueData d = new DialogueData();
                    d.NpcId = Convert.ToInt32(SplitLine[0]);
                    d.NpcName = SplitLine[1];
                    SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈");
                    SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
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
