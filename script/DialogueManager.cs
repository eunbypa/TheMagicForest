using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : DialogueManager
 * Description : csv 파일에서 대사 데이터들을 읽어들이고 알맞은 곳에 저장하는 동작을 수행하는 대화 데이터 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // 싱글톤 패턴
    int checkFirst = 0; // csv파일의 맨 첫 번째 줄은 저장할 데이터에 해당하지 않으므로 이 경우 저장 과정을 생략하고자 첫 번째 줄을 읽은 상태인지 나타냄
    int curNum = 0; // 현재 읽고 있는 데이터가 몇번째 집합체의 데이터인지 그 정보를 저장함(ex : 1번 npc의 대사는 1번을 기준으로 묶여있으므로 그 묶여있는 대사가 어디까지 있는지 파악하는 용도)
    char[] LineSeperate = new char[] { '\n' }; // 줄바꿈
    char[] CSVSeperate = new char[] { ',' }; // csv는 쉼표를 기준으로 데이터를 나누기 때문에 데이터를 분리하기 위한 쉼표 

    List<DialogueData> diaData = new List<DialogueData>(); // npc의 기본 대사, index : npcId
    List<DialogueData> questAskDiaData = new List<DialogueData>(); // 플레이어에게 퀘스트 요청 시 대사, index : 퀘스트 번호
    List<DialogueData> questAcceptDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 수락 시 대사, index : 퀘스트 번호
    List<DialogueData> questRefuseDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 거절 시 대사, index : 퀘스트 번호
    List<DialogueData> questDoingDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 진행 중 일때 대사, index : 퀘스트 번호
    List<DialogueData> questSuccessDiaData = new List<DialogueData>(); // 플레이어가 퀘스트 성공 시 대사, index : 퀘스트 번호
    List<Tuple<int, List<DialogueData>>> questReplyDiaData = new List<Tuple<int, List<DialogueData>>>(); // 현재 퀘스트 요구사항 타입이 npc와 대화일 때 플레이어가 대화해야 하는 npc일 때 대사, Tuple.Item1 : 퀘스트 번호, Tuple.Item2 : npc 대사

    /* Property */
    public List<DialogueData> DiaData
    {
        get
        {
            return diaData;
        }
    }
    /* Property */
    public List<DialogueData> QuestAskDiaData
    {
        get
        {
            return questAskDiaData;
        }
    }
    /* Property */
    public List<DialogueData> QuestAcceptDiaData
    {
        get
        {
            return questAcceptDiaData;
        }
    }
    /* Property */
    public List<DialogueData> QuestRefuseDiaData
    {
        get
        {
            return questRefuseDiaData;
        }
    }
    /* Property */
    public List<DialogueData> QuestDoingDiaData
    {
        get
        {
            return questDoingDiaData;
        }
    }
    /* Property */
    public List<DialogueData> QuestSuccessDiaData
    {
        get
        {
            return questSuccessDiaData;
        }
    }
    /* Property */
    public List<Tuple<int, List<DialogueData>>> QuestReplyDiaData
    {
        get
        {
            return questReplyDiaData;
        }
    }

    void Awake()
    {
        instance = this;
        //csv 파일 읽어들이고 해당 리스트에 저장하는 작업
        ReadDiaData("NpcScript", ref diaData);
        ReadDiaData("QuestScript_Ask", ref questAskDiaData);
        ReadDiaData("QuestScript_Accept", ref questAcceptDiaData);
        ReadDiaData("QuestScript_Refuse", ref questRefuseDiaData);
        ReadDiaData("QuestScript_Doing", ref questDoingDiaData);
        ReadDiaData("QuestScript_Success", ref questSuccessDiaData);
        ReadDiaData("QuestScript_Reply", ref questReplyDiaData);
    }

    /* Method : ReadDiaData
     * Description : csv 파일에서 데이터를 읽어들이고 줄바꿈 단위로 한 줄씩 분리하고, 한 줄에서 쉼표 단위로 데이터를 분리한 다음 각각의 데이터를 알맞은 위치에 저장하는 동작을 수행하는 메서드입니다.
     * csv 파일은 쉼표를 기준으로 데이터를 분리하기 때문에 npc 대사에 쉼표를 어떻게하면 넣을 수 있을까 고민하다 쉼표가 들어가야 할 위치에 한글로 쉼표라고 쓰고 데이터를 읽어들이는 과정에서 string Replace함수를
     * 이용해 쉼표를 쉼표 기호로 바꿔주는 방식으로 구현했습니다. 마찬가지로 플레이어의 닉네임 또한 타이틀 씬에서 입력받은 정보에 따라 달라지므로 같은 방식으로 구현했는데 아직 타이틀 씬을 구현하지 않아서 닉네임은
     * 일단 임시 이름으로 명시한 상태입니다.
     * Parameter : string s - csv 파일 이름, ref List<DialogueData> dList - DialogueData 리스트 
     * Return Value : void
     */
    void ReadDiaData(string s, ref List<DialogueData> dList)
    {
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
            int nextNum = Convert.ToInt32(SplitLine[0]);
            if (curNum != nextNum)
            {
                curNum++;
                DialogueData d = new DialogueData();
                dList.Add(d);
            }
            if (s == "NpcScript")
            {
                dList[curNum - 1].NpcId = Convert.ToInt32(SplitLine[0]);
                dList[curNum - 1].NpcName = SplitLine[1];
                SplitLine[2] = SplitLine[2].Replace("닉네임", "로빈"); // 나중에 타이틀 씬에서 입력받은 닉네임 정보를 저장하는 코드로 바꿔야 함. 지금은 예시용
                SplitLine[2] = SplitLine[2].Replace("쉼표", ",");
                dList[curNum - 1].Dialogue.Add(SplitLine[2]);
            }
            else
            {
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

    /* Method : ReadDiaData
     * Description : 위의 함수와 같은 동작을 하는 메서드지만 매개변수를 달리 할 필요가 있어 오버로딩하였습니다.
     * Parameter : string s - csv 파일 이름, ref List<Tuple<int, List<DialogueData>>> dList - Tuple 리스트, Tuple은 int와 DialogueData 리스트 쌍으로 구성됨
     * Return Value : void
     */
    void ReadDiaData(string s, ref List<Tuple<int, List<DialogueData>>> dList)
    {
        TextAsset textset = Resources.Load<TextAsset>(s);
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);
        string[] SplitLine;
        int curNpc = 0;
        foreach (string line in Lines)
        {
            if (checkFirst == 0)
            {
                checkFirst++;
                continue;
            }
            SplitLine = line.Split(CSVSeperate, StringSplitOptions.RemoveEmptyEntries);
            int nextNum = Convert.ToInt32(SplitLine[0]);
            if (curNum != nextNum)
            {
                curNum = nextNum;
                dList.Add(new Tuple<int, List<DialogueData>>(curNum, new List<DialogueData>()));
            }
            int nextNpc = Convert.ToInt32(SplitLine[1]);
            if (curNpc != nextNpc)
            {
                curNpc = nextNpc;
                DialogueData d = new DialogueData();
                d.NpcId = nextNpc;
                d.NpcName = SplitLine[2];
                dList[dList.Count - 1].Item2.Add(d);
            }
            SplitLine[3] = SplitLine[3].Replace("닉네임", "로빈"); // 나중에 타이틀 씬에서 입력받은 닉네임 정보를 저장하는 코드로 바꿔야 함. 지금은 예시용
            SplitLine[3] = SplitLine[3].Replace("쉼표", ",");
            dList[dList.Count - 1].Item2[dList[dList.Count - 1].Item2.Count - 1].Dialogue.Add(SplitLine[3]);
        }
    }
}
