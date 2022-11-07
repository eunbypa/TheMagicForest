using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.U2D.Animation;

/* Class : Player
 * Description : 게임 속 플레이어를 담당하는 클래스입니다. 유니티의 생명 주기 함수들과 충돌 처리 함수를 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class Player : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private GameObject MagicStone; // 마법석
    [SerializeField] private GameObject MagicStick; // 마법지팡이
    [SerializeField] private GameObject skill; // 스킬
    [SerializeField] private float speed = 5f; // 이동 속도
    [SerializeField] private int curCollidingNum = 0; // 현재 플레이어와 충돌중인 물체 수
    [SerializeField] private bool move = true; // 이동 가능 여부

    int directionX = 0; // 가장 최근 x축 이동 방향
    int directionY = 0; // 가장 최근 y축 이동 방향

    // 플레이어 데이터 관련 고민중인 부분 -> 데이터 분리? 통합?
    int level = 1; // 레벨
    int curHp = 100; // 현재 체력
    int curMp = 100; // 현재 마력
    int curExp = 0; // 현재 경험치

    int skillMp = 10; // 스킬 사용에 필요한 마력 수치
    float x = 0; // 현재 x축 이동 방향
    float y = 0; // 현재 y축 이동 방향
    float wait = 0; // 스킬 사용 시 각각의 단계 진입 여부를 가리기 위한 현재 대기중인 시간 값
    float oneSecond = 0; // 1초 단위
    float skReady = 0.3f; // 스킬 사용 시 스킬이 생성되기까지 필요한 시간
    float skTime = 0.5f; // 스킬 유효 시간
    float remainCool = 0; // 남은 스킬 쿨타임 시간
    float notHurtTime = 1f; // 무적 상태 시간
    bool flip = false; // 좌우 뒤집어진 상태 여부
    bool npcContact = false; // npc와 접촉 여부
    bool tableContact = false; // 마법석 변경 가능한 테이블과 접촉 여부
    bool skillUse = false; // 스킬 사용 여부
    bool skOn = false; // 스킬 활성화 여부
    bool getHurt = false; // 다친 상태 여부
    bool actLock = false; // 플레이어의 움직임, 스킬 사용과 같은 행동 제한 여부

    IEnumerator hurtPeriod; // HurtEvent 코루틴 변수
    WaitForSeconds wfs; // 코루틴에서 제어권을 돌려주고 기다리는 시간

    SpriteResolver spResolver; // 유니티 캐릭터 스킨 파츠 교체 등에 사용되는 SpriteResolver 컴포넌트
    Rigidbody2D rb; // 유니티에서 객체의 물리이동에 필요한 RigidBody 컴포넌트
    Animator ani; // 유니티 애니메이션 컴포넌트
    SortingGroup sg; // 유니티에서 복합체를 하나의 레이어를 기준으로 해서 그룹으로 정렬하고자 할 때 사용하는 SortingGroup 컴포넌트
    Skill sk; // 스킬 클래스 객체

    void Start()
    {
        this.hurtPeriod = HurtEvent(); // 코루틴 할당
        this.wfs = new WaitForSeconds(notHurtTime); // 무적 시간 설정
        this.spResolver = GetComponent<SpriteResolver>(); // 현재 클래스가 할당된 GameObject 객체에서 SpriteResolver 컴포넌트를 가져옵니다.
        this.rb = GetComponent<Rigidbody2D>(); // 현재 클래스가 할당된 GameObject 객체에서 Rigidbody2D 컴포넌트를 가져옵니다.
        this.ani = GetComponent<Animator>(); // 현재 클래스가 할당된 GameObject 객체에서 Animator 컴포넌트를 가져옵니다.
        this.sg = GetComponent<SortingGroup>(); // 현재 클래스가 할당된 GameObject 객체에서 SortingGroup 컴포넌트를 가져옵니다.
        GameManager.instance.unLockAct += UnLockAct; // gm의 delegate 변수인 unLockAct에 현재 클래스의 UnLockAct 함수를 할당합니다.
        GameManager.instance.setMagic += SetMagicStone; // gm의 delegate 변수인 setMagic에 현재 클래스의 SetMagicStone 함수를 할당합니다.
    }

    void Update()
    {
        GameManager.instance.PlayerPos = transform.position; // 플레이어 위치 실시간 갱신
        EnteringKey();
        if (GameManager.instance.SkDisable) // 스킬 사용 후 스킬 쿨타임이 끝날 때까지 스킬 사용이 불가능한 상태
        {
            WaitForCoolTimeDone();
        }
        if (skillUse) // 스킬 사용한 상황
        {
            CheckSkillExistedTime();
        }
        if (GameManager.instance.CurQuestNum == 2) // 1번 퀘스트 완료 후부터 마법지팡이가 활성화 되도록 구현
        {
            SetMagicStick();
        }
    }

    void FixedUpdate()
    {
        MoveEvent();
    }

    /* Method : EnteringKey
     * Description : 플레이어가 키 입력 시 어떤 키를 입력했는지 각각에 맞는 동작을 수행하도록 하는 메서드입니다.
     * 이동 : 상하좌우 방향키, Npc와 대화 or 맵 특정 오브젝트와 상호작용 : Space 바, 스킬 사용 : S 키, 포션 사용 단축키 : A, D키
     * 이동 방향키 입력 상태에 따라 플레이어의 현재 이동 방향과 플레이어가 가장 최근에 이동한 방향 값을 가져오도록 구현했습니다. 
     * S 키를 눌러 스킬을 사용하고자 할 때 1번 퀘스트가 완료되지 않았으면 키를 눌러도 아무런 동작이 수행되지 않도록 했고, 해당 조건을 통과하면 gm을 통해 스킬 사용 불가능 상태 여부 확인 후
     * 현재 마력 수치를 가져와 스킬 사용에 필요한 마력이 남아있는지 여부를 검사해 두 조건을 다 만족하면 스킬 동작을 수행하도록 구현했습니다.
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
     * Description : 플레이어의 이동 동작을 수행하는 메서드입니다. 이동 상태와 방향에 따라 알맞은 애니메이션이 실행되도록 구현했습니다.
     * move 변수는 플레이어가 스킬 사용 시 잠시 준비동작을 하는 동안 움직이지 못하게 하기 위해 사용했습니다. actLock이 true 상태면 속도를 0으로 설정하고 정지 애니메이션이 실행되게 구현했습니다.
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
     * Description : 플레이어가 스킬 사용 시 스킬 동작을 수행하는 메서드입니다. 스킬 사용 동작 애니메이션이 실행되도록 하고 사용 시점을 기준으로 남은 쿨타임을 설정하도록 구현했습니다.
     * 제가 만든 퀘스트 중 스킬 사용 여부에 대한 퀘스트가 존재해서 gm의 Accept가 참일 때는 플레이어가 현재 진행중인 퀘스트가 있다는 의미이므로 이 경우 QuestUpdate 함수에 "마법사용"이란 메시지를 보내
     * 퀘스트 조건에 해당하는 동작을 수행했는지를 알 수 있도록 구현했습니다.
     * Return Value : void
     */
    void SkillEvent()
    {
        skillUse = true;
        if (GameManager.instance.Accept) // 진행중인 퀘스트가 있을 때 어떤 행동을 할때마다 퀘스트 조건 검사 함수에 전달해 이 행동이 해당되는지 검사
        {
            GameManager.instance.QuestUpdate("마법사용");
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
     * Description : 플레이어가 npc와 대화하는 동작을 수행하는 메서드입니다. gm에게 npc와 대화하겠다는 의사표현으로 TalkEvent 메서드를 호출하는 방식으로 구현했습니다.
     * Return Value : void
     */
    void DialogueEvent()
    {
        GameManager.instance.TalkEvent();
    }

    /* Method : PotionEvent
     * Description : 플레이어가 단축키 A또는 D를 클릭하여 포션을 사용하는 경우 관련 동작을 수행하는 메서드입니다.
     * Parameter : int idx - 위치(A 키 : 0, D 키 : 1)
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
     * Description : 플레이어가 마법석 변경 시 해당 마법석에 맞는 스킬을 매개변수로 받아서 현재 사용 가능한 스킬에 할당하는 메서드입니다.
     * Parameter : GameObject sk - 스킬(게임 오브젝트 형태)
     * Return Value : void
     */
    void SetMagicStone(GameObject sk)
    {
        skill = sk;
        this.sk = skill.GetComponent<Skill>();
        if(GameManager.instance.CurQuestNum >= 2) SetMagicStick();
    }

    /* Method : SetMagicStick
     * Description : 플레이어가 마법석 변경 시 마법 지팡이 모습도 알맞게 변경되도록 하는 메서드입니다.
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
     * Description : 스킬 사용 시점부터 스킬이 비활성화 될 때까지 현재 어떤 단계인지에 따라 알맞은 동작을 수행하는 메서드입니다.
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
     * Description : 스킬 쿨타임이 끝날 때까지 1초 단위로 남은 쿨타임을 갱신하는 메서드입니다.
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

    //해당 태그를 가진 물체와 충돌 상태면 플레이어의 레이어 값을 더 낮게 설정하는 방식 -> 향후 수정 필요(레이어 값이 명시되어도 괜찮을까?)
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
                if (transform.position.x > Other.transform.position.x) transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); // 공격받은 방향으로 살짝 넉백하는 동작
                else transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
                GameManager.instance.HpDown(GameManager.instance.MonsterPower); // 몬스터의 공격력 만큼 체력 감소
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
     * Description : 플레이어가 다쳤을 때 동작을 수행하는 코루틴입니다. 현재 체력을 일정량 감소시키고 다친 상태의 애니메이션이 실행되게 하고 설정해놓은 무적 시간동안 다친 상태가 되지 않도록 구현했습니다.
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
     * Description : 플레이어의 행동 제한을 풀어주는 메서드입니다.
     * Return Value : void
     */
    public void UnLockAct()
    {
        actLock = false;
    }
}
