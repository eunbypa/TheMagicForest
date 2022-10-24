using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Gold
 * Description : 게임 내 재화인 골드를 담당하는 클래스입니다.
 */
public class Gold
{
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
