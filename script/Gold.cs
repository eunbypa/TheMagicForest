using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Gold
 * Description : 게임 내 재화인 골드를 담당하는 클래스입니다. MonoBehaviour 클래스를 상속받습니다.
 */
public class Gold : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int goldNum; // 골드량

    /* Property */
    public int GoldNum
    {
        get
        {
            return goldNum;
        }
    }
}
