using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Portal
 * Description : 맵 이동에 필요한 포탈을 담당하는 클래스입니다. 유니티의 생명 주기 함수들과 충돌 처리 함수를 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class Portal : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private GameObject player; // 플레이어
    [SerializeField] private GameObject destination; // 도착지 포탈
    [SerializeField] private GameObject curMap; // 현재 맵
    [SerializeField] private GameObject nextMap; // 다음 맵
    [SerializeField] private int departureMapNum; // 출발지 맵 번호
    [SerializeField] private int destinationMapNum; // 도착지 맵 번호

    bool teleport = false; // 플레이어가 포탈과 접촉해 있어서 텔레포트가 가능한 상태인지 여부


    void Start()
    {

    }

    void Update()
    {
        if (teleport) // 텔레포트 가능한 상태일 때
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // 플레이어가 위쪽 방향키를 누른 경우 
            {
                GameManager.instance.WaitForTeleportReady(); // 텔레포트 완료까지 걸리는 시간만큼 대기하기 위해 gm의 WaitForTeleportReady 메서드 호출
            }
            if (GameManager.instance.TeleportReady) TeleportEvent(); // 텔레포트 준비가 끝났으면 Teleport 시작
        }
    }

    /* Method : TeleportEvent
     * Description : 텔레포트 시 동작을 수행하는 메서드입니다. gm의 TeleportMap 함수에 도착지 맵 번호를 매개변수로 전달해서 이동 후의 맵 번호를 알 수 있게 했고 플레이어의 위치를 도착지 포탈 위치로 바꾸도록
     * 구현했습니다. 그리고 현재 맵을 비활성화하고 도착지 맵을 활성화합니다.  
     * Return Value : void
     */
    public void TeleportEvent()
    {
        GameManager.instance.TeleportMap(destinationMapNum);
        player.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
        curMap.SetActive(false);
        nextMap.SetActive(true);
        destination.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // 플레이어가 포탈과 접촉한 상태
        {
            teleport = true;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // 플레이어가 포탈과 접촉하지 않은 상태
        {
            teleport = false;
        }
    }
}
