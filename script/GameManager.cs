using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Class : GameManager
 * Description : 게임 내 발생하는 다양한 상호작용들을 관리하고 게임 UI를 담당하는 게임 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 패턴
    public delegate bool GetTalkData(); // npc한테서 대사를 전달받는 대리자
    public delegate void UnLockAct(); // 플레이어 행동 제한을 풀어주는 대리자
    public delegate void UnLockWaiting(); // 기다리고 있는 상황을 기다리지 않아도 되도록 풀어주는 대리자
    public delegate void SetMagic(GameObject sk); // 마법석 변경 시 플레이어 사용 가능 스킬 세팅하는 대리자
    public delegate void SellRequestToMerchant(int idx, int num); // 상인이 아이템을 판매하도록 SellItem 함수 호출하는 대리자
    public delegate int GetItemId(int idx); // 선택한 아이템의 위치를 기반으로 해당 아이템의 아이디가 무엇인지 상인에게서 정보를 받는 대리자
    public GetTalkData npcTalk; // delegate 변수
    public UnLockAct unLockAct; // delegate 변수
    public UnLockWaiting unLockWait; // delegate 변수
    public SetMagic setMagic; // delegate 변수
    public SellRequestToMerchant sellRequest; // delegate 변수
    public GetItemId getItemId; // delegate 변수

    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private GameObject[] skills; // 스킬 GameObject 배열
    [SerializeField] private GameObject yes; // 퀘스트 수락 버튼 GameObject
    [SerializeField] private GameObject no; // 퀘스트 거절 버튼 GameObject
    [SerializeField] private GameObject skillLocked; // 스킬 쿨타임을 기다리는 동안 스킬 사용불가능 표시를 나타내는 이미지 GameObject 
    [SerializeField] private GameObject up; // 진행중인 퀘스트 창 축소하는 버튼 GameObject
    [SerializeField] private GameObject down; // 진행중인 퀘스트 차아 확대하는 버튼 GameObject
    [SerializeField] private GameObject progressingQuest; // 진행중인 퀘스트 창 GameObject
    [SerializeField] private GameObject talkUI; // 대화창 GameObject
    [SerializeField] private GameObject questUI; // 퀘스트 정보창 GameObject
    [SerializeField] private GameObject skillImage; // 플레이어가 현재 사용 가능한 스킬 이미지 GameObject
    [SerializeField] private GameObject inven; // 인벤토리 GameObject
    [SerializeField] private GameObject useItem; // 아이템 사용 버튼 GameObject
    [SerializeField] private GameObject[] invenItemSpaces; // 인벤토리 각 아이템 칸 GameObject 배열
    [SerializeField] private GameObject[] invenItemQuantitySpaces; // 인벤토리 각 아이템 수량 칸 GameObject 배열
    [SerializeField] private GameObject[] reqList; // 퀘스트 요구사항 조건 GameObject 배열
    [SerializeField] private GameObject selectMagicStoneUI; // 마법석 변경창 GameObject
    [SerializeField] private GameObject shopBack; // 상점 백그라운드 창 GameObject
    [SerializeField] private GameObject shopUI; // 상점 창 GameObject
    [SerializeField] private GameObject enterNumberUI; // 구매할 아이템 수량 입력창 GameObject
    [SerializeField] private GameObject blackOut; // 화면 블랙아웃 동작하는 이미지 GameObject
    [SerializeField] private GameObject dragImage; // 드래그 이미지 사본 GameObject
    [SerializeField] private TMP_InputField itemQuantityInput; //아이템 수량 입력칸
    [SerializeField] private TMPro.TMP_Text level; // 플레이어 레벨 
    [SerializeField] private TMPro.TMP_Text gold; //  플레이어가 보유중인 골드
    [SerializeField] private Image hpGraph; // 플레이어 체력 그래프
    [SerializeField] private TMPro.TMP_Text maxHp; // 플레이어 체력 최댓값
    [SerializeField] private TMPro.TMP_Text hp; // 플레이어 체력
    [SerializeField] private Image mpGraph; // 플레이어 마력 그래프
    [SerializeField] private TMPro.TMP_Text maxMp; // 플레이어 마력 최댓값
    [SerializeField] private TMPro.TMP_Text mp; // 플레이어 현재 마력
    [SerializeField] private Image expGraph; // 플레이어 경험치 그래프
    [SerializeField] private TMPro.TMP_Text maxExp; // 플레이어 경험치 최댓값
    [SerializeField] private TMPro.TMP_Text exp; // 플레이어 경험치
    [SerializeField] private Image skill; // 플레이어가 현재 사용 가능한 스킬 이미지
    [SerializeField] private TMPro.TMP_Text coolTime; // 스킬 쿨타임
    [SerializeField] private Image npcImage; // npc 이미지
    [SerializeField] private TMPro.TMP_Text npcName; // npc 이름
    [SerializeField] private TMPro.TMP_Text talk; // npc의 대사
    [SerializeField] private TMPro.TMP_Text questT; // 퀘스트 제목
    [SerializeField] private TMPro.TMP_Text questTitle; // 퀘스트 제목
    [SerializeField] private TMPro.TMP_Text questNpc; // 퀘스트를 준 npc
    [SerializeField] private TMPro.TMP_Text questNpcName; // 퀘스트를 준 npc 이름
    [SerializeField] private TMPro.TMP_Text questInfo; // 퀘스트 정보
    [SerializeField] private TMPro.TMP_Text[] questReqName; // 퀘스트 요구사항 진행 정도 데이터 배열
    [SerializeField] private TMPro.TMP_Text itemName; // 아이템 이름
    [SerializeField] private TMPro.TMP_Text itemInfo; // 아이템 정보
    [SerializeField] private Image[] shopSelectedItem; // 상점에서 선택된 아이템이 무엇인지 표시하기 위해 사용된 이미지 배열
    [SerializeField] private Image[] invenItemList; // 인벤토리 각 칸에 들어갈 아이템 이미지 배열
    [SerializeField] private TMPro.TMP_Text[] invenItemQuantityList; // 인벤토리 각 칸에 들어갈 아이템 수량 배열
    [SerializeField] private Sprite[] npcImages; // npc 스프라이트(이미지) 배열
    [SerializeField] private Sprite[] skImages; // 스킬 스프라이트(이미지) 배열
    [SerializeField] private Sprite[] itemImages; // 아이템 스프라이트(이미지) 배열

    IEnumerator talkAni; // 대화 대사 출력 동작을 수행하는 TalkAnime 코루틴 변수
    IEnumerator waitForTeleport; // 텔레포트 대기 동작을 수행하는 WaitForTeleport 코루틴 변수
    WaitForSeconds wfs; // 대기 시간
    WaitForSeconds wfs2; // 대기 시간

    int curMapNum = 0; // 현재 맵 번호
    int curNpcId = 0; // 현재 플레이어와 접촉중인 npc 아이디
    int curLevel = 1; // 플레이어의 현재 레벨
    int curGold = 1000; //  플레이어가 현재 보유중인 골드
    int curHp = 100; // 플레이어의 현재 체력
    int curMaxHp = 100; // 플레이어의 현재 체력 최댓값
    int curMp = 100; // 플레이어의 현재 마력
    int curMaxMp = 100; // 플레이어의 현재 마력 최댓값
    int curExp = 0; // 플레이어의 현재 경험치
    int curMaxExp = 100; // 플레이어의 레벨업에 필요한 현재 경험치 최댓값
    int curQuestNum = 1; // 현재 진행 가능한 혹은 진행중인 퀘스트 번호
    int curQuestNpcId; // 현재 진행 가능한 혹은 진행중인 퀘스트를 가지고 있는 npc의 아이디
    int finishReqNum = 0; // 퀘스트 조건들에서 완료한 조건 수
    int selectedInvenItemLoc = -1; // 선택된 아이템의 칸 위치(인벤토리), -1 : 선택된 아이템이 없음을 의미
    int selectedShopItemLoc = -1; // 선택된 아이템의 칸 위치(상점), -1 : 선택된 아이템이 없음을 의미
    int curUsedItemLoc = -1; // 현재 사용할 아이템의 칸 위치(인벤토리), -1 : 사용할 아이템이 없음
    int curDragItemLoc = -1; // 현재 드래그 되고 있는 아이템 인벤토리 내 위치, -1 : 드래그 중인 아이템이 없음
    int monsterPower = 0; // 몬스터의 공격력
    int[] curShortCutPotions = new int[]{ -1, -1 }; // 포션 단축키에 올려져 있는 포션의 아이템 아이디 목록
    float percent = 0; // 그래프 증가 혹은 감소 게이지 퍼센트 값
    string tmp = null; // 대사 출력에 쓰일 임시 문자열
    char[] textData = null; // npc의 대사를 나타내는 문자 배열
    bool isTalking = false; // 대화중 즉 대사가 출력되는 중인지 여부
    bool teleportReady = false; // 텔레포트 준비 완료 여부
    bool success = false; // 퀘스트 성공 여부
    bool finishTalk = false; // 대화 종료 여부
    bool skDisable = false; // 스킬 사용불가능 여부
    bool accept = false; // 퀘스트 수락 여부
    bool waterSelected = false; // 물 마법석 선택 여부
    bool dirtSelected = false; // 흙 마법석 선택 여부
    bool windSelected = false; // 바람 마법석 선택 여부
    bool noEmptySpace = false; // 인벤토리 내 빈공간이 없는지 여부
    bool invenOn = false; // 인벤토리 창이 켜져 있는지 여부
    bool shopExit = false; // 상점 퇴장 여부
    bool clickLock = false; // 맨 앞에 다른 UI 창이 켜져있으면 뒤쪽 UI들에 대한 마우스 클릭 이벤트 제한하도록 함
    List<int> curQuestReqNum = new List<int>(); // 현재 진행중인 퀘스트 성공에 필요한 조건 목록
    Vector3 playerPos; // 플레이어의 현재 위치
    InventoryManager im; // 인벤토리 관리자 InventoryManager 클래스 객체
    Animator ani; // 유니티 애니메이션 컴포넌트

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.ani = blackOut.GetComponent<Animator>(); // blackOut GameObject 에서 Animator 컴포넌트를 가져옵니다.
        this.talkAni = TalkAnime(); // 코루틴 할당
        this.waitForTeleport = WaitForTeleport(); // 코루틴 할당
        this.wfs = new WaitForSeconds(0.05f); // 대기 시간
        this.wfs2 = new WaitForSeconds(0.5f); // 대기 시간
        this.im = inven.GetComponent<InventoryManager>(); // inven GameObject 에서 Inventory Manager 클래스 컴포넌트를 가져옵니다.

        //향후 데이터 저장&로드 구현 시 수정해야 함
        this.curGold = Convert.ToInt32(gold.text); // 현재 플레이어가 보유중인 골드
        this.curHp = Convert.ToInt32(hp.text); // 현재 플레이어의 체력
        this.curMaxHp = Convert.ToInt32(maxHp.text); // 현재 플레이어의 체력 최댓값
        this.curMp = Convert.ToInt32(mp.text); // 현재 플레이어의 마력
        this.curMaxMp = Convert.ToInt32(maxMp.text); // 현재 플레이어의 마력 최댓값
        this.curMaxExp = Convert.ToInt32(maxExp.text); // 현재 플레이어의 경험치 최댓값
        this.curQuestNpcId = QuestManager.instance.QuestDataList[curQuestNum - 1].NpcId; // 현재 진행가능한 퀘스트를 가지고 있는 npc 아이디
    }

    /* Property */
    public int CurLevel
    {
        get
        {
            return curLevel;
        }
    }
    /* Property */
    public int CurGold
    {
        get
        {
            return curGold;
        }
    }
    /* Property */
    public int CurMp
    {
        get
        {
            return curMp;
        }
    }
    /* Property */
    public int CurMapNum
    {
        get
        {
            return curMapNum;
        }
    }
    /* Property */
    public int CurQuestNum
    {
        get
        {
            return curQuestNum;
        }
    }
    /* Property */
    public int CurQuestNpcId
    {
        get
        {
            return curQuestNpcId;
        }
    }
    /* Property */
    public int SelectedShopItemLoc
    {
        get
        {
            return selectedShopItemLoc;
        }
    }
    /* Property */
    public int CurUsedItemLoc
    {
        get
        {
            return curUsedItemLoc;
        }
        set
        {
            curUsedItemLoc = value;
        }
    }
    /* Property */
    public int CurDragItemLoc
    {
        get
        {
            return curDragItemLoc;
        }
        set
        {
            curDragItemLoc = value;
        }
    }
    /* Property */
    public int MonsterPower
    {
        get
        {
            return monsterPower;
        }
        set
        {
            monsterPower = value;
        }
    }
    /* Property */
    public int[] CurShortCutPotions
    {
        get
        {
            return curShortCutPotions;
        }
        set
        {
            curShortCutPotions = value;
        }
    }
    /* Property */
    public bool Accept
    {
        get
        {
            return accept;
        }
    }
    /* Property */
    public bool Success
    {
        get
        {
            return success;
        }
    }
    /* Property */
    public bool SkDisable
    {
        get
        {
            return skDisable;
        }
    }
    /* Property */
    public char[] TextData
    {
        set
        {
            textData = value;
        }
    }
    /* Property */
    public bool FinishTalk
    {
        set
        {
            finishTalk = value;
        }
    }
    /* Property */
    public bool WaterSelected
    {
        get
        {
            return waterSelected;
        }
        set
        {
            waterSelected = value;
        }
    }
    /* Property */
    public bool DirtSelected
    {
        get
        {
            return dirtSelected;
        }
        set
        {
            dirtSelected = value;
        }
    }
    /* Property */
    public bool WindSelected
    {
        get
        {
            return windSelected;
        }
        set
        {
            windSelected = value;
        }
    }
    /* Property */
    public bool TeleportReady
    {
        get
        {
            return teleportReady;
        }
    }
    /* Property */
    public bool ShopExit
    {
        get
        {
            return shopExit;
        }
    }
    /* Property */
    public bool NoEmptySpace
    {
        get
        {
            return noEmptySpace;
        }
    }
    /* Property */
    public InventoryManager InvenManager
    {
        get
        {
            return im;
        }
    }
    /* Property */
    public Vector3 PlayerPos
    {
        get
        {
            return playerPos;
        }
        set
        {
            playerPos = value;
        }
    }
    /* Property */
    public GameObject DragImage
    {
        get
        {
            return dragImage;
        }
    }
    /* Method : WaitForTeleportReady
     * Description : 텔레포트 준비 완료까지 기다리는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void WaitForTeleportReady()
    {
        blackOut.SetActive(true);
        ani.SetTrigger("blackOut");
        waitForTeleport = WaitForTeleport();
        StartCoroutine(waitForTeleport);
    }

    /* Method : TeleportMap
     * Description : 텔레포트 시 현재 맵 번호를 이동 후 맵 번호로 바꾸는 동작을 수행하는 메서드입니다.
     * Parameter : int n - 맵 번호
     * Return Value : void
     */
    public void TeleportMap(int n)
    {
        curMapNum = n;
        teleportReady = false;
    }

    /* Coroutine : WaitForTeleport
     * Description : 텔레포트 준비까지 설정해둔 대기 시간만큼 기다리고 준비가 완료되면 teleportReady를 true로, 그리고 텔레포트가 끝날때까지 해당 시간만큼 대기하는 동작을 수행하는 코루틴입니다.
     * 텔레포트 할 때마다 화면 블랙아웃 동작이 수행되어서 텔레포트가 끝나면 해당 객체를 비활성화하도록 구현했습니다.
     */
    IEnumerator WaitForTeleport()
    {
        yield return wfs2;
        teleportReady = true;
        yield return wfs2;
        blackOut.SetActive(false);
        yield break;
    }

    /* Method : SetSkillDisable
     * Description : 스킬 사용 불가능 상태로 전환하는 메서드입니다. 그리고 스킬 사용 불가능 상태라는 것을 UI에 직관적으로 표시하는 이미지를 활성화하도록 구현했습니다.
     * Return Value : void
     */
    public void SetSkillDisable()
    {
        skDisable = true;
        skillLocked.SetActive(true);
    }

    /* Method : ResetSkillDisable
     * Description : 스킬 사용 가능 상태로 전환하는 메서드입니다. 그리고 스킬 사용 불가능 상태를 나타내는 이미지를 비활성화하도록 구현했습니다.
     * Return Value : void
     */
    public void ResetSkillDisable()
    {
        skDisable = false;
        skillLocked.SetActive(false);
    }

    /* Method : CheckCoolTime
     * Description : 매개변수로 받은 남은 쿨타임을 쿨타임 UI에 표시하는 메서드입니다. 이 값이 0이면 쿨타임이 끝났다는 뜻이므로 UI를 비웁니다. 
     * Parameter : float num - 남은 쿨타임 시간(초 단위)
     * Return Value : void
     */
    public void CheckCoolTime(float num)
    {
        coolTime.text = Convert.ToString(num);
        if (num == 0)
        {
            coolTime.text = null;
        }
    }

    /* Method : GoldIncrease
     * Description : 매개변수로 받은 값만큼 골드를 증가시키는 메서드입니다.
     * Parameter : int n - 골드 값
     * Return Value : void
     */
    public void GoldIncrease(int n)
    {
        curGold += n;
        gold.text = Convert.ToString(curGold);
    }

    /* Method : GoldDecrease
     * Description : 매개변수로 받은 값만큼 골드를 감소시키는 메서드입니다.
     * Parameter : int n - 골드 값
     * Return Value : void
     */
    public void GoldDecrease(int n)
    {
        curGold -= n;
        gold.text = Convert.ToString(curGold);
    }

    /* Method : HpUp
     * Description : 매개변수로 받은 값만큼 체력을 올리는 메서드입니다.
     * Parameter : int n - 체력 값
     * Return Value : void
     */
    public void HpUp(int n) // 최댓값 초과 여부 검사조건 나중에 추가해야함
    {
        curHp += n;
        if (curHp <= curMaxHp)
        {
            percent = (float)((float)(n) * (1.0 / (float)(curMaxHp)));
            hpGraph.fillAmount += percent;
        }
        else
        {
            curHp = curMaxHp;
            hpGraph.fillAmount = 1f;
        }
        hp.text = Convert.ToString(curHp);
    }

    /* Method : HpDown
     * Description : 매개변수로 받은 값만큼 체력을 내리는 메서드입니다.
     * Parameter : int n - 체력 값
     * Return Value : void
     */
    public void HpDown(int n) // 0 이하 여부 검사조건 나중에 추가해야함
    {
        curHp -= n;
        if (curHp >= 0)
        {
            percent = (float)((float)(n) * (1.0 / (float)(curMaxHp)));
            hpGraph.fillAmount -= percent;
        }
        else
        {
            curHp = 0;
            hpGraph.fillAmount = 0f;
            //게임 오버 메서드 추가해야 함
        }
        hp.text = Convert.ToString(curHp);
    }

    /* Method : MpUp
     * Description : 매개변수로 받은 값만큼 마력을 올리는 메서드입니다.
     * Parameter : int n - 마력 값
     * Return Value : void
     */
    public void MpUp(int n) // 최댓값 초과 여부 검사조건 나중에 추가해야함
    {
        curMp += n;
        if (curMp <= curMaxMp)
        {
            percent = (float)((float)(n) * (1.0 / (float)(curMaxMp)));
            mpGraph.fillAmount += percent;
        }
        else
        {
            curMp = curMaxMp;
            mpGraph.fillAmount = 1f;
        }
        mp.text = Convert.ToString(curMp);
    }

    /* Method : MpDown
     * Description : 매개변수로 받은 값만큼 마력을 내리는 메서드입니다.
     * Parameter : int n - 마력 값
     * Return Value : void
     */
    public void MpDown(int n)
    {
        curMp -= n;
        if (curMp >= 0)
        {
            percent = (float)((float)(n) * (1.0 / (float)(curMaxMp)));
            mpGraph.fillAmount -= percent;
        }
        else
        {
            curMp = 0;
            mpGraph.fillAmount = 0;

        }
        mp.text = Convert.ToString(curMp);
    }

    /* Method : ExpUp
     * Description : 매개변수로 받은 값만큼 경험치를 올리는 메서드입니다. 게이지가 100프로 차면 레벨업 메서드가 호출됩니다.
     * Parameter : int n - 경험치 값
     * Return Value : void
     */
    public void ExpUp(int n)
    {
        curExp += n;
        percent = (float)((float)(n) * (1.0 / (float)(curMaxExp)));
        expGraph.fillAmount += percent;
        exp.text = Convert.ToString(curExp);
        if (expGraph.fillAmount == 1f)
        {
            LevelUp();
        }
    }

    /* Method : LevelUp
     * Description : 레벨업 시 동작을 수행하는 메서드입니다. 경험치를 0으로 초기화하고 레벨을 1 증가시킵니다.
     * Return Value : void
     */
    public void LevelUp()
    {
        curLevel++;
        level.text = Convert.ToString(curLevel);
        curExp = 0;
        expGraph.fillAmount = 0f;
        exp.text = Convert.ToString(curExp);
        foreach(GameObject sk in skills)
        {
            sk.GetComponent<Skill>().AttackPower += 5;
        }
    }

    /* Method : InventoryOn
     * Description : 인벤토리 창을 여는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void InventoryOn()
    {
        invenOn = true;
        for (int i = 0; i < im.MaxSize; i++)
        {
            if (i < im.InvenItemList.Count)
            {
                invenItemSpaces[i].SetActive(true);
                invenItemQuantitySpaces[i].SetActive(true);
            }
            else
            {
                invenItemSpaces[i].SetActive(false);
                invenItemQuantitySpaces[i].SetActive(false);
            }
        }
        inven.SetActive(true);
    }

    /* Method : InventoryUpdate
     * Description : 인벤토리 아이템 보유 현황을 업데이트 하는 메서드입니다.
     * Return Value : void
     */
    public void InventoryUpdate()
    {
        for (int i = 0; i < im.MaxSize; i++)
        {
            if (i < im.InvenItemList.Count)
            {
                invenItemList[i].sprite = itemImages[im.InvenItemList[i].ItemId - 1];
                invenItemQuantityList[i].text = Convert.ToString(im.InvenItemQuantityList[i]);
            }
            else
            {
                invenItemList[i].sprite = null;
                invenItemQuantityList[i].text = null;
            }
        }
    }

    /* Method : ShowItemInfo
     * Description : 인벤토리 아이템 선택 시 아이템 정보를 보여주는 동작을 하는 메서드입니다.
     * Parameter : string name - 아이템 이름, string info - 아이템 정보
     * Return Value : void
     */
    public void ShowItemInfo(string name, string info)
    {
        itemName.text = name;
        itemInfo.text = info;
    }

    /* Method : ClearItemInfo
     * Description : 인벤토리 아이템 정보창을 빈 상태로 초기화합니다.
     * Return Value : void
     */
    public void ClearItemInfo()
    {
        itemName.text = null;
        itemInfo.text = null;
        CloseUseItemButton();
    }

    /* Method : OpenUseItemButton
    * Description : 아이템 사용 버튼을 활성화합니다.
    * Return Value : void
    */
    public void OpenUseItemButton()
    {
        useItem.SetActive(true);
    }

    /* Method : CloseUseItemButton
    * Description : 아이템 사용 버튼을 비활성화합니다.
    * Return Value : void
    */
    public void CloseUseItemButton()
    {
        useItem.SetActive(false);
    }

    /* Method : UsePotion
     * Description : 플레이어가 포션 아이템을 사용 시 관련 동작을 수행하는 메서드입니다. 현재 사용할 아이템이 인벤토리 내 어느 위치에 있는지 나타내는 curUsedItemLoc을 이용해 해당 아이템의 수량을 감소시키고
     * 아이템의 UseItem 함수를 호출합니다. 그리고 인벤토리 아이템 보유 현황을 업데이트합니다.
     * Return Value : void
     */
    public void UsePotion()
    {
        Potion potion = im.InvenItemList[curUsedItemLoc] as Potion;
        int curNum = im.InvenItemQuantityList[curUsedItemLoc];
        im.ItemQuantityDecrease(curUsedItemLoc, 1);
        curNum--;
        potion.UseItem();
        InventoryUpdate();
        if (invenOn && curNum == 0)
        {
            InventoryOn();
            if (selectedInvenItemLoc == curUsedItemLoc) SelectedItemFromInven(selectedInvenItemLoc);
        }
    }

    /* Method : InventoryOff
     * Description : 인벤토리 창을 닫는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void InventoryOff()
    {
        invenOn = false;
        if(selectedInvenItemLoc != -1) SelectedItemFromInven(selectedInvenItemLoc);
        inven.SetActive(false);
    }

    /* Method : ShopOn
     * Description : 상점 창을 여는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void ShopOn()
    {
        shopExit = false;
        shopBack.SetActive(true);
        shopUI.SetActive(true);
    }

    /* Method : ShopOff
     * Description : 상점 창을 닫는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void ShopOff()
    {
        selectedShopItemLoc = -1;
        shopExit = true;
        unLockWait();
        ClearTalk();
        TalkEvent();
        foreach (Image i in shopSelectedItem)
        {
            Color color = i.color;
            color.a = 0f;
            i.color = color;
        }
        shopBack.SetActive(false);
        shopUI.SetActive(false);
    }

    /* Method : BuyItemRequest
     * Description : 상점에서 플레이어의 아이템 구매 요청이 발생하면 상인 npc에게 선택된 아이템과 입력받은 수량을 매개변수로 보내서 판매 요청 동작을 수행하는 메서드입니다. 
     * Return Value : void
     */
    public void BuyItemRequest()
    {
        if (itemQuantityInput.text == null) return;
        int num = Convert.ToInt32(itemQuantityInput.text);
        if (num <= 0) return;
        clickLock = false;
        itemQuantityInput.text = null;
        enterNumberUI.SetActive(false);
        sellRequest(selectedShopItemLoc, num);
        unLockWait();
        ClearTalk();
        TalkEvent();
    }

    /* Method : GetItem
     * Description : 플레이어가 상점에서 아이템 구매를 성공적으로 완료했거나 맵 내에서 아이템을 획득한 경우 아이템 획득에 대한 동작을 수행하는 메서드입니다.
     * 획득한 아이템이 이미 인벤토리 내 보유중이었던 아이템인지 FindItem 메서드로 파악한 후 보유중이 아니었으면 ItemInsert 메서드를, 보유중이었으면 ItemItemQuantityIncrease 메서드를 호출하도록 구현했습니다.
     * Parameter Item item - 아이템, int num - 수량
     * Return Value : void
     */
    public void GetItem(Item item, int num)
    {
        int idx = im.FindItem(item.ItemId);
        if (idx == -1) im.ItemInsert(item, num);
        else
        {
            im.ItemQuantityIncrease(idx, num);
        }
        InventoryUpdate();
    }

    /* Method : EnterItemQuantity
     * Description : 상점에서 구매할 아이템을 얼마나 구매할지 그 수량을 입력받는 동작을 수행하는 메서드입니다. 이 때 선택된 아이템이 없으면 수량 입력창을 띄우기 전에 메서드를 빠져나오도록 구현했습니다
     * Return Value : void
     */
    public void EnterItemQuantity()
    {
        if (selectedShopItemLoc == -1)
        {
            unLockWait();
            ClearTalk();
            TalkEvent();
            return;
        }
        if (im.IsFull && im.FindItem(getItemId(selectedShopItemLoc)) == -1) noEmptySpace = true;
        else noEmptySpace = false;
        if (noEmptySpace)
        {
            unLockWait();
            ClearTalk();
            TalkEvent();
            return;
        }
        clickLock = true;
        enterNumberUI.SetActive(true);
    }

    /* Method : SelectedItem
     * Description : 아이템을 선택하는 동작이 일어날 수 있는 곳은 인벤토리와 상점입니다. 그렇기 때문에 케이스를 나누어서 알맞은 아이템 선택 관련 동작을 수행하도록 구현했습니다. 
     * Parameter : int idx - 위치
     * Return Value : void
     */
    public void SelectedItem(int idx)
    {
        if (invenOn) SelectedItemFromInven(idx);
        else SelectedItemFromShop(idx);
    }

    /* Method : SelectedItemFromInven
     * Description : 인벤토리에서 마우스 왼쪽 버튼으로 선택된 아이템의 위치를 기준으로 해당 아이템이 선택되었단 의미에서 이미지 불투명도를 조정하고 선택된 위치 값을 selectedInvenItemLoc에 저장하는 메서드입니다.
     * 이미 선택되어 있던 이미지가 다시 선택되거나 다른 이미지를 선택하면 기존에 선택된 이미지가 선택되지 않은 상태로 돌아가도록 구현했습니다.
     * Parameter : int idx - 위치
     * Return Value : void
     */
    public void SelectedItemFromInven(int idx)
    {
        Color color;
        if (selectedInvenItemLoc == idx)
        {
            color = invenItemList[selectedInvenItemLoc].color;
            color.a = 1f;
            invenItemList[selectedInvenItemLoc].color = color;
            color = invenItemQuantityList[selectedInvenItemLoc].color;
            color.a = 1f;
            invenItemQuantityList[selectedInvenItemLoc].color = color;
            selectedInvenItemLoc = -1;
            ClearItemInfo();
        }
        else
        {
            if (selectedInvenItemLoc != -1)
            {
                color = invenItemList[selectedInvenItemLoc].color;
                color.a = 1f;
                invenItemList[selectedInvenItemLoc].color = color;
                color = invenItemQuantityList[selectedInvenItemLoc].color;
                color.a = 1f;
                invenItemQuantityList[selectedInvenItemLoc].color = color;
                ClearItemInfo();
            }
            selectedInvenItemLoc = idx;
            color = invenItemList[selectedInvenItemLoc].color;
            color.a = 0.5f;
            invenItemList[selectedInvenItemLoc].color = color;
            color = invenItemQuantityList[selectedInvenItemLoc].color;
            color.a = 0.5f;
            invenItemQuantityList[selectedInvenItemLoc].color = color;
            im.ShowSelectedItemInfo(selectedInvenItemLoc);
        }
    }

    /* Method : SelectedItemFromShop
     * Description : 상점에서 마우스 왼쪽 버튼으로 선택된 아이템의 위치를 기준으로 해당 아이템이 선택되었단 의미에서 이미지 불투명도를 조정하고 선택된 위치 값을 selectedShopItemLoc에 저장하는 메서드입니다.
     * 이미 선택되어 있던 이미지가 다시 선택되거나 다른 이미지를 선택하면 기존에 선택된 이미지가 선택되지 않은 상태로 돌아가도록 구현했습니다.
     * Parameter : int idx - 위치
     * Return Value : void
     */
    public void SelectedItemFromShop(int idx)
    {
        if (clickLock) return;
        Color color;
        if (selectedShopItemLoc == idx)
        {
            color = shopSelectedItem[selectedShopItemLoc].color;
            color.a = 0f;
            shopSelectedItem[selectedShopItemLoc].color = color;
            selectedShopItemLoc = -1;
        }
        else
        {
            if (selectedShopItemLoc != -1)
            {
                color = shopSelectedItem[selectedShopItemLoc].color;
                color.a = 0f;
                shopSelectedItem[selectedShopItemLoc].color = color;
            }
            selectedShopItemLoc = idx;
            color = shopSelectedItem[selectedShopItemLoc].color;
            color.a = 0.5f;
            shopSelectedItem[selectedShopItemLoc].color = color;
        }
    }

    /* Method : TalkEvent
     * Description : 플레이어가 npc와 대화 하고자 할 때 실행되는 메서드입니다. 대화창을 활성화한 후 npcTalk 대리자를 통해 사전에 들어있던 npc의 DialogueReady 함수를 호출하는데 이 때 반환값이 false면 바로 메서드를
     * 빠져나옵니다. 이렇게 가져온 대사를 좀 더 실감나게 연출하기 위해 한 글자씩 시간차를 두고 출력되도록 TalkAnime 코루틴을 실행하는 방식으로 구현했습니다. 그리고 대사가 출력중인 상태일 때 이 메서드가 호출되면 
     * 다음 대사를 가져오는 작업을 수행하지 않고 현재 출력중인 대사가 한번에 출력되도록 스킵 동작을 수행하게 구현했습니다. finishTalk가 true일 때 이 메서드가 호출되면 대화를 종료하라는 의미이므로 TalkDone 메서드를
     * 호출합니다.
     * Return Value : void
     */
    public void TalkEvent()
    {
        talkUI.SetActive(true);
        if (isTalking)
        {
            SkipTalk();
        }
        else
        {
            if (!npcTalk()) return;
            if (finishTalk)
            {
                TalkDone();
            }
            else
            {
                talkAni = TalkAnime();
                StartCoroutine(talkAni);
            }
        }
    }

    /* Method : SkipTalk
     * Description : 현재 대화창에 출력중인 대사를 한번에 출력하는 스킵 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void SkipTalk()
    {
        StopCoroutine(talkAni);
        tmp = null;
        talk.text = new string(textData);
        isTalking = false;
    }

    /* Method : ClearTalk
     * Description : 현재 출력중인 대사를 중단시키고 임시 문자열 tmp를 빈 상태로 놓는 메서드입니다.
     * Return Value : void
     */
    public void ClearTalk()
    {
        isTalking = false;
        StopCoroutine(talkAni);
        tmp = null;
    }

    /* Method : TalkDone
     * Description : 대화를 종료하는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void TalkDone()
    {
        unLockAct();
        talkUI.SetActive(false);
        finishTalk = false;
    }

    /* Coroutine : TalkAnime
     * Description : 대사를 한글자씩 순서대로 시간차를 두면서 출력하는 동작을 수행하는 코루틴입니다.
     */
    IEnumerator TalkAnime()
    {
        for (int i = 0; i < textData.Length; i++)
        {
            isTalking = true;
            tmp += textData[i];
            talk.text = tmp;
            yield return wfs;
        }
        tmp = null;
        isTalking = false;
        yield break;
    }

    /* Method : OpenCurQuestList
     * Description : 현재 진행중인 퀘스트 목록 창을 여는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void OpenCurQuestList()
    {
        down.SetActive(false);
        up.SetActive(true);
        progressingQuest.SetActive(true);
    }

    /* Method : CloseCurQuestList
     * Description : 현재 진행중인 퀘스트 목록 창을 닫는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void CloseCurQuestList()
    {
        down.SetActive(true);
        up.SetActive(false);
        progressingQuest.SetActive(false);
    }

    /* Method : ChangeTalkNpc
     * Description : 매개변수로 받은 npc 아이디와 이름으로 대화창에서 npc의 이미지 부분과 이름을 알맞게 세팅하는 메서드입니다.
     * Parameter : int id - 아이디, string name - 이름
     * Return Value : void
     */
    public void ChangeTalkNpc(int id, string name)
    {
        if (curNpcId != id)
        {
            curNpcId = id;
            npcImage.sprite = npcImages[id - 1];
            npcName.text = name;
        }
    }


    /* Method : OpenYesOrNoButton
     * Description : 퀘스트 수락, 거절 버튼을 활성화하는 메서드입니다.
     * Return Value : void
     */
    public void OpenYesOrNoButton()
    {
        yes.SetActive(true);
        no.SetActive(true);
    }

    /* Method : CloseYesOrNoButton
     * Description : 퀘스트 수락, 거절 버튼을 비활성화하는 메서드입니다.
     * Return Value : void
     */
    public void CloseYesOrNoButton()
    {
        yes.SetActive(false);
        no.SetActive(false);
    }

    /* Method : QuestAccept
     * Description : 플레이어가 퀘스트 수락 시 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void QuestAccept()
    {
        accept = true;
        unLockWait();
        CloseYesOrNoButton();
        ShowQuestInfo();
        ClearTalk();
        TalkEvent();
    }

    /* Method : QuestRefuse
     * Description : 플레이어가 퀘스트 거절 시 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void QuestRefuse()
    {
        accept = false;
        unLockWait();
        CloseYesOrNoButton();
        ClearTalk();
        TalkEvent();
    }

    /* Method : ShowQuestInfo
     * Description : 플레이어가 수락한 퀘스트에 대한 정보를 퀘스트 정보창에 나타내는 메서드입니다.
     * Return Value : void
     */
    public void ShowQuestInfo()
    {
        questUI.SetActive(true);
        questTitle.text = QuestManager.instance.QuestDataList[curQuestNum - 1].Title;
        questNpcName.text = QuestManager.instance.QuestDataList[curQuestNum - 1].NpcName;
        questInfo.text = QuestManager.instance.QuestDataList[curQuestNum - 1].Info;
        questT.color = Color.white;
        questTitle.color = Color.white;
        questNpc.color = Color.white;
        questNpcName.color = Color.white;
        questInfo.color = Color.white;
        for (int i = 0; i < QuestManager.instance.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            questReqName[i].text = QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Name[i] + " " + "0 / " + Convert.ToString(QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Num[i]);
            reqList[i].SetActive(true);
            questReqName[i].color = Color.white;
            curQuestReqNum.Add(0);
        }
    }

    /* Method : QuestUpdate
     * Description : 진행중인 퀘스트가 있을 때 플레이어가 해당 퀘스트 완료조건에 해당하는 동작을 수행하였는지 확인하고 만약 해당하면 현재 수행한 조건 상태를 업데이트하는 동작을 수행하는 메서드입니다.
     * 현재 진행 중인 퀘스트가 없으면 메서드를 빠져나옵니다. 조건을 검사할 때 id가 디폴트 값인 0으로 들어온 경우 별도로 식별해야 할 아이디 정보가 없다는 것을 의미합니다. 즉, type만 일치하면 퀘스트 요구 조건에
     * 해당한다는 의미입니다.
     * Parameter : string type - 퀘스트 조건 유형, int id - 퀘스트 조건에 해당하는 정보를 식별하는 아이디(ex : 아이디가 1 인 몬스터 처치)
     * Return Value : void
     */
    public void QuestUpdate(string type, int id = 0)
    {
        if (!accept) return;
        for (int i = 0; i < QuestManager.instance.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            if (curQuestReqNum[i] == QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Num[i]) continue;
            if (QuestManager.instance.QuestDataList[curQuestNum - 1].Type[i] == type && (id == 0 || QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Id[i] == id))
            {
                if (curQuestReqNum[i] < QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Num[i])
                {
                    curQuestReqNum[i]++;
                    questReqName[i].text = QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Name[i] + " " + Convert.ToString(curQuestReqNum[i]) + " / " + Convert.ToString(QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Num[i]);
                }
                if (curQuestReqNum[i] == QuestManager.instance.QuestDataList[curQuestNum - 1].Req_Num[i]) finishReqNum++;
            }
        }
        if (finishReqNum == QuestManager.instance.QuestDataList[curQuestNum - 1].Type.Count)
        {
            QuestSuccess();
        }
    }

    /* Method : QuestSuccess
     * Description : 퀘스트 성공 시 동작을 수행하는 메서드입니다. 성공했다는 것을 직관적으로 보여주기 위해 퀘스트 정보창에 퀘스트 내용을 표시하는 텍스트 색깔이 모두 노란색으로 변경되도록 구현했습니다.
     * Return Value : void
     */
    public void QuestSuccess()
    {
        finishReqNum = 0;
        success = true;
        questT.color = Color.yellow;
        questTitle.color = Color.yellow;
        questNpc.color = Color.yellow;
        questNpcName.color = Color.yellow;
        questInfo.color = Color.yellow;
        for (int i = 0; i < QuestManager.instance.QuestDataList[curQuestNum - 1].Type.Count; i++)
        {
            questReqName[i].color = Color.yellow;
        }
    }

    /* Method : QuestDone
     * Description : 퀘스트 완료 시 동작을 수행하는 메서드입니다. 퀘스트 내용이 표시되어 있던 정보창을 비활성화하고 만약 완료한 퀘스트 번호가 1이면 스킬 이미지가 UI에 표시되도록 구현했습니다.
     * 그리고 퀘스트 보상을 플레이어가 받도록 QuestReward 메서드를 호출합니다. 또 해당 퀘스트 수행 정도를 저장하는 curQuestReqNum 리스트에 들어있던 정보를 모두 초기화합니다. 그리고 다음 퀘스트가 존재하면
     * qm에게서 해당 퀘스트를 가진 npc 아이디를 가져와 curQuestNpcId에 저장하도록 구현했습니다. 
     * Return Value : void
     */
    public void QuestDone()
    {
        questUI.SetActive(false);
        if (curQuestNum == 1) skillImage.SetActive(true);
        accept = false;
        success = false;
        QuestReward();
        for (int i = QuestManager.instance.QuestDataList[curQuestNum - 1].Type.Count - 1; i >= 0; i--)
        {
            curQuestReqNum.RemoveAt(i);
        }
        curQuestNum++;
        if (curQuestNum <= QuestManager.instance.QuestDataList.Count) curQuestNpcId = QuestManager.instance.QuestDataList[curQuestNum - 1].NpcId;
        else
        {
            curQuestNpcId = 0;
        }
    }

    /* Method : QuestReward
     * Description : 플레이어가 완료한 퀘스트 보상을 수여하는 메서드입니다.(아직 일부 미구현 상태)
     * Return Value : void
     */
    public void QuestReward()
    {
        for (int i = 0; i < QuestManager.instance.QuestDataList[curQuestNum - 1].RewardType.Count; i++)
        {
            if (QuestManager.instance.QuestDataList[curQuestNum - 1].RewardType[i] == "exp")
            {
                ExpUp(QuestManager.instance.QuestDataList[curQuestNum - 1].Reward[i]);
            }
            if (QuestManager.instance.QuestDataList[curQuestNum - 1].RewardType[i] == "gold")
            {
                GoldIncrease(QuestManager.instance.QuestDataList[curQuestNum - 1].Reward[i]);
            }
        }
    }

    /* Method : SelectMagicStoneUIOn
     * Description : 마법석 변경창을 여는 메서드입니다. 1번 퀘스트를 아직 수락한 상태가 아니면 메서드를 빠져나옵니다.
     * Return Value : void
     */
    public void SelectMagicStoneUIOn()
    {
        if (curQuestNum == 1 && !accept)
        {
            unLockAct();
            return;
        }
        selectMagicStoneUI.SetActive(true);
    }

    /* Method : SelectMagicStoneUIOn
     * Description : 마법석 변경창을 닫는 메서드입니다. 
     * Return Value : void
     */
    public void SelectMagicStoneUIOff()
    {
        selectMagicStoneUI.SetActive(false);
        unLockAct();
    }

    /* Method : SwitchingMagicStone
     * Description : 마법석 선택 시 선택된 마법석의 정보를 파악하고 해당 스킬 이미지를 UI에 나타내고 플레이어가 해당 스킬을 사용할 수 있도록 매개변수로 해당 스킬 객체를 전달하는 메서드입니다. 
     * Return Value : void
     */
    public void SwitchingMagicStone()
    {
        if (!waterSelected && !dirtSelected && !windSelected) return;
        if (waterSelected)
        {
            skill.sprite = skImages[0];
            setMagic(skills[0]);
        }
        else if (dirtSelected)
        {
            skill.sprite = skImages[1];
            setMagic(skills[1]);

        }
        else if (windSelected)
        {
            skill.sprite = skImages[2];
            setMagic(skills[2]);

        }
        QuestUpdate("마법석선택");
        SelectMagicStoneUIOff();
    }
}
