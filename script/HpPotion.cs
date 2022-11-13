using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : HpPotion
 * Description : ü�� ������ ��Ÿ���� Ŭ�����Դϴ�. Potion Ŭ������ ��ӹ޽��ϴ�.
 */
public class HpPotion : Potion
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int recoverHp; // ȸ�� ü��

    /* Method : UseItem
    * Description : ������ ��� �� ������ �����ϴ� �޼����Դϴ�.
    * Return Value : void
    */
    public override void UseItem()
    {
        GameManager.instance.HpUp(recoverHp);
    }
}
