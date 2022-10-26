using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Item
 * Description : 게임 아이템을 담당하는 클래스입니다. MonoBehaviour 클래스를 상속받습니다.
 */
public class Item : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int itemId; // 아이템을 식별하는 아이디

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
     * Description : 인벤토리에서 마우스 왼쪽 버튼으로 아이템 클릭 시 해당 아이템 정보를 보여주는 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 하였습니다. (아직 미구현 상태)
     * Return Value : void
     */
    public virtual void ShowItemInfo()
    {

    }
}
