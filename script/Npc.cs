using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Npc
 * Description : npc�� ��Ÿ���� �߻� Ŭ�����Դϴ�. ���� Ŭ�������� ����Ƽ�� ���� �ֱ� �Լ����� ����� �� �ְ� MonoBehaviour Ŭ������ ��ӹ޾ҽ��ϴ�.
 */
public abstract class Npc : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int npcId; // npc�� �ĺ��ϴ� ���̵�

    bool wait = false; // ��ȭ ��� ���� ����

    /* Property */
    public int NpcId
    {
        get
        {
            return npcId;
        }
    }
    /* Property */
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

    /* Method : UnLockWait
     * Description : npc�� ��ȭ ��� ���¸� Ǯ���ִ� �޼����Դϴ�. 
     * Return Value : void
     */
    public virtual void UnLockWait()
    {
        wait = false;
    }

    /* Method : DialogueReady
     * Description : ���� Ŭ�������� npc ��ȭ �غ� �Ϸ� ���¸� true�� ��ȭ ��� ���¸� false�� ��ȯ�ϴ� ������ �����ϵ��� ������ �߻� �޼����Դϴ�. ���� Ŭ�������� �ݵ�� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Return Value : bool
     */
    public abstract bool DialogueReady();

    /* Method : SetDiaState
     * Description : ���� Ŭ�������� npc�� ��ȭ ���¸� �����ϴ� ������� ������ �߻� �޼����Դϴ�. ���� Ŭ�������� �ݵ�� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Return Value : void
     */
    public abstract void SetDiaState();

    /* Method : GetDiaData
     * Description : ���� Ŭ�������� npc�� ��ȭ ���¸� �������� ��縦 �������� ������� ������ �߻� �޼����Դϴ�. ���� Ŭ�������� �ݵ�� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Return Value : void
     */
    public abstract void GetDiaData();
}
