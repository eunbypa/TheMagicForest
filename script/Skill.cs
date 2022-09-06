using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{    
    //public bool attack = false;
   // public bool finish = false;
   // public bool attackSuccess = false;

    //public int goX = 0;
    //public int goY = 0;

    //public float coolTime = 2f;

    Animator ani;
    Rigidbody2D rb;
    int goX = 0;
    int goY = 0;
    float coolTime = 2f;
    bool attack = false;
    bool finish = false;
    bool attackSuccess = false;

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.ani = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Moving();
    }

    void Moving ()
    {
        if (attack)
        {
            ani.SetTrigger("set");
            rb.velocity = new Vector2(goX, goY).normalized * 20;
        }
        if (finish)
        {
            ani.SetTrigger("reset");
            rb.velocity = new Vector2(0, 0) * 0;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "MOP")
        {
            attackSuccess = true;
            attack = false;
            finish = true;

        }
    }

    public float CoolTime
    {
        get
        {
            return coolTime;
        }
        set
        {
            coolTime = value;
        }
    }

    public bool Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public bool AttackSuccess
    {
        get
        {
            return attackSuccess;
        }
        set
        {
            attackSuccess = value;
        }
    }

    public bool Finish
    {
        get
        {
            return finish;
        }
        set
        {
            finish = value;
        }
    }

    public void setting(float x, float y, int directionX, int directionY)
    {
        this.transform.position = new Vector2(x, y + 3);
        if ((directionX == 0) && (directionY == 0))
        {
            goX = -1;
            goY = 0;
        }
        else
        {
            goX = directionX;
            goY = directionY;
        }
    }

}
