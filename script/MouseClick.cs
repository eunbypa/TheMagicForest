using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/* Class : MouseClick
 * Description : UI 이미지 마우스 클릭 이벤트를 다루는 클래스입니다. 유니티의 GameObject에 컴포넌트로 추가하기 위해 MonoBehaviour 클래스를 상속받았고, 마우스 클릭 이벤트 발생 시 원하는 동작을 수행하도록
 * OnPointerClick 메서드를 사용하기 위해 유니티에서 제공하는 IPointerClickHandler 인터페이스도 상속받았습니다.
 */
public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int idx; // 이미지 위치(ex : 인벤토리 1번째 칸)
    /* Property */
    public int Idx
    {
        get
        {
            return idx;
        }
    }
    void Start()
    {

    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left) // 마우스 왼쪽 버튼 클릭 시 실행
        {
            GameManager.instance.SelectedItem(idx); // gm에서 SelectedItem 메서드에 현재 자신의 이미지 위치 정보를 매개변수로 전달해 호출
        }
    }

}
