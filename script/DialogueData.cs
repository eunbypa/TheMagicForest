using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData //: MonoBehaviour
{
    //public int npcid;
    //public string name;
    //public List<string> Dialogue =  new List<string>();

    int npcId;
    string npcName;
    List<string> dialogue = new List<string>();

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
