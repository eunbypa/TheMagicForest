using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : SaveData
 * Description : ���� ���� �����͸� ����ϴ� Ŭ�����Դϴ�. �����ؾ� �� �����͸� ��Ƶξ����ϴ�.
 */
[Serializable]
public class SaveData
{
    public Vector3 playerPos; // �÷��̾� ��ġ
    public int curMapNum; // �÷��̾ ��ġ�� �� ��ȣ
    public int level; // �÷��̾� ����
    public string name; // �÷��̾� �̸�
    public int gold = -1; // ���� ��差(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int curHp = -1; // ���� ü��(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int maxHp = -1; // �ִ� ü��(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int curMp = -1; // ���� ����(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int maxMp = -1; // �ִ� ����(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int curExp = -1; // ���� ����ġ(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public int maxExp = -1; // �ִ� ����ġ(-1 �̸� ����� ���� ���ٴ� �ǹ�)
    public string magicStone; // ���� �������� ������
    public int[] shortCutPotions = new int[2] { -1, -1 }; // ���� ����Ű�� ��ϵ� ���� ������ id
    public int[] shortCutPotionsQuantity; // ���� ����Ű�� ��ϵ� ���� ���� ����
    public List<int> invenItemIdList; // �κ��丮 �� ĭ�� ��ġ�� �������� ���̵� ��Ÿ��
    public List<int> invenItemQuantityList; // �κ��丮 �� ĭ�� ������ ������ ��Ÿ��
    public int curQuestNum; // ���� ���� ����Ʈ ��ȣ
    public int curQuestNpcId; // ���� ���� ����Ʈ ��� npc id
    public bool accept; // ���� ����
    public bool success; // ���� ����
    public List<int> curQuestReqNum; // ���� �������� ����Ʈ ������ �ʿ��� ���� ���� ���� ���
  
}
