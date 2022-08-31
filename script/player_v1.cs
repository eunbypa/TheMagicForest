using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class player_v1 : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ani;
    SortingGroup sg;
    //public GameObject p;
    bool flip = false;
    public bool move = true;
    bool gethurt = false;

    float NotHurtTime = 1f;
    public float speed = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.ani = GetComponent<Animator>();
        this.sg = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            move = false;
            ani.SetTrigger("sk");
            StartCoroutine(SKILL());
        }

        if (move)
        {
            if (inputX == 0 && inputY == 0)
            {
                ani.SetTrigger("stop");
                //flip = false;
            }
            else
            {
                ani.SetTrigger("walk");
                if (inputX == 1) //¿À¸¥ÂÊ
                {
                    if (flip == false)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        //transform.localScale = new Vector3(-0.7f, 0.5f, 1);
                        //transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);
                        flip = true;
                    }
                }
                if (inputX == -1) //¿ÞÂÊ
                {
                    if (flip == true)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        //transform.localScale = new Vector3(0.7f, 0.5f, 1);
                        //transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);
                        flip = false;
                    }
                }
            }

            rb.velocity = new Vector2(inputX, inputY).normalized * speed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
       
    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "MOP")
        {
            if (gethurt == false) {
                gethurt = true;
                //move = false;
                ani.SetTrigger("hurt");
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); // µÚ·Î ³Ë¹é
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                StartCoroutine(HURT());
            }
           
            //ani.SetTrigger("hurt");
            //if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
            //else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
            //move = false;
        }
        if (Other.gameObject.tag == "OBJ")
        {
            sg.sortingOrder = -4;
        }

    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "OBJ")
        {
            sg.sortingOrder = -4;
        }
        if (Other.gameObject.tag == "MOP")
        {
            if (gethurt == false)
            {
                gethurt = true;
                //move = false;
                ani.SetTrigger("hurt");
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                StartCoroutine(HURT());
            }

            //ani.SetTrigger("hurt");
            //if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
            //else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
            //move = false;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        /*if (Other.gameObject.tag == "MOP")
        {
            //ani.SetTrigger("hurt");
            //if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);
            //else transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);
            //move = true;
        }*/
        if (Other.gameObject.tag == "OBJ")
        {
            sg.sortingOrder = 0;
        }

    }
    IEnumerator HURT()
    {
        yield return new WaitForSeconds(NotHurtTime);
        gethurt = false;
    }
    
    IEnumerator SKILL()
    {
        //move = false;
        yield return new WaitForSeconds(0.3f);
        move = true;
    }
}

