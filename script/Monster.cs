using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject manager;
    public GameObject player;
    public GameObject Bar;
    public Image HPbar;
    //public bool end;

    MonsterManager mm;
    Rigidbody2D rb;
    Animator ani;
    bool wait = false;
    bool ON = false;
    bool gethurt = false;
    bool end;
    int attackarea = 8;
    int mid = 0;
    float notAttackTime = 2f;
    private IEnumerator attackPeriod;
    private WaitForSeconds wfs;

    void Start()
    {
        this.end = false;
        this.mm = manager.GetComponent<MonsterManager>();
        this.attackPeriod = AttackWait();
        this.wfs = new WaitForSeconds(notAttackTime);
        this.rb = GetComponent<Rigidbody2D>();
        this.ani = GetComponent<Animator>();
    }

    void Update()
    {
        AttackEvent();
        if (end)
        {
            mm.DieEvent(mid);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "skill")
        {
            if (!gethurt)
            {
                gethurt = true;
                ani.SetTrigger("hurt");
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); // µÚ·Î ³Ë¹é
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                HurtEvent();
            }
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "skill")
        {
            gethurt = false;
        }
    }

    void AttackEvent()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (((player.transform.position.x > transform.position.x - attackarea) && (player.transform.position.x < transform.position.x + attackarea)) &&
            ((player.transform.position.y > transform.position.y - attackarea) && (player.transform.position.y < transform.position.y + attackarea)))
        {
            if (!wait)
            {
                ani.SetTrigger("attack");
                wait = true;
                StartCoroutine(AttackWait());
            }
        }
    }
    void HurtEvent()
    {
        if (!ON)
        {
            Bar.SetActive(true);
            ON = true;
        }
        if(HPbar.fillAmount - 0.4f > 0)
        {
            HPbar.fillAmount -= 0.4f;
        }
        else
        {
            Bar.SetActive(false);
            ani.SetTrigger("die");
        }
    }

    IEnumerator AttackWait()
    {
        yield return wfs;
        wait = false;
    }

    public bool End
    {
        get
        {
            return end;
        }
        set
        {
            end = value;
        }
    }
}
