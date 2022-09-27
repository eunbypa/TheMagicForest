using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int idx;
    [SerializeField] private GameObject gM;
    //Image image;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        //this.image = GetComponent<image>();
        gm = gM.GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(pointerEventData.button == PointerEventData.InputButton.Left) // 마우스 왼쪽 버튼 클릭 시 실행됨
        {
            //if (image.color.a > 0f) image.color.a = 0f;
            //else image.color.a = 0.5f;
            gm.SelectedItem(idx);
        }
    }
   
}
