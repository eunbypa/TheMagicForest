using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Gold
 * Description : ���� �� ��ȭ�� ��带 ����ϴ� Ŭ�����Դϴ�. MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class Gold : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int goldNum; // ��差

    /* Property */
    public int GoldNum
    {
        get
        {
            return goldNum;
        }
    }
}
