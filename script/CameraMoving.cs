using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject player;
    public GameObject GM;
    public float[] maxX;
    public float[] minX;
    public float[] maxY;
    public float[] minY;

    int curMapNum;
    Vector3 playerPos;
    GameManager gm;

    void Start()
    {
        this.gm = GM.GetComponent<GameManager>();
        this.curMapNum = gm.CurMapNum;
        playerPos = this.player.transform.position;
        if (playerPos.x < minX[this.curMapNum])
        {
            transform.position = new Vector3(minX[this.curMapNum], playerPos.y, transform.position.z);
        }
        if (playerPos.x > maxX[this.curMapNum])
        {
            transform.position = new Vector3(maxX[this.curMapNum], playerPos.y, transform.position.z);
        }
        if (playerPos.y < minY[this.curMapNum])
        {
            transform.position = new Vector3(playerPos.x, minY[this.curMapNum], transform.position.z);
        }
        if (playerPos.y > maxY[this.curMapNum])
        {
            transform.position = new Vector3(playerPos.x, maxY[this.curMapNum], transform.position.z);
        }
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (this.curMapNum != gm.CurMapNum)
        {
            this.curMapNum = gm.CurMapNum;
            playerPos = this.player.transform.position;
            if (playerPos.x < minX[this.curMapNum])
            {
                transform.position = new Vector3(minX[this.curMapNum], playerPos.y, transform.position.z);
            }
            if (playerPos.x > maxX[this.curMapNum])
            {
                transform.position = new Vector3(maxX[this.curMapNum], playerPos.y, transform.position.z);
            }
            if (playerPos.y < minY[this.curMapNum])
            {
                transform.position = new Vector3(playerPos.x, minY[this.curMapNum], transform.position.z);
            }
            if (playerPos.y > maxY[this.curMapNum])
            {
                transform.position = new Vector3(playerPos.x, maxY[this.curMapNum], transform.position.z);
            }
        }
        else
        {
            playerPos = this.player.transform.position;
            if (playerPos.x > minX[this.curMapNum] && playerPos.x < maxX[this.curMapNum])
            {
                transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
            }
            if (playerPos.y > minY[this.curMapNum] && playerPos.y < maxY[this.curMapNum])
            {
                transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
            }
        }
    }
}
