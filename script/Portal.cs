using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject GM;
    public GameObject player;
    public GameObject destination;
    public GameObject CurMap;
    public GameObject NextMap;

    //public int departuremapnum;
    //public int destinationmapnum;

    int departureMapNum;
    int destinationMapNum;
    bool teleport = false;
    GameManager gm;

    void Start()
    {
        this.gm = GM.GetComponent<GameManager>();
    }

    void Update()
    {
        if (teleport)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                TeleportEvent();
            }
        }
    }

    /*public int DepartureMapNum {
        get
        {
            return departureMapNum;
        }
    
    }

    public int DestinationMapNum;
    {
        get
        {
            return departureMapNum;
        }

    }*/

    public void TeleportEvent()
    {
        gm.TeleportMap(destinationMapNum);
        player.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
        CurMap.SetActive(false);
        NextMap.SetActive(true);
        destination.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            teleport = true;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player")
        {
            teleport = false;
        }
    }
}
