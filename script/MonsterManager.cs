using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MonsterManager
 * Description : Ư�� ��Ȳ�� ���͸� ��Ȱ��ȭ, Ȱ��ȭ�ϴ� �۾��� �����ϴ� ���� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // �̱��� ����
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private MonsterList[] monstersList; // �ʿ� �ִ� ���͵�
    [Serializable]
    public struct MonsterList
    {
        [SerializeField] private GameObject[] monsters; // ���͵�
        /* Property */
        public GameObject[] Monsters
        {
            get
            {
                return monsters;
            }
        }
    }

    int lastMap = -1; // ���� �ֱٿ� �־��� ��
    Queue respawnList = new Queue(); // �װ� ���� ������ �Ǳ⸦ ��ٸ��� ���͵�
    IEnumerator waitForRespawn; // �������ϱ���� �ɸ��� �ð����� ����ϴ� WaitForRespawn �ڷ�ƾ ����
    //IEnumerator waitForReset; // ���� ��Ȱ��ȭ �� 1�� ����ϴ� WaitForReset �ڷ�ƾ ����
    WaitForSeconds wfs; // ��� �ð�
    //WaitForSeconds wfs2; // ��� �ð�

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        waitForRespawn = WaitForRespawn(); // �ڷ�ƾ �Ҵ�
        //waitForReset = WaitForReset(); // �ڷ�ƾ �Ҵ�
        wfs = new WaitForSeconds(15f); // ��� �ð�
        //wfs2 = new WaitForSeconds(1f); // ��� �ð�
        SetMonstersOnMap(GameManager.instance.CurMapNum); // ���� ���� ���Ͱ� �ִ� ���̸� ���� Ȱ��ȭ
    }
    /* Method : SetMonstersOnMap
     * Description : �ش� �ʿ� ���͵��� ��� Ȱ��ȭ�ϴ� �޼����Դϴ�. Ȱ��ȭ�ϱ� �� ���� 2���� �迭�� Ÿ�� �����ͷ� ������ �۾��� �ϱ� ���� �� ������ Ŭ������ DivideMapIntoTiles �޼��带 ȣ���մϴ�.
     * ���Ͱ� �ִ� ���� �ƴϸ� ������ �����ϱ� �� �������ɴϴ�.
     * Parameter : int idx - �� ��ȣ
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
     * Description : �ش� �ʿ� ���͵��� ��� ��Ȱ��ȭ�ϴ� �޼����Դϴ�. ���� ������ ��� ���� ���Ͱ� �ִٸ� �������� ����մϴ�. ���Ͱ� �ִ� ���� �ƴϸ� ������ �����ϱ� �� �������ɴϴ�.
     * Parameter : int idx - �� ��ȣ
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
     * Description : ������ �� ������ ����ϵ��� WaitForRespawn �ڷ�ƾ�� ȣ���ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void StartWaitingRespawn()
    {
        waitForRespawn = WaitForRespawn();
        StartCoroutine(waitForRespawn);
    }
    /* Method : RespawnEvent
     * Description : ���� ������ ������ �����ϴ� �޼����Դϴ�. ť�� �� �տ� �ִ� ���Ҹ� Dequeue �޼���� ������ GameObject ���·� ��ȯ�ؼ� Ȱ��ȭ��ŵ�ϴ�.
     * Return Value : void
     */
    public void RespawnEvent()
    {
        GameObject monster = respawnList.Dequeue() as GameObject;
        monster.SetActive(true);
        monster.GetComponent<Monster>().ResetState();
    }
    /* Method : DieEvent
     * Description : ���Ͱ� �׾��� �� ���� ������ �����ϴ� �޼����Դϴ�. �Ű������� ���� GameObject ������ ���͸� ť�� Enqueue �޼���� �����ϰ� ��Ȱ��ȭ��ŵ�ϴ�.
     * �׸��� GameManager�� ExpUp �޼��忡 �Ű������� ���� ����ġ ���� ������ �÷��̾��� ���� ����ġ ���� �ø��� GameManager�� QuestUpdate �޼��忡 �Ű������� ������ 
     * ���̵� �����ؼ� �ش� ���͸� óġ�ߴٴ� �޽����� �����ϴ�.
     * Parameter : GameObject monster - ����, int expValue - ����ġ, int mid - ���� id
     * Return Value : void
     */
    public void DieEvent(GameObject monster, int expValue, int mid)
    {
        respawnList.Enqueue(monster);
        monster.SetActive(false);
        GameManager.instance.ExpUp(expValue); // ����ġ ��½�Ű��
        GameManager.instance.QuestUpdate("����óġ", mid); // ����Ʈ ������Ʈ ȣ��
        StartWaitingRespawn();
    }
    /* Coroutine : WaitForRespawn
     * Description : ���������� ����ϴ� ������ �����ϴ� �ڷ�ƾ�Դϴ�.
     */
    IEnumerator WaitForRespawn()
    {
        yield return wfs;
        RespawnEvent();
        yield break;
    }

    /* Coroutine : WaitForReset
     * Description : 1�� ��� ������ �����ϴ� �ڷ�ƾ�Դϴ�.
     
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