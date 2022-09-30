using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MpPotion
 * Description : 마력 포션을 나타내는 클래스입니다. Potion 클래스를 상속받습니다.
 */
public class MpPotion : Potion
{
    int recoverMp = 10; // 사용 시 회복 수치 -> 지금은 값을 명시한 상태, 나중에 수정 필요

    // MpPotion 생성자
    // ItemId는 상위 클래스 Item에 속한 정보 
    public MpPotion(int itemId)
    {
        ItemId = itemId;
    }

    /* Method : ShowItemInfo
     * Description : 인벤토리에서 마우스 왼쪽 버튼으로 아이템 클릭 시 해당 아이템 정보를 보여주는 동작을 수행하는 메서드입니다. (아직 미구현 상태)
     * Return Value : void
     */
    public override void ShowItemInfo()
    {

    }
}
