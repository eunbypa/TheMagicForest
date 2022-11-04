using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Potion
 * Description : 포션을 나타내는 클래스입니다. Item 클래스를 상속받습니다.
 */
public class Potion : Item
{
    /* Method : ShowItemInfo
     * Description : 인벤토리에서 마우스 왼쪽 버튼으로 아이템 클릭 시 해당 아이템 정보를 보여주는 동작을 수행하는 메서드입니다. 
     * Return Value : void
     */
    public override void ShowItemInfo()
    {
        GameManager.instance.ShowItemInfo(ItemName, ItemInfo);
        GameManager.instance.OpenUseItemButton();
        GameManager.instance.CurUsedItemLoc = GameManager.instance.InvenManager.FindItem(ItemId);
    }

    /* Method : UseItem
     * Description : 아이템 사용 시 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 구현했습니다.
     * Return Value : void
     */
    public virtual void UseItem()
    {

    }
}
