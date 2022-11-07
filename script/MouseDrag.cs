using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Class : MouseDrag
 * Description : UI 이미지 마우스 드래그 이벤트를 다루는 클래스입니다. 유니티의 GameObject에 컴포넌트로 추가하기 위해 MonoBehaviour 클래스를 상속받았고, 
 * 마우스 드래그 이벤트 발생 시 원하는 동작을 수행하도록 OnBeginDrag, OnDrag, OnEndDrag 메서드를 사용하기 위해 유니티에서 제공하는 IBeginDragHandler, IEndDragHandler, IDragHandler 인터페이스도 상속받았습니다.
 */
public class MouseDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    void Start()
    {
        
    }
    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.instance.CurDragItemLoc = GetComponent<MouseClick>().Idx;
        GameManager.instance.DragImage.transform.position = this.transform.position;
        GameManager.instance.DragImage.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        GameManager.instance.DragImage.SetActive(true);
    }
    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        GameManager.instance.DragImage.transform.position = eventData.position;
    }
    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.instance.DragImage.SetActive(false);
    }
}
