using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.U2D.Animation;

/* Class : Player
 * Description : ���� �� �÷��̾ ����ϴ� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ���� �浹 ó�� �Լ��� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class Player : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private GameObject MagicStone; // ������
    [SerializeField] private GameObject MagicStick; // ����������
    [SerializeField] private GameObject skill; // ��ų
    [SerializeField] private float speed = 5f; // �̵� �ӵ�
    [SerializeField] private int curCollidingNum = 0; // ���� �÷��̾�� �浹���� ��ü ��
    [SerializeField] private bool move = true; // �̵� ���� ����

    int directionX = 0; // ���� �ֱ� x�� �̵� ����
    int directionY = 0; // ���� �ֱ� y�� �̵� ����

    // �÷��̾� ������ ���� ������� �κ� -> ������ �и�? ����?
    int level = 1; // ����
    int curHp = 100; // ���� ü��
    int curMp = 100; // ���� ����
    int curExp = 0; // ���� ����ġ

    int skillMp = 10; // ��ų ��뿡 �ʿ��� ���� ��ġ
    float x = 0; // ���� x�� �̵� ����
    float y = 0; // ���� y�� �̵� ����
    float wait = 0; // ��ų ��� �� ������ �ܰ� ���� ���θ� ������ ���� ���� ������� �ð� ��
    float oneSecond = 0; // 1�� ����
    float skReady = 0.3f; // ��ų ��� �� ��ų�� �����Ǳ���� �ʿ��� �ð�
    float skTime = 0.5f; // ��ų ��ȿ �ð�
    float remainCool = 0; // ���� ��ų ��Ÿ�� �ð�
    float notHurtTime = 1f; // ���� ���� �ð�
    bool flip = false; // �¿� �������� ���� ����
    bool npcContact = false; // npc�� ���� ����
    bool tableContact = false; // ������ ���� ������ ���̺�� ���� ����
    bool skillUse = false; // ��ų ��� ����
    bool skOn = false; // ��ų Ȱ��ȭ ����
    bool getHurt = false; // ��ģ ���� ����
    bool actLock = false; // �÷��̾��� ������, ��ų ���� ���� �ൿ ���� ����

    IEnumerator hurtPeriod; // HurtEvent �ڷ�ƾ ����
    WaitForSeconds wfs; // �ڷ�ƾ���� ������� �����ְ� ��ٸ��� �ð�

    SpriteResolver spResolver; // ����Ƽ ĳ���� ��Ų ���� ��ü � ���Ǵ� SpriteResolver ������Ʈ
    Rigidbody2D rb; // ����Ƽ���� ��ü�� �����̵��� �ʿ��� RigidBody ������Ʈ
    Animator ani; // ����Ƽ �ִϸ��̼� ������Ʈ
    SortingGroup sg; // ����Ƽ���� ����ü�� �ϳ��� ���̾ �������� �ؼ� �׷����� �����ϰ��� �� �� ����ϴ� SortingGroup ������Ʈ
    Skill sk; // ��ų Ŭ���� ��ü

    void Start()
    {
        this.hurtPeriod = HurtEvent(); // �ڷ�ƾ �Ҵ�
        this.wfs = new WaitForSeconds(notHurtTime); // ���� �ð� ����
        this.spResolver = GetComponent<SpriteResolver>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� SpriteResolver ������Ʈ�� �����ɴϴ�.
        this.rb = GetComponent<Rigidbody2D>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� Rigidbody2D ������Ʈ�� �����ɴϴ�.
        this.ani = GetComponent<Animator>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� Animator ������Ʈ�� �����ɴϴ�.
        this.sg = GetComponent<SortingGroup>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� SortingGroup ������Ʈ�� �����ɴϴ�.
        GameManager.instance.unLockAct += UnLockAct; // gm�� delegate ������ unLockAct�� ���� Ŭ������ UnLockAct �Լ��� �Ҵ��մϴ�.
        GameManager.instance.setMagic += SetMagicStone; // gm�� delegate ������ setMagic�� ���� Ŭ������ SetMagicStone �Լ��� �Ҵ��մϴ�.
    }

    void Update()
    {
        GameManager.instance.PlayerPos = transform.position; // �÷��̾� ��ġ �ǽð� ����
        EnteringKey();
        if (GameManager.instance.SkDisable) // ��ų ��� �� ��ų ��Ÿ���� ���� ������ ��ų ����� �Ұ����� ����
        {
            WaitForCoolTimeDone();
        }
        if (skillUse) // ��ų ����� ��Ȳ
        {
            CheckSkillExistedTime();
        }
        if (GameManager.instance.CurQuestNum == 2) // 1�� ����Ʈ �Ϸ� �ĺ��� ���������̰� Ȱ��ȭ �ǵ��� ����
        {
            SetMagicStick();
        }
    }

    void FixedUpdate()
    {
        MoveEvent();
    }

    /* Method : EnteringKey
     * Description : �÷��̾ Ű �Է� �� � Ű�� �Է��ߴ��� ������ �´� ������ �����ϵ��� �ϴ� �޼����Դϴ�.
     * �̵� : �����¿� ����Ű, Npc�� ��ȭ or �� Ư�� ������Ʈ�� ��ȣ�ۿ� : Space ��, ��ų ��� : S Ű, ���� ��� ����Ű : A, DŰ
     * �̵� ����Ű �Է� ���¿� ���� �÷��̾��� ���� �̵� ����� �÷��̾ ���� �ֱٿ� �̵��� ���� ���� ���������� �����߽��ϴ�. 
     * S Ű�� ���� ��ų�� ����ϰ��� �� �� 1�� ����Ʈ�� �Ϸ���� �ʾ����� Ű�� ������ �ƹ��� ������ ������� �ʵ��� �߰�, �ش� ������ ����ϸ� gm�� ���� ��ų ��� �Ұ��� ���� ���� Ȯ�� ��
     * ���� ���� ��ġ�� ������ ��ų ��뿡 �ʿ��� ������ �����ִ��� ���θ� �˻��� �� ������ �� �����ϸ� ��ų ������ �����ϵ��� �����߽��ϴ�.
     * Return Value : void
     */
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
                actLock = true;
                DialogueEvent();
            }
            if (tableContact)
            {
                actLock = true;
                GameManager.instance.SelectMagicStoneUIOn();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (GameManager.instance.CurQuestNum == 1) return;
            if (!GameManager.instance.SkDisable)
            {
                curMp = GameManager.instance.CurMp;
                if (curMp >= skillMp)
                {
                    SkillEvent();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PotionEvent(0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PotionEvent(1);
        }
    }

    /* Method : MoveEvent
     * Description : �÷��̾��� �̵� ������ �����ϴ� �޼����Դϴ�. �̵� ���¿� ���⿡ ���� �˸��� �ִϸ��̼��� ����ǵ��� �����߽��ϴ�.
     * move ������ �÷��̾ ��ų ��� �� ��� �غ����� �ϴ� ���� �������� ���ϰ� �ϱ� ���� ����߽��ϴ�. actLock�� true ���¸� �ӵ��� 0���� �����ϰ� ���� �ִϸ��̼��� ����ǰ� �����߽��ϴ�.
     * Return Value : void
     */
    void MoveEvent()
    {
        if (!move)
        {
            rb.velocity = new Vector2(0, 0).normalized * speed;
            return;
        }
        if (actLock)
        {
            rb.velocity = new Vector2(0, 0).normalized * speed;
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

    /* Method : SkillEvent
     * Description : �÷��̾ ��ų ��� �� ��ų ������ �����ϴ� �޼����Դϴ�. ��ų ��� ���� �ִϸ��̼��� ����ǵ��� �ϰ� ��� ������ �������� ���� ��Ÿ���� �����ϵ��� �����߽��ϴ�.
     * ���� ���� ����Ʈ �� ��ų ��� ���ο� ���� ����Ʈ�� �����ؼ� gm�� Accept�� ���� ���� �÷��̾ ���� �������� ����Ʈ�� �ִٴ� �ǹ��̹Ƿ� �� ��� QuestUpdate �Լ��� "�������"�̶� �޽����� ����
     * ����Ʈ ���ǿ� �ش��ϴ� ������ �����ߴ����� �� �� �ֵ��� �����߽��ϴ�.
     * Return Value : void
     */
    void SkillEvent()
    {
        skillUse = true;
        if (GameManager.instance.Accept) // �������� ����Ʈ�� ���� �� � �ൿ�� �Ҷ����� ����Ʈ ���� �˻� �Լ��� ������ �� �ൿ�� �ش�Ǵ��� �˻�
        {
            GameManager.instance.QuestUpdate("�������");
        }
        GameManager.instance.SetSkillDisable();
        skill.SetActive(true);
        remainCool = sk.CoolTime;
        wait = 0;
        sk.Setting(transform.position.x, transform.position.y, directionX, directionY);
        ani.SetTrigger("skill");
        skOn = true;
        GameManager.instance.MpDown(skillMp);
        GameManager.instance.CheckCoolTime(remainCool);
    }

    /* Method : DialogueEvent
     * Description : �÷��̾ npc�� ��ȭ�ϴ� ������ �����ϴ� �޼����Դϴ�. gm���� npc�� ��ȭ�ϰڴٴ� �ǻ�ǥ������ TalkEvent �޼��带 ȣ���ϴ� ������� �����߽��ϴ�.
     * Return Value : void
     */
    void DialogueEvent()
    {
        GameManager.instance.TalkEvent();
    }

    /* Method : PotionEvent
     * Description : �÷��̾ ����Ű A�Ǵ� D�� Ŭ���Ͽ� ������ ����ϴ� ��� ���� ������ �����ϴ� �޼����Դϴ�.
     * Parameter : int idx - ��ġ(A Ű : 0, D Ű : 1)
     * Return Value : void
     */
    void PotionEvent(int idx)
    {
        if (GameManager.instance.CurShortCutPotions[idx] != -1)
        {
            if (GameManager.instance.InvenManager.FindItem(GameManager.instance.CurShortCutPotions[idx]) != -1)
            {
                GameManager.instance.CurUsedItemLoc = GameManager.instance.InvenManager.FindItem(GameManager.instance.CurShortCutPotions[idx]);
                GameManager.instance.UsePotion();
            }
        }
    }

    /* Method : SetMagicStone
     * Description : �÷��̾ ������ ���� �� �ش� �������� �´� ��ų�� �Ű������� �޾Ƽ� ���� ��� ������ ��ų�� �Ҵ��ϴ� �޼����Դϴ�.
     * Parameter : GameObject sk - ��ų(���� ������Ʈ ����)
     * Return Value : void
     */
    void SetMagicStone(GameObject sk)
    {
        skill = sk;
        this.sk = skill.GetComponent<Skill>();
        if(GameManager.instance.CurQuestNum >= 2) SetMagicStick();
    }

    /* Method : SetMagicStick
     * Description : �÷��̾ ������ ���� �� ���� ������ ����� �˸°� ����ǵ��� �ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    void SetMagicStick() 
    {
        spResolver = MagicStone.GetComponent<SpriteResolver>();
        if (GameManager.instance.WaterSelected)
        {
            spResolver.SetCategoryAndLabel("MagicStone", "water");
        }
        else if (GameManager.instance.DirtSelected)
        {
            spResolver.SetCategoryAndLabel("MagicStone", "dirt");
        }
        else if (GameManager.instance.WindSelected)
        {
            spResolver.SetCategoryAndLabel("MagicStone", "wind");
        }
        spResolver = MagicStick.GetComponent<SpriteResolver>();
        spResolver.SetCategoryAndLabel("MagicStick", "hold");
    }

    /* Method : CheckSkillExistedTime
     * Description : ��ų ��� �������� ��ų�� ��Ȱ��ȭ �� ������ ���� � �ܰ������� ���� �˸��� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    void CheckSkillExistedTime()
    {
        if (sk.AttackSuccess)
        {
            skOn = false;
            wait = 0;
            sk.AttackSuccess = false;
        }
        if (skOn)
        {
            wait += Time.deltaTime;
            if (wait > skReady)
            {
                wait = 0;
                sk.Attack = true;
                skOn = false;
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

    /* Method : WaitForCoolTimeDone
     * Description : ��ų ��Ÿ���� ���� ������ 1�� ������ ���� ��Ÿ���� �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    void WaitForCoolTimeDone()
    {
        if (remainCool > 0)
        {
            oneSecond += Time.deltaTime;
            if (oneSecond > 1f)
            {
                remainCool -= 1f;
                GameManager.instance.CheckCoolTime(remainCool);
                oneSecond = 0;
            }
        }
        else
        {
            GameManager.instance.ResetSkillDisable();
        }
    }

    //�ش� �±׸� ���� ��ü�� �浹 ���¸� �÷��̾��� ���̾� ���� �� ���� �����ϴ� ��� -> ���� ���� �ʿ�(���̾� ���� ��õǾ ��������?)
    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "Gold")
        {
            GameManager.instance.GoldIncrease(Other.gameObject.GetComponent<Gold>().GoldNum);
            Other.gameObject.SetActive(false);
        }
        if (Other.gameObject.tag == "Item")
        {
            Item item = Other.gameObject.GetComponent<Item>();
            GameManager.instance.GetItem(item, 1);
            Other.gameObject.SetActive(false);
        }
        if (Other.gameObject.tag == "MagicStoneTable")
        {
            tableContact = true;
        }
        if (Other.gameObject.tag == "MOP" || Other.gameObject.tag == "attack")
        {
            if (getHurt == false)
            {
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); // ���ݹ��� �������� ��¦ �˹��ϴ� ����
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                GameManager.instance.HpDown(GameManager.instance.MonsterPower); // ������ ���ݷ� ��ŭ ü�� ����
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

    /* Coroutine : HurtEvent
     * Description : �÷��̾ ������ �� ������ �����ϴ� �ڷ�ƾ�Դϴ�. ���� ü���� ������ ���ҽ�Ű�� ��ģ ������ �ִϸ��̼��� ����ǰ� �ϰ� �����س��� ���� �ð����� ��ģ ���°� ���� �ʵ��� �����߽��ϴ�.
     */
    IEnumerator HurtEvent()
    {
        getHurt = true;
        ani.SetTrigger("hurt");
        yield return wfs;
        getHurt = false;
        yield break;
    }

    /* Method : UnLockAct
     * Description : �÷��̾��� �ൿ ������ Ǯ���ִ� �޼����Դϴ�.
     * Return Value : void
     */
    public void UnLockAct()
    {
        actLock = false;
    }
}
