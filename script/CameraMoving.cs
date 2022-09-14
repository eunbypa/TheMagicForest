using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMoving : MonoBehaviour
{
    //[SerializeField] private Tilemap[] tilemap;
    [SerializeField] private Vector3[] minMapPos;
    [SerializeField] private Vector3[] maxMapPos;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject GM;
    //public float[] maxX;
    //public float[] minX;
    //public float[] maxY;
    //public float[] minY;
    //private Camera camera;
    private int curMapNum;
    private Vector3 playerPos;
    private GameManager gm;
    private float height;
    private float width;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    void Start()
    {
        this.gm = GM.GetComponent<GameManager>();
        this.curMapNum = gm.CurMapNum;
        //this.camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //this.camera = Camera.main;
        //this.height = 2f * camera.orthographicSize;
        //this.width = height * camera.aspect;
        SetLimit();
        //minX = maps[curMapNum].cellToWorld(maps[curMapNum].cellBounds.xMin);
        //maxX = maps[curMapNum].cellToWorld(maps[curMapNum].cellBounds.xMin);
        CheckPlayerPos();
    }

    void Update()
    {
        Moving();
    }

    void SetLimit() // 카메라 이동범위 제한 세팅
    {
        /*Vector3 minTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.min);
        Vector3 maxTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.max);
        minX = minTile.x + width/2;
        maxX = maxTile.x - width/2;
        minY = minTile.y + height/2;
        maxY = maxTile.y - height/2;*/
        minX = minMapPos[curMapNum].x;
        maxX = maxMapPos[curMapNum].x;
        minY = minMapPos[curMapNum].y;
        maxY = maxMapPos[curMapNum].y;
    }

    void CheckPlayerPos() // 플레이어의 현재 위치가 카메라 이동범위를 벗어났는지 체크
    {
        playerPos = this.player.transform.position;
        if(playerPos.x >= minX && playerPos.x <= maxX)
        {
            transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
        }
        if(playerPos.y >= minY && playerPos.y <= maxY)
        {
            transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
        }
        if (playerPos.x < minX)
        {
            transform.position = new Vector3(minX, playerPos.y, transform.position.z);
        }
        if (playerPos.x > maxX)
        {
            transform.position = new Vector3(maxX, playerPos.y, transform.position.z);
        }
        if (playerPos.y < minY)
        {
            transform.position = new Vector3(playerPos.x, minY, transform.position.z);
        }
        if (playerPos.y > maxY)
        {
            transform.position = new Vector3(playerPos.x, maxY, transform.position.z);
        }
    }

    void Moving()
    {
        if (this.curMapNum != gm.CurMapNum)
        {
            this.curMapNum = gm.CurMapNum;
            SetLimit();
        }
        CheckPlayerPos();
    }
}
