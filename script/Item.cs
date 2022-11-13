using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Item
 * Description : ���� �������� ����ϴ� Ŭ�����Դϴ�. MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class Item : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int itemId; // �������� �ĺ��ϴ� ���̵�
    [SerializeField] private string itemName; // ������ �̸�
    [SerializeField] private string itemInfo; // ������ ����

    /* Property */
    public int ItemId
    {
        get
        {
            return itemId;
        }
    }
    /* Property */
    public string ItemName
    { 
        get
        {
            return itemName;
        }
    }
    /* Property */
    public string ItemInfo
    {
        get
        {
            return itemInfo;
        }
    }

    /* Method : ShowItemInfo
     * Description : �κ��丮���� ���콺 ���� ��ư���� ������ Ŭ�� �� �ش� ������ ������ �����ִ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� �Ͽ����ϴ�.
     * Return Value : void
     */
    public virtual void ShowItemInfo()
    {
        GameManager.instance.ShowItemInfo(itemName, itemInfo);
    }
}
