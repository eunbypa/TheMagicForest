using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Class : MouseDrop
 * Description : UI 이미지 마우스 드랍 이벤트를 다루는 클래스입니다. 유니티의 GameObject에 컴포넌트로 추가하기 위해 MonoBehaviour 클래스를 상속받았고, 
 * 마우스 드랍 이벤트 발생 시 원하는 동작을 수행하도록 OnDrop 메서드를 사용하기 위해 유니티에서 제공하는 IDropHandler 인터페이스도 상속받았습니다.
 */
public class MouseDrop : MonoBehaviour, IDropHandler
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int idx; // 이미지 위치
    [SerializeField] private TMPro.TMP_Text curQuantity; // 현재 남은 아이템 수량

    void Start()
    {
    }

    void Update()
    {
        if (GameManager.instance.CurShortCutPotions[idx] != -1)
        {
            if (GameManager.instance.InvenManager.FindItem(GameManager.instance.CurShortCutPotions[idx]) != -1)
            {
                curQuantity.text = Convert.ToString(GameManager.instance.InvenManager.InvenItemQuantityList[GameManager.instance.InvenManager.FindItem(GameManager.instance.CurShortCutPotions[idx])]);
            }
            else curQuantity.text = "0";
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(GameManager.instance.CurDragItemLoc != -1)
        {
            //포션 아이템을 드랍한 경우
            if (GameManager.instance.InvenManager.InvenItemList[GameManager.instance.CurDragItemLoc].ItemId == 1 || GameManager.instance.InvenManager.InvenItemList[GameManager.instance.CurDragItemLoc].ItemId == 2)
            {
                GetComponent<Image>().sprite = GameManager.instance.DragImage.GetComponent<Image>().sprite;
                Color color;
                color = GetComponent<Image>().color;
                color.a = 1f;
                GetComponent<Image>().color = color;
                GameManager.instance.CurShortCutPotions[idx] = GameManager.instance.InvenManager.InvenItemList[GameManager.instance.CurDragItemLoc].ItemId;
            }
        }
        GameManager.instance.CurDragItemLoc = -1;
    }
}
