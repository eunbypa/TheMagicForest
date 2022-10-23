using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터를 오브젝트 풀을 통해 비활성화하고 활성화한다.
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // 싱글톤 패턴
    Queue respawnList =  new Queue(); // 죽고 나서 리스폰 되기를 기다리는 몬스터들(GameObject 형태로 삽입)
    IEnumerator waitForRespawn;
    WaitForSeconds wfs;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        waitForRespawn = WaitForRespawn();
        wfs = new WaitForSeconds(5f);
    }

    public void StartWaitingRespawn()
    {
        waitForRespawn = WaitForRespawn();
        StartCoroutine(waitForRespawn);
    }
    public void RespawnEvent()
    {
        Debug.Log(respawnList.Count);
        GameObject monster = respawnList.Dequeue() as GameObject;
        monster.SetActive(true);
        monster.GetComponent<Monster>().ResetState();
        if (respawnList.Count == 0) StopCoroutine(waitForRespawn);
    }
    public void DieEvent(GameObject monster, int expValue, int mid)
    {
        respawnList.Enqueue(monster);
        monster.SetActive(false);
        GameManager.instance.ExpUp(expValue); // 경험치 상승시키기
        GameManager.instance.QuestUpdate("몬스터처치", mid); // 퀘스트 업데이트 호출
        StartWaitingRespawn();
    }
    //설정한 시간만큼 대기하다가 리스트에 삽입된 순서대로 차례로 리스폰되도록 함
    IEnumerator WaitForRespawn()
    {
        yield return wfs;
        RespawnEvent();
        yield break;
    }
}
