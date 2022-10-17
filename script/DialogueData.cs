using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : DialogueData
 * Description : npc�� ��ȭ �����͸� ����ϴ� Ŭ�����Դϴ�.
 */
public class DialogueData
{
    int npcId; // npc�� �ĺ��ϴ� ���̵�
    string npcName; // npc�� �̸�
    List<string> dialogue = new List<string>(); // ��ȭ �� ��� ������ ������� ����� ����Ʈ, index : ��ȭ ����

    /* Property */
    public int NpcId
    {
        get
        {
            return npcId;
        }
        set
        {
            npcId = value;
        }
    }
    /* Property */
    public string NpcName
    {
        get
        {
            return npcName;
        }
        set
        {
            npcName = value;
        }
    }
    /* Property */
    public List<string> Dialogue
    {
        get
        {
            return dialogue;
        }
        set
        {
            dialogue = value;
        }
    }

}
