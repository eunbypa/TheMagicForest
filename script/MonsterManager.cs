using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MonsterManager
 * Description : 특정 상황에 몬스터를 비활성화, 활성화하는 작업을 수행하는 몬스터 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // 싱글톤 패턴
    Queue respawnList =  new Queue(); // 죽고 나서 리스폰 되기를 기다리는 몬스터들
    IEnumerator waitForRespawn; // 리스폰하기까지 걸리는 시간동안 대기하는 WaitForRespawn 코루틴 변수
    WaitForSeconds wfs; // 대기 시간

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        waitForRespawn = WaitForRespawn(); // 코루틴 할당
        wfs = new WaitForSeconds(5f); // 대기 시간
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
}
