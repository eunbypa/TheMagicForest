using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MpPotion
 * Description : ���� ������ ��Ÿ���� Ŭ�����Դϴ�. Potion Ŭ������ ��ӹ޽��ϴ�.
 */
public class MpPotion : Potion
{
    int recoverMp = 10; // ��� �� ȸ�� ��ġ -> ������ ���� ����� ����, ���߿� ���� �ʿ�

    // MpPotion ������
    // ItemId�� ���� Ŭ���� Item�� ���� ���� 
    public MpPotion(int itemId)
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
