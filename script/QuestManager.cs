using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : QuestManager
 * Description : �ۼ��ص� csv ���Ͽ��� ���� ����Ʈ �����͸� �о���̴� ������ �����ϴ� ����Ʈ ������ ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class QuestManager : MonoBehaviour
{
    private List<QuestData> questDataList = new List<QuestData>(); // ����Ʈ ������

    char[] LineSeperate = new char[] { '\n' }; // �ٹٲ�
    char[] CSVSeperate = new char[] { ',' }; // csv�� ��ǥ�� �������� �����͸� ������ ������ �����͸� �и��ϱ� ���� ��ǥ 

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
        //csv ���� �о���̴� �۾�
        ReadData("QuestList");
        ReadData("Quest_Requirement");
        ReadData("Quest_Reward");
    }

    /* Method : ReadData
     * Description : csv ���Ͽ��� �����͸� �о���̰� �ٹٲ� ������ �� �پ� �и��ϰ�, �� �ٿ��� ��ǥ ������ �����͸� �и��� ���� ������ �����͸� �˸��� ��ġ�� �����ϴ� ������ �����ϴ� �޼����Դϴ�.
     * ����Ʈ�� �����͸� �� csv ���Ͽ� �� �ۼ��� �� �ƴ϶� ���� ���� ����� �ۼ��߱� ������ �̸� ������� �д� �������� QuestData ��ü�� ������ �������� �ʵ��� ���� �а� �ִ� �����Ͱ� ����Ű�� ����Ʈ ��ȣ��
     * ����Ʈ ������ ����Ʈ ��ü ������ �۰ų� ���� ��츸 ���� ��ü�� ������ ����Ʈ�� �߰��ϵ��� �����߽��ϴ�.
     * Parameter : string s - csv ���� �̸�
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
                if (SplitLine[2] != "����") questDataList[curNum - 1].Req_Id.Add(Convert.ToInt32(SplitLine[2]));
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
