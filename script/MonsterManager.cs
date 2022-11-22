using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MonsterManager
 * Description : 특정 상황에 몬스터를 비활성화, 활성화하는 작업을 수행하는 몬스터 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // 싱글톤 패턴
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private MonsterList[] monstersList; // 맵에 있는 몬스터들
    [Serializable]
    public struct MonsterList
    {
        [SerializeField] private GameObject[] monsters; // 몬스터들
        /* Property */
        public GameObject[] Monsters
        {
            get
            {
                return monsters;
            }
        }
    }

    int lastMap = -1; // 가장 최근에 있었던 맵
    Queue respawnList = new Queue(); // 죽고 나서 리스폰 되기를 기다리는 몬스터들
    IEnumerator waitForRespawn; // 리스폰하기까지 걸리는 시간동안 대기하는 WaitForRespawn 코루틴 변수
    //IEnumerator waitForReset; // 몬스터 비활성화 전 1초 대기하는 WaitForReset 코루틴 변수
    WaitForSeconds wfs; // 대기 시간
    //WaitForSeconds wfs2; // 대기 시간

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        waitForRespawn = WaitForRespawn(); // 코루틴 할당
        //waitForReset = WaitForReset(); // 코루틴 할당
        wfs = new WaitForSeconds(15f); // 대기 시간
        //wfs2 = new WaitForSeconds(1f); // 대기 시간
        SetMonstersOnMap(GameManager.instance.CurMapNum); // 현재 맵이 몬스터가 있는 맵이면 몬스터 활성화
    }
    /* Method : SetMonstersOnMap
     * Description : 해당 맵에 몬스터들을 모두 활성화하는 메서드입니다. 활성화하기 전 맵을 2차원 배열의 타일 데이터로 나누는 작업을 하기 위해 맵 관리자 클래스의 DivideMapIntoTiles 메서드를 호출합니다.
     * 몬스터가 있는 맵이 아니면 동작을 실행하기 전 빠져나옵니다.
     * Parameter : int idx - 맵 번호
     * Return Value : void
     */
    public void SetMonstersOnMap(int idx)
    {
        if (monstersList[idx].Monsters.Length == 0) return;
        MapManager.instance.DivideMapIntoTiles(monstersList[idx].Monsters[0].GetComponent<Monster>().LocatedMapNum);
        for (int i = 0; i < monstersList[idx].Monsters.Length; i++)
        {
            monstersList[idx].Monsters[i].SetActive(true);
            if(lastMap != -1) monstersList[lastMap].Monsters[i].GetComponent<Monster>().enabled = true;
            monstersList[idx].Monsters[i].GetComponent<Monster>().ResetState();
        }
    }
    /* Method : ResetMonstersOnMap
     * Description : 해당 맵에 몬스터들을 모두 비활성화하는 메서드입니다. 만약 리스폰 대기 중인 몬스터가 있다면 리스폰을 취소합니다. 몬스터가 있는 맵이 아니면 동작을 실행하기 전 빠져나옵니다.
     * Parameter : int idx - 맵 번호
     * Return Value : void
     */
    public void ResetMonstersOnMap(int idx)
    {
        if (monstersList[idx].Monsters.Length == 0) return;
        for (int i = 0; i < monstersList[idx].Monsters.Length; i++)
        {
            monstersList[idx].Monsters[i].SetActive(false);
        }
        StopCoroutine(waitForRespawn);
        respawnList.Clear();
        /*lastMap = idx;
        waitForReset = WaitForReset();
        StartCoroutine(waitForReset);*/
    }
    /* Method : StartWaitingRespawn
     * Description : 리스폰 될 때까지 대기하도록 WaitForRespawn 코루틴을 호출하는 메서드입니다.
     * Return Value : void
     */
    public void StartWaitingRespawn()
    {
        waitForRespawn = WaitForRespawn();
        StartCoroutine(waitForRespawn);
    }
    /* Method : RespawnEvent
     * Description : 몬스터 리스폰 동작을 수행하는 메서드입니다. 큐의 맨 앞에 있는 원소를 Dequeue 메서드로 빼내고 GameObject 형태로 변환해서 활성화시킵니다.
     * Return Value : void
     */
    public void RespawnEvent()
    {
        GameObject monster = respawnList.Dequeue() as GameObject;
        monster.SetActive(true);
        monster.GetComponent<Monster>().ResetState();
    }
    /* Method : DieEvent
     * Description : 몬스터가 죽었을 때 관련 동작을 수행하는 메서드입니다. 매개변수로 받은 GameObject 형태의 몬스터를 큐에 Enqueue 메서드로 삽입하고 비활성화시킵니다.
     * 그리고 GameManager의 ExpUp 메서드에 매개변수로 받은 경험치 값을 전달해 플레이어의 현재 경험치 값을 올리고 GameManager의 QuestUpdate 메서드에 매개변수로 몬스터의 
     * 아이디를 전달해서 해당 몬스터를 처치했다는 메시지를 보냅니다.
     * Parameter : GameObject monster - 몬스터, int expValue - 경험치, int mid - 몬스터 id
     * Return Value : void
     */
    public void DieEvent(GameObject monster, int expValue, int mid)
    {
        respawnList.Enqueue(monster);
        monster.SetActive(false);
        GameManager.instance.ExpUp(expValue); // 경험치 상승시키기
        GameManager.instance.QuestUpdate("몬스터처치", mid); // 퀘스트 업데이트 호출
        StartWaitingRespawn();
    }
    /* Coroutine : WaitForRespawn
     * Description : 리스폰까지 대기하는 동작을 수행하는 코루틴입니다.
     */
    IEnumerator WaitForRespawn()
    {
        yield return wfs;
        RespawnEvent();
        yield break;
    }

    /* Coroutine : WaitForReset
     * Description : 1초 대기 동작을 수행하는 코루틴입니다.
     
    IEnumerator WaitForReset()
    {
        for (int i = 0; i < monstersList[lastMap].Monsters.Length; i++)
        {
            monstersList[lastMap].Monsters[i].GetComponent<Monster>().enabled = false;
        }
        yield return wfs2;
        for (int i = 0; i < monstersList[lastMap].Monsters.Length; i++)
        {
            monstersList[lastMap].Monsters[i].SetActive(false);
        }
        yield break;
    }*/
}