using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] Mop;
    
    public void DieEvent(int n)
    {
        Mop[n].SetActive(false);
    }
}
