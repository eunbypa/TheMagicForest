using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : DialogueManager
 * Description : csv ���Ͽ��� ��� �����͵��� �о���̰� �˸��� ���� �����ϴ� ������ �����ϴ� ��ȭ ������ ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // �̱��� ����
    int checkFirst = 0; // csv������ �� ù ��° ���� ������ �����Ϳ� �ش����� �����Ƿ� �� ��� ���� ������ �����ϰ��� ù ��° ���� ���� �������� ��Ÿ��
    int curNum = 0; // ���� �а� �ִ� �����Ͱ� ���° ����ü�� ���������� �� ������ ������(ex : 1�� npc�� ���� 1���� �������� ���������Ƿ� �� �����ִ� ��簡 ������ �ִ��� �ľ��ϴ� �뵵)
    char[] LineSeperate = new char[] { '\n' }; // �ٹٲ�
    char[] CSVSeperate = new char[] { ',' }; // csv�� ��ǥ�� �������� �����͸� ������ ������ �����͸� �и��ϱ� ���� ��ǥ 

    List<DialogueData> diaData = new List<DialogueData>(); // npc�� �⺻ ���, index : npcId
    List<DialogueData> questAskDiaData = new List<DialogueData>(); // �÷��̾�� ����Ʈ ��û �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questAcceptDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questRefuseDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questDoingDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� �϶� ���, index : ����Ʈ ��ȣ
    List<DialogueData> questSuccessDiaData = new List<DialogueData>(); // �÷��̾ ����Ʈ ���� �� ���, index : ����Ʈ ��ȣ
    List<Tuple<int, List<DialogueData>>> questReplyDiaData = new List<Tuple<int, List<DialogueData>>>(); // ���� ����Ʈ �䱸���� Ÿ���� npc�� ��ȭ�� �� �÷��̾ ��ȭ�ؾ� �ϴ� npc�� �� ���, Tuple.Item1 : ����Ʈ ��ȣ, Tuple.Item2 : npc ���

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
        //csv ���� �о���̰� �ش� ����Ʈ�� �����ϴ� �۾�
        ReadDiaData("NpcScript", ref diaData);
        ReadDiaData("QuestScript_Ask", ref questAskDiaData);
        ReadDiaData("QuestScript_Accept", ref questAcceptDiaData);
        ReadDiaData("QuestScript_Refuse", ref questRefuseDiaData);
        ReadDiaData("QuestScript_Doing", ref questDoingDiaData);
        ReadDiaData("QuestScript_Success", ref questSuccessDiaData);
        ReadDiaData("QuestScript_Reply", ref questReplyDiaData);
    }

    /* Method : ReadDiaData
     * Description : csv ���Ͽ��� �����͸� �о���̰� �ٹٲ� ������ �� �پ� �и��ϰ�, �� �ٿ��� ��ǥ ������ �����͸� �и��� ���� ������ �����͸� �˸��� ��ġ�� �����ϴ� ������ �����ϴ� �޼����Դϴ�.
     * csv ������ ��ǥ�� �������� �����͸� �и��ϱ� ������ npc ��翡 ��ǥ�� ����ϸ� ���� �� ������ ����ϴ� ��ǥ�� ���� �� ��ġ�� �ѱ۷� ��ǥ��� ���� �����͸� �о���̴� �������� string Replace�Լ���
     * �̿��� ��ǥ�� ��ǥ ��ȣ�� �ٲ��ִ� ������� �����߽��ϴ�. ���������� �÷��̾��� �г��� ���� Ÿ��Ʋ ������ �Է¹��� ������ ���� �޶����Ƿ� ���� ������� �����ߴµ� ���� Ÿ��Ʋ ���� �������� �ʾƼ� �г�����
     * �ϴ� �ӽ� �̸����� ����� �����Դϴ�.
     * Parameter : string s - csv ���� �̸�, ref List<DialogueData> dList - DialogueData ����Ʈ 
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
                SplitLine[2] = SplitLine[2].Replace("�г���", "�κ�"); // ���߿� Ÿ��Ʋ ������ �Է¹��� �г��� ������ �����ϴ� �ڵ�� �ٲ�� ��. ������ ���ÿ�
                SplitLine[2] = SplitLine[2].Replace("��ǥ", ",");
                dList[curNum - 1].Dialogue.Add(SplitLine[2]);
            }
            else
            {
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

    /* Method : ReadDiaData
     * Description : ���� �Լ��� ���� ������ �ϴ� �޼������� �Ű������� �޸� �� �ʿ䰡 �־� �����ε��Ͽ����ϴ�.
     * Parameter : string s - csv ���� �̸�, ref List<Tuple<int, List<DialogueData>>> dList - Tuple ����Ʈ, Tuple�� int�� DialogueData ����Ʈ ������ ������
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
            SplitLine[3] = SplitLine[3].Replace("�г���", "�κ�"); // ���߿� Ÿ��Ʋ ������ �Է¹��� �г��� ������ �����ϴ� �ڵ�� �ٲ�� ��. ������ ���ÿ�
            SplitLine[3] = SplitLine[3].Replace("��ǥ", ",");
            dList[dList.Count - 1].Item2[dList[dList.Count - 1].Item2.Count - 1].Dialogue.Add(SplitLine[3]);
        }
    }
}
