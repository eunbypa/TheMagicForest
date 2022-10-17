using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : QuestManager
 * Description : 작성해둔 csv 파일에서 게임 퀘스트 데이터를 읽어들이는 동작을 수행하는 퀘스트 데이터 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class QuestManager : MonoBehaviour
{
    private List<QuestData> questDataList = new List<QuestData>(); // 퀘스트 데이터

    char[] LineSeperate = new char[] { '\n' }; // 줄바꿈
    char[] CSVSeperate = new char[] { ',' }; // csv는 쉼표를 기준으로 데이터를 나누기 때문에 데이터를 분리하기 위한 쉼표 

    /* Property */
    public List<QuestData> QuestDataList
    {
        get
        {
            return questDataList;
        }
    }

    void Awake()
    {
        //csv 파일 읽어들이는 작업
        ReadData("QuestList");
        ReadData("Quest_Requirement");
        ReadData("Quest_Reward");
    }

    /* Method : ReadData
     * Description : csv 파일에서 데이터를 읽어들이고 줄바꿈 단위로 한 줄씩 분리하고, 한 줄에서 쉼표 단위로 데이터를 분리한 다음 각각의 데이터를 알맞은 위치에 저장하는 동작을 수행하는 메서드입니다.
     * 퀘스트의 데이터를 한 csv 파일에 다 작성한 게 아니라 여러 개로 나누어서 작성했기 때문에 이를 순서대로 읽는 과정에서 QuestData 객체를 여러번 생성하지 않도록 현재 읽고 있는 데이터가 가리키는 퀘스트 번호가
     * 퀘스트 데이터 리스트 객체 수보다 작거나 같은 경우만 새로 객체를 생성해 리스트에 추가하도록 구현했습니다.
     * Parameter : string s - csv 파일 이름
     * Return Value : void
     */
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
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            if (Convert.ToString(curNum) != SplitLine[0])
            {
                if (questDataList.Count <= curNum)
                {
                    QuestData qdata = new QuestData();
                    questDataList.Add(qdata);
                }
                curNum++;
                i++;
                if (i == 3) return;
            }
            if (s == "QuestList")
            {
                questDataList[curNum - 1].Title = SplitLine[1];
                questDataList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[2]);
                questDataList[curNum - 1].NpcName = SplitLine[3];
                questDataList[curNum - 1].Info = SplitLine[4];
            }
            if (s == "Quest_Requirement")
            {
                questDataList[curNum - 1].Type.Add(SplitLine[1]);
                if (SplitLine[2] != "없음") questDataList[curNum - 1].Req_Id.Add(Convert.ToInt32(SplitLine[2]));
                questDataList[curNum - 1].Req_Name.Add(SplitLine[3]);
                questDataList[curNum - 1].Req_Num.Add(Convert.ToInt32(SplitLine[4]));
            }
            if (s == "Quest_Reward")
            {
                questDataList[curNum - 1].RewardType.Add(SplitLine[1]);
                questDataList[curNum - 1].Reward.Add(Convert.ToInt32(SplitLine[2]));
            }
        }
    }
}
