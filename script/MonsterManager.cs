using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���͸� ������Ʈ Ǯ�� ���� ��Ȱ��ȭ�ϰ� Ȱ��ȭ�Ѵ�.
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // �̱��� ����
    Queue respawnList =  new Queue(); // �װ� ���� ������ �Ǳ⸦ ��ٸ��� ���͵�(GameObject ���·� ����)
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
        GameManager.instance.ExpUp(expValue); // ����ġ ��½�Ű��
        GameManager.instance.QuestUpdate("����óġ", mid); // ����Ʈ ������Ʈ ȣ��
        StartWaitingRespawn();
    }
    //������ �ð���ŭ ����ϴٰ� ����Ʈ�� ���Ե� ������� ���ʷ� �������ǵ��� ��
    IEnumerator WaitForRespawn()
    {
        yield return wfs;
        RespawnEvent();
        yield break;
    }
}
