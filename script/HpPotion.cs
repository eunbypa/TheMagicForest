using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : HpPotion
 * Description : 체력 포션을 나타내는 클래스입니다. Potion 클래스를 상속받습니다.
 */
public class HpPotion : Potion
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int recoverHp; // 회복 체력

    /* Method : UseItem
    * Description : 아이템 사용 시 동작을 수행하는 메서드입니다.
    * Return Value : void
    */
    public override void UseItem()
    {
        GameManager.instance.HpUp(recoverHp);
    }
}
