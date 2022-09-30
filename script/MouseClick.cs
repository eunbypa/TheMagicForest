using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

/* Class : MouseClick
 * Description : UI �̹��� ���콺 Ŭ�� �̺�Ʈ�� �ٷ�� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޾Ұ�, ���콺 Ŭ�� �̺�Ʈ �߻� �� ���ϴ� ������ �����ϵ���
 * OnPointerClick �޼��带 ����ϱ� ���� ����Ƽ���� �����ϴ� IPointerClickHandler �������̽��� ��ӹ޾ҽ��ϴ�.
 */
public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private int idx; // �̹��� ��ġ(ex : �κ��丮 1��° ĭ)
    [SerializeField] private GameObject gM; // ���� ������ GameManager

    GameManager gm; // ���� ������ GameManager Ŭ���� ��ü

    void Start()
    {
        gm = gM.GetComponent<GameManager>(); // gM GameObject ��ü�� �Ҵ�� GameManager Ŭ���� ������Ʈ�� �����ɴϴ�.
    }

    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(pointerEventData.button == PointerEventData.InputButton.Left) // ���콺 ���� ��ư Ŭ�� �� ����
        {
            gm.SelectedItem(idx); // gm���� SelectedItem �޼��忡 ���� �ڽ��� �̹��� ��ġ ������ �Ű������� ������ ȣ��
        }
    }
   
}
