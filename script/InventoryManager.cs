using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : InventoryManager
 * Description : �κ��丮 �� ������ ���� ���� �� �κ��丮�� �����ϴ� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޾ҽ��ϴ�.
 */
public class InventoryManager : MonoBehaviour
{
    int maxSize = 28; // �κ��丮 �ִ� ĭ ��
    bool isFull = false; // ������ �� ���� ���� �� á���� ����

    List<Item> invenItemList = new List<Item>(); // �κ��丮 �� ĭ�� ��ġ�� ������ ��Ÿ��
    List<int> invenItemQuantityList = new List<int>(); // �κ��丮 �� ĭ�� ������ ������ ��Ÿ��

    //���� ������ ����&�ε� ��� ���� �� ����� �����Ϳ��� �κ��丮 ������ �������� �۾� ���� ����
    void Start()
    {
        
    }

    /* Property */
    public int MaxSize
    {
        get
        {
            return maxSize;
        }
    }
    /* Property */
    public bool IsFull
    {
        get
        {
            return isFull;
        }
    }
    /* Property */
    public List<Item> InvenItemList
    {
        get
        {
            return invenItemList;
        }
    }
    /* Property */
    public List<int> InvenItemQuantityList
    {
        get
        {
            return invenItemQuantityList;
        }
    }

    /* Method : ItemInsert
     * Description : �κ��丮�� ���������� ���� �������� ȹ���� ��� �κ��丮 ������ ����Ʈ�� �ش� �������� �������ִ� ������ �����ϴ� �޼����Դϴ�.
     * ���� �����ϰ� ���� �κ��丮�� �� �� ���°� �Ǹ� IsFull ���� true�� �ٲٵ��� �����߽��ϴ�.
     * Parameter : Item item - ������, int num - ������ ����
     * Return Value : void
     */
    public void ItemInsert(Item item, int num)
    {
        invenItemList.Add(item);
        invenItemQuantityList.Add(num);
        if (invenItemList.Count == maxSize) isFull = true;
    }

    /* Method : ItemDelete
     * Description : �κ��丮�� ���� ���� �������� ������ 0�� �Ǿ��� �� �ش� �������� �κ��丮���� �����ϴ� ������ �����ϴ� �޼����Դϴ�.
     * ���� �����ϱ� �� �κ��丮�� �� �� ���¿����� ���� �� �� ������ �ִٴ� ������ IsFull ���� false�� �ٲٵ��� �����߽��ϴ�.
     * Parameter : int idx - �κ��丮 �� ������ ��ġ
     * Return Value : void
     */
    public void ItemDelete(int idx)
    {
        invenItemList.RemoveAt(idx);
        invenItemQuantityList.RemoveAt(idx);
        if (isFull) isFull = false;
    }

    /* Method : ItemQuantityIncrease
     * Description : �κ��丮 �� �ش� ������ ���� ������ ������Ű�� �޼����Դϴ�.
     * Parameter : int idx - �κ��丮 �� ������ ��ġ, int num - ������ ����
     * Return Value : void
     */
    public void ItemQuantityIncrease(int idx, int num)
    {
        invenItemQuantityList[idx] += num;
    }

    /* Method : ItemQuantityDecrease
     * Description : �κ��丮 �� �ش� ������ ���� ������ ���ҽ�Ű�� �޼����Դϴ�.
     * Parameter : int idx - �κ��丮 �� ������ ��ġ, int num - ������ ����
     * Return Value : void
     */
    public void ItemQuantityDecrease(int idx, int num)
    {
        if (invenItemQuantityList[idx] - num <= 0)
        {
            ItemDelete(idx);
            return;
        }
        invenItemQuantityList[idx] -= num;
    }

    /* Method : FindItem
     * Description : �κ��丮 �� �������� �����۵� �� �ش� ���̵� ������ ������ Ž�� ������ �����ϴ� �޼����Դϴ�. ���� �������� ��� �ش� �������� ��ġ ���� ��ȯ�ϰ� �������� �������� �ƴ϶�� -1�� ��ȯ�մϴ�.  
     * Parameter : int itemId - ������ ���̵�
     * Return Value : void
     */
    public int FindItem(int itemId)
    {
        for(int i = 0; i < invenItemList.Count; i++)
        {
            if(invenItemList[i].ItemId == itemId)
            {
                return i;
            }
        }
        return -1;
    }
}
