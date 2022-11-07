using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Class : MouseDrag
 * Description : UI �̹��� ���콺 �巡�� �̺�Ʈ�� �ٷ�� Ŭ�����Դϴ�. ����Ƽ�� GameObject�� ������Ʈ�� �߰��ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޾Ұ�, 
 * ���콺 �巡�� �̺�Ʈ �߻� �� ���ϴ� ������ �����ϵ��� OnBeginDrag, OnDrag, OnEndDrag �޼��带 ����ϱ� ���� ����Ƽ���� �����ϴ� IBeginDragHandler, IEndDragHandler, IDragHandler �������̽��� ��ӹ޾ҽ��ϴ�.
 */
public class MouseDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    void Start()
    {
        
    }
    //�巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.instance.CurDragItemLoc = GetComponent<MouseClick>().Idx;
        GameManager.instance.DragImage.transform.position = this.transform.position;
        GameManager.instance.DragImage.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        GameManager.instance.DragImage.SetActive(true);
    }
    //�巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        GameManager.instance.DragImage.transform.position = eventData.position;
    }
    //�巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.instance.DragImage.SetActive(false);
    }
}
