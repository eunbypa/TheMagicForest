using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Item
 * Description : ���� �������� ����ϴ� Ŭ�����Դϴ�.
 */
public class Item
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int itemId; // �������� �ĺ��ϴ� ���̵�

    /* Property */
    public int ItemId
    {
        get
        {
            return itemId;
        }
        set
        {
            itemId = value;
        }
    }

    /* Method : ShowItemInfo
     * Description : �κ��丮���� ���콺 ���� ��ư���� ������ Ŭ�� �� �ش� ������ ������ �����ִ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� �Ͽ����ϴ�. (���� �̱��� ����)
     * Return Value : void
     */
    public virtual void ShowItemInfo()
    {

    }
}
