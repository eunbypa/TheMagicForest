using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MonoBehaviour
{
    [SerializeField] private int npcId;
    bool wait = false;

    public int NpcId
    {
        get
        {
            return npcId;
        }
    }
    public bool Wait
    {
        get
        {
            return wait;
        }
        set
        {
            wait = value;
        }
    }
    public virtual void UnLockWait()
    {
        wait = false;
    }
    public abstract bool DialogueReady();
    public abstract void SetDiaState();
    public abstract void GetDiaData();
}
