using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : HpPotion
 * Description : ü�� ������ ��Ÿ���� Ŭ�����Դϴ�. Potion Ŭ������ ��ӹ޽��ϴ�.
 */
public class HpPotion : Potion
{
    int recoverHp = 10; // ��� �� ȸ�� ��ġ -> ������ ���� ����� ����, ���߿� ���� �ʿ�

    // HpPotion ������
    // ItemId�� ���� Ŭ���� Item�� ���� ���� 
    public HpPotion(int itemId)
    {
        ItemId = itemId;
    }

    /* Method : ShowItemInfo
     * Description : �κ��丮���� ���콺 ���� ��ư���� ������ Ŭ�� �� �ش� ������ ������ �����ִ� ������ �����ϴ� �޼����Դϴ�. (���� �̱��� ����)
     * Return Value : void
     */
    public override void ShowItemInfo()
    {

    }
}
