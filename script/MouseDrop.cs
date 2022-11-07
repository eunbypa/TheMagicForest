using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Class : MouseDrop
 * Description : UI �̹��� ���콺 ��� �̺�Ʈ�� �ٷ�� Ŭ�����Դϴ�. ����Ƽ�� GameObject�� ������Ʈ�� �߰��ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޾Ұ�, 
 * ���콺 ��� �̺�Ʈ �߻� �� ���ϴ� ������ �����ϵ��� OnDrop �޼��带 ����ϱ� ���� ����Ƽ���� �����ϴ� IDropHandler �������̽��� ��ӹ޾ҽ��ϴ�.
 */
public class MouseDrop : MonoBehaviour, IDropHandler
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int idx; // �̹��� ��ġ
    [SerializeField] private TMPro.TMP_Text curQuantity; // ���� ���� ������ ����

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
            //���� �������� ����� ���
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
