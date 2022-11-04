using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Potion
 * Description : ������ ��Ÿ���� Ŭ�����Դϴ�. Item Ŭ������ ��ӹ޽��ϴ�.
 */
public class Potion : Item
{
    /* Method : ShowItemInfo
     * Description : �κ��丮���� ���콺 ���� ��ư���� ������ Ŭ�� �� �ش� ������ ������ �����ִ� ������ �����ϴ� �޼����Դϴ�. 
     * Return Value : void
     */
    public override void ShowItemInfo()
    {
        GameManager.instance.ShowItemInfo(ItemName, ItemInfo);
        GameManager.instance.OpenUseItemButton();
        GameManager.instance.CurUsedItemLoc = GameManager.instance.InvenManager.FindItem(ItemId);
    }

    /* Method : UseItem
     * Description : ������ ��� �� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� �����߽��ϴ�.
     * Return Value : void
     */
    public virtual void UseItem()
    {

    }
}
