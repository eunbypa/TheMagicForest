using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Class : CameraMoving
 * Description : 게임 화면 담당 카메라의 움직임을 관리하는 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class CameraMoving : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private Tilemap[] maps; // 유니티 타일맵 배열
    [SerializeField] private GameObject player; // 플레이어
    //[SerializeField] private GameObject gM; // 게임 관리자 GameManager

    int curMapNum; // 현재 맵 번호
    float height; // 높이
    float width; // 너비
    float maxX; // 최대 x값
    float minX; // 최소 x값
    float maxY; // 최대 y값
    float minY; // 최소 y값

    Camera camera; // 유니티 카메라 컴포넌트
    Vector3 playerPos; // 플레이어의 위치를 나타내는 3차원 벡터
    //GameManager gm; // 게임 관리자 GameManager 클래스 객체

    void Start()
    {
        //this.gm = gM.GetComponent<GameManager>(); // gM GameObject 객체에 할당된 GameManager 클래스 컴포넌트를 가져옵니다.
        this.curMapNum = GameManager.instance.CurMapNum; // gm에게서 플레이어가 현재 있는 맵 번호 값을 가져옵니다.
        this.camera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Main Camera 이름을 가지는 GameObject 객체에서 카메라 컴포넌트를 가져옵니다.
        this.height = 2f * camera.orthographicSize; // camera.orthograpicSize 는 카메라 영역의 세로 길이의 반을 나타내서 세로 길이, 즉 높이를 구하기 위해 거기에 2배를 다시 곱하는 방식으로 구현했습니다.
        this.width = height * camera.aspect; // 카메라 영역의 가로 길이, 너비는 세로 길이와 화면 비율에 비례하므로 높이에 카메라 화면 비율을 곱하는 방식으로 구현했습니다.
        SetLimit(); // 시작 시점에 카메라 이동 제한 범위를 세팅합니다.
        CheckPlayerPos(); // 시작 시점에 플레이어의 위치가 위에서 설정한 제한 범위를 벗어났는지 검사합니다.
    }

    void Update()
    {
        Moving();
    }

    /* Method : SetLimit
     * Description : 카메라의 이동 범위를 제한하는 동작을 수행하는 메서드입니다. 현재 맵 번호를 기준으로 해당 타일맵의 최소 타일 위치와 최대 타일 위치를 파악한 다음 카메라의 영역이 맵을 벗어나지 않게 하기 위해
     * 미리 계산한 높이와 너비 값을 이용해서 카메라 위치의 x축과 y축 최대 최소를 설정합니다.
     * Return Value : void
     */
    void SetLimit()
    {
        Vector3 minTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.min);
        Vector3 maxTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.max);
        minX = minTile.x + width / 2;
        maxX = maxTile.x - width / 2;
        minY = minTile.y + height / 2;
        maxY = maxTile.y - height / 2;
    }

    /* Method : CheckPlayerPos
     * Description : 플레이어의 현재 위치를 파악해서 카메라의 이동 범위에서 벗어났는지 검사하는 메서드입니다. 벗어나지 않았으면 플레이어의 위치를 그대로 카메라 위치로 설정합니다.
     * 만약 x 또는 y 값이 제한 범위를 벗어난 경우 벗어난 값만 제한 범위의 최소 또는 최대 값으로 고정하고 카메라 위치를 설정하는 방식으로 구현했습니다.
     * Return Value : void
     */
    void CheckPlayerPos()
    {
        playerPos = this.player.transform.position;
        if (playerPos.x >= minX && playerPos.x <= maxX)
        {
            transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
        }
        if (playerPos.y >= minY && playerPos.y <= maxY)
        {
            transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
        }
        if (playerPos.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (playerPos.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (playerPos.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        if (playerPos.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }

    /* Method : Moving
     * Description : 플레이어의 움직임에 맞춰 카메라도 움직이도록 하는 메서드입니다. 만약 이동 중 플레이어가 포탈을 이용해서 현재 맵 번호가 변경되는 경우 해당 맵 번호로 바꾸고 그 맵에 알맞은 이동 제한 범위를
     * 가지도록 SetLimit 메서드를 호출하는 방식으로 구현했습니다.
     * Return Value : void
     */
    void Moving()
    {
        if (this.curMapNum != GameManager.instance.CurMapNum)
        {
            this.curMapNum = GameManager.instance.CurMapNum;
            SetLimit();
        }
        CheckPlayerPos();
    }
}
