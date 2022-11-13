using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : MpPotion
 * Description : ���� ������ ��Ÿ���� Ŭ�����Դϴ�. Potion Ŭ������ ��ӹ޽��ϴ�.
 */
public class MpPotion : Potion
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int recoverMp; // ȸ�� ����

    /* Method : UseItem
     * Description : ������ ��� �� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public override void UseItem()
    {
        GameManager.instance.MpUp(recoverMp);
    }
}
