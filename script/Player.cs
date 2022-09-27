using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.U2D.Animation;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject MagicStone;
    [SerializeField] private GameObject MagicStick;
    [SerializeField] private GameObject skill;
    [SerializeField] private GameObject gM;
    [SerializeField] private float speed = 5f;
    //[SerializeField] private int talking = 0;
    [SerializeField] private int curCollidingNum = 0;
    [SerializeField] private bool moving = true;

    int directionX = 0;
    int directionY = 0;
    int level = 1;
    int curHp = 100;
    int curMp = 100;
    int curExp = 0;
    int skillMp = 10;
    float x = 0;
    float y = 0;
    float wait = 0;
    //float waitportDone = 0;
    float oneSecond = 0;
    //float portTime = 0.1f;
    float skReady = 0.3f;
    float skTime = 0.5f;
    float remainCool = 0;
    float notHurtTime = 1f;
    bool flip = false;
    bool npcContact = false;
    bool tableContact = false;
    bool skillUse = false;
    bool skON = false;
    bool getHurt = false;

    private IEnumerator hurtPeriod;
    private WaitForSeconds wfs;

    SpriteResolver spResolver;
    Rigidbody2D rb;
    Animator ani;
    SortingGroup sg;
    GameManager gm;
    InventoryManager im;
    Skill sk;

    void Start()
    {
        this.hurtPeriod = HurtEvent();
        this.wfs = new WaitForSeconds(notHurtTime);
        //this.sk = Skill.GetComponent<Skill>(); // 퀘스트 1번 시작 전이면 비활성화 해야함
        this.spResolver = GetComponent<SpriteResolver>();
        this.rb = GetComponent<Rigidbody2D>();
        this.ani = GetComponent<Animator>();
        this.sg = GetComponent<SortingGroup>();
        this.gm = gM.GetComponent<GameManager>();
        gm.setMagic += SetMagicStone;
    }

    void Update()
    {
        EnteringKey();
        if (gm.SkDisable)
        {
            WaitForCoolTimeDone();
        }
        if (skillUse)
        {
            CheckSkillExistedTime();
            /*if (sk.AttackSuccess)
            {
                skON = false;
                wait = 0;
                sk.AttackSuccess = false;
            }
            if (skON)
            {
                wait += Time.deltaTime;
                if (wait > skReady)
                {
                    wait = 0;
                    sk.Attack = true;
                    skON = false;
                }
            }
            if (sk.Attack)
            {
                wait += Time.deltaTime;
                if (wait > skTime)
                {
                    wait = 0;
                    sk.Finish = true;
                    sk.Attack = false;
                }
            }
            if (sk.Finish)
            {
                wait += Time.deltaTime;
                if (wait > skTime)
                {
                    wait = 0;
                    skill.SetActive(false);
                    sk.Finish = false;
                    skillUse = false;
                }
            }*/
        }
        if(gm.CurQuestNum == 2)
        {
            SetMagicStick();
        }
        /*if (x == 0 && y == 0)
        {
            ani.SetTrigger("stop");
        }
        else
        {
            ani.SetTrigger("walk");
            if (x == 1) 
            {
                if (flip == false)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    flip = true;
                }
            }
            if (x == -1) 
            {
                if (flip == true)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    flip = false;
                }
            }
        }*/
    }

    void FixedUpdate()
    {
        MoveEvent();
    }

    void EnteringKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            x = -1;
            directionX = -1;
            directionY = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            x = 1;
            directionX = 1;
            directionY = 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            y = 1;
            directionY = 1;
            directionX = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            y = -1;
            directionY = -1;
            directionX = 0;

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            directionX = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            directionX = 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            directionY = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            directionY = -1;
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                x = 0;
                if (directionY != 0)
                {
                    directionX = 0;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            y = 0;
            if (directionX != 0)
            {
                directionY = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (npcContact)
            {
                DialogueEvent();
            }
            if (tableContact) gm.SelectMagicStoneUIOn();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gm.CurQuestNum == 1) return; // 아직 스킬을 사용할 수 없는 상태
            if (!(gm.SkDisable))
            {
                curMp = gm.CurMp;
                if (curMp != 0) // 정확히는 현재 스킬 사용에 필요한 mp 양만큼 존재하는지 체크해야함
                {
                    SkillEvent();
                }
            }
        }
    }
    void MoveEvent()
    {
        if (!moving)
        {
            rb.velocity = new Vector2(0, 0).normalized * 0;
            ani.SetTrigger("stop");
            return;
        }
        if (x == 0 && y == 0)
        {
            ani.SetTrigger("stop");
        }
        else
        {
            ani.SetTrigger("walk");
            if (x == 1)
            {
                if (flip == false)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    flip = true;
                }
            }
            if (x == -1)
            {
                if (flip == true)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    flip = false;
                }
            }
        }
        rb.velocity = new Vector2(x, y).normalized * speed;
    }
    void SkillEvent()
    {
        skillUse = true;
        if (gm.Accept) // 진행중인 퀘스트가 있을 때 어떤 행동을 할때마다 퀘스트 조건 검사 함수에 전달해 이 행동이 해당되는지 검사
        {
            gm.QuestUpdate("마법사용");
        }
        gm.SetSkillDisable();
        skill.SetActive(true);
        remainCool = sk.CoolTime;
        wait = 0;
        sk.setting(transform.position.x, transform.position.y, directionX, directionY);
        ani.SetTrigger("skill");
        skON = true;
        gm.MPDown(skillMp);
        gm.CheckCoolTime(remainCool); 
    }
    void DialogueEvent()
    {
        gm.TalkEvent();
        /*if (gm.Istalking)
        {
            gm.SkipTalk(talking);
        }
        else
        {
            talking++;
            gm.TalkOn(talking);
        }
        if (gm.FinishTalk)
        {
            talking = 0;
            gm.FinishTalk = false;
        }*/
    }
    void SetMagicStone(GameObject sk)
    {
        skill = sk;
        this.sk = skill.GetComponent<Skill>();
    }
    void SetMagicStick() // 다른 마법석 상태도 추가해야 함 아직 바람만 추가한 상태
    {
        spResolver = MagicStone.GetComponent<SpriteResolver>();
        spResolver.SetCategoryAndLabel("MagicStone", "wind");
        spResolver = MagicStick.GetComponent<SpriteResolver>();
        spResolver.SetCategoryAndLabel("MagicStick", "hold");
    }
    void CheckSkillExistedTime()
    {
        if (sk.AttackSuccess)
        {
            skON = false;
            wait = 0;
            sk.AttackSuccess = false;
        }
        if (skON)
        {
            wait += Time.deltaTime;
            if (wait > skReady)
            {
                wait = 0;
                sk.Attack = true;
                skON = false;
            }
        }
        if (sk.Attack)
        {
            wait += Time.deltaTime;
            if (wait > skTime)
            {
                wait = 0;
                sk.Finish = true;
                sk.Attack = false;
            }
        }
        if (sk.Finish)
        {
            wait += Time.deltaTime;
            if (wait > skTime)
            {
                wait = 0;
                skill.SetActive(false);
                sk.Finish = false;
                skillUse = false;
            }
        }
    }
    void WaitForCoolTimeDone()
    {
        if (remainCool > 0)
        {
            oneSecond += Time.deltaTime;
            if (oneSecond > 1f)
            {
                remainCool -= 1f;
                gm.CheckCoolTime(remainCool);
                oneSecond = 0;
            }
        }
        else
        {
            gm.ResetSkillDisable();
        }
    }
    public void GetItem(Item item, int num)
    {

    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "MagicStoneTable")
        {
            tableContact = true;
        }
        if (Other.gameObject.tag == "MOP" || Other.gameObject.tag == "attack")
        {
            if (getHurt == false)
            {
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                hurtPeriod = HurtEvent();
                StartCoroutine(hurtPeriod);
            }
        }
        if (Other.gameObject.tag == "TREE0")
        {
            ++curCollidingNum;
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "TREE-1")
        {
            ++curCollidingNum;
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "TREE")
        {
            ++curCollidingNum;
            if(curCollidingNum == 1) 
            {
                sg.sortingOrder = -4;
            }
            else
            {
                if (sg.sortingOrder > -4)
                {
                    sg.sortingOrder = -4;
                }
            }
          
        }
        if (Other.gameObject.tag == "TREE2")
        {
            ++curCollidingNum;
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -8;
            }
            else
            {
                if (sg.sortingOrder > -8)
                {
                    sg.sortingOrder = -8;
                }
            }
        }
        if (Other.gameObject.tag == "HOUSE")
        {
            ++curCollidingNum;
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -6;
            }
            else
            {
                if (sg.sortingOrder > -6)
                {
                    sg.sortingOrder = -6;
                }
            }
        }
        if (Other.gameObject.tag == "HouseRoof")
        {
            ++curCollidingNum;
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "NPC")
        {
            ++curCollidingNum;
            npcContact = true;
            if (curCollidingNum == 1)
            {
                if (transform.position.y > Other.transform.position.y)
                {
                    sg.sortingOrder = -1;
                }
            }
            else
            {
                if (transform.position.y > Other.transform.position.y)
                {
                    if (sg.sortingOrder > -1)
                    {
                        sg.sortingOrder = -1;
                    }
                }
            }
        }

    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "TREE0")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "TREE-1")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "TREE")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -4;
            }
            else
            {
                if (sg.sortingOrder > -4)
                {
                    sg.sortingOrder = -4;
                }
            }
        }
        if (Other.gameObject.tag == "TREE2")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -8;
            }
            else
            {
                if (sg.sortingOrder > -8)
                {
                    sg.sortingOrder = -8;
                }
            }
        }
        if (Other.gameObject.tag == "HOUSE")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -6;
            }
            else
            {
                if (sg.sortingOrder > -6)
                {
                    sg.sortingOrder = -6;
                }
            }
        }
        if (Other.gameObject.tag == "HouseRoof")
        {
            if (curCollidingNum == 1)
            {
                sg.sortingOrder = -2;
            }
            else
            {
                if (sg.sortingOrder > -2)
                {
                    sg.sortingOrder = -2;
                }
            }
        }
        if (Other.gameObject.tag == "NPC")
        {
            npcContact = true;
            if (curCollidingNum == 1)
            {
                if (transform.position.y > Other.transform.position.y)
                {
                    sg.sortingOrder = -1;
                }
                else
                {
                    sg.sortingOrder = 0;
                }
            }
            else
            {
                if (transform.position.y > Other.transform.position.y)
                {
                    if (sg.sortingOrder > -1)
                    {
                        sg.sortingOrder = -1;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if ((Other.gameObject.tag == "TREE0") || (Other.gameObject.tag == "TREE") || (Other.gameObject.tag == "HOUSE") || (Other.gameObject.tag == "TREE2") || (Other.gameObject.tag == "HouseRoof") || 
            (Other.gameObject.tag == "TREE-1"))
        {
            --curCollidingNum;
            if (curCollidingNum == 0)
            {
                sg.sortingOrder = 0;
            }
        }
        if (Other.gameObject.tag == "NPC")
        {
            npcContact = false;
            --curCollidingNum;
            if (curCollidingNum == 0)
            {
                sg.sortingOrder = 0;
            }
        }
        if (Other.gameObject.tag == "MagicStoneTable")
        {
            tableContact = false;
        }
    }

    IEnumerator HurtEvent()
    {
        getHurt = true;
        gm.HPDown(5);
        ani.SetTrigger("hurt");
        yield return wfs;
        getHurt = false;
        yield break;
    }

  
}
