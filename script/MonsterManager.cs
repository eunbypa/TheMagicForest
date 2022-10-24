using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MonsterManager
 * Description : Ư�� ��Ȳ�� ���͸� ��Ȱ��ȭ, Ȱ��ȭ�ϴ� �۾��� �����ϴ� ���� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // �̱��� ����
    Queue respawnList =  new Queue(); // �װ� ���� ������ �Ǳ⸦ ��ٸ��� ���͵�
    IEnumerator waitForRespawn; // �������ϱ���� �ɸ��� �ð����� ����ϴ� WaitForRespawn �ڷ�ƾ ����
    WaitForSeconds wfs; // ��� �ð�

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        waitForRespawn = WaitForRespawn(); // �ڷ�ƾ �Ҵ�
        wfs = new WaitForSeconds(5f); // ��� �ð�
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
}
