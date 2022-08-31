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

    int curmapnum;
    Vector3 playerPos;
    GameManager gm;

    void Start()
    {
        this.gm = GM.GetComponent<GameManager>();
        this.curmapnum = gm.curmapnum;
        playerPos = this.player.transform.position;
        if (playerPos.x < minX[this.curmapnum])
        {
            transform.position = new Vector3(minX[this.curmapnum], playerPos.y, transform.position.z);
        }
        if (playerPos.x > maxX[this.curmapnum])
        {
            transform.position = new Vector3(maxX[this.curmapnum], playerPos.y, transform.position.z);
        }
        if (playerPos.y < minY[this.curmapnum])
        {
            transform.position = new Vector3(playerPos.x, minY[this.curmapnum], transform.position.z);
        }
        if (playerPos.y > maxY[this.curmapnum])
        {
            transform.position = new Vector3(playerPos.x, maxY[this.curmapnum], transform.position.z);
        }
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (this.curmapnum != gm.curmapnum)
        {
            this.curmapnum = gm.curmapnum;
            playerPos = this.player.transform.position;
            if (playerPos.x < minX[this.curmapnum])
            {
                transform.position = new Vector3(minX[this.curmapnum], playerPos.y, transform.position.z);
            }
            if (playerPos.x > maxX[this.curmapnum])
            {
                transform.position = new Vector3(maxX[this.curmapnum], playerPos.y, transform.position.z);
            }
            if (playerPos.y < minY[this.curmapnum])
            {
                transform.position = new Vector3(playerPos.x, minY[this.curmapnum], transform.position.z);
            }
            if (playerPos.y > maxY[this.curmapnum])
            {
                transform.position = new Vector3(playerPos.x, maxY[this.curmapnum], transform.position.z);
            }
        }
        else
        {
            playerPos = this.player.transform.position;
            if (playerPos.x > minX[this.curmapnum] && playerPos.x < maxX[this.curmapnum])
            {
                transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
            }
            if (playerPos.y > minY[this.curmapnum] && playerPos.y < maxY[this.curmapnum])
            {
                transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
            }
        }
    }
}
