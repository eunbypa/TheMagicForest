using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Class : GameManager
 * Description : ���� �� �߻��ϴ� �پ��� ��ȣ�ۿ���� �����ϰ� ���� UI�� ����ϴ� ���� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� ����
    public delegate bool GetTalkData(); // npc���׼� ��縦 ���޹޴� �븮��
    public delegate void UnLockAct(); // �÷��̾� �ൿ ������ Ǯ���ִ� �븮��
    public delegate void UnLockWaiting(); // ��ٸ��� �ִ� ��Ȳ�� ��ٸ��� �ʾƵ� �ǵ��� Ǯ���ִ� �븮��
    public delegate void SetMagic(GameObject sk); // ������ ���� �� �÷��̾� ��� ���� ��ų �����ϴ� �븮��
    public delegate void SellRequestToMerchant(int idx, int num); // ������ �������� �Ǹ��ϵ��� SellItem �Լ� ȣ���ϴ� �븮��
    public delegate int GetItemId(int idx); // ������ �������� ��ġ�� ������� �ش� �������� ���̵� �������� ���ο��Լ� ������ �޴� �븮��
    public GetTalkData npcTalk; // delegate ����
    public UnLockAct unLockAct; // delegate ����
    public UnLockWaiting unLockWait; // delegate ����
    public SetMagic setMagic; // delegate ����
    public SellRequestToMerchant sellRequest; // delegate ����
    public GetItemId getItemId; // delegate ����

    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private GameObject[] skills; // ��ų GameObject �迭
    [SerializeField] private GameObject yes; // ����Ʈ ���� ��ư GameObject
    [SerializeField] private GameObject no; // ����Ʈ ���� ��ư GameObject
    [SerializeField] private GameObject skillLocked; // ��ų ��Ÿ���� ��ٸ��� ���� ��ų ���Ұ��� ǥ�ø� ��Ÿ���� �̹��� GameObject 
    [SerializeField] private GameObject up; // �������� ����Ʈ â ����ϴ� ��ư GameObject
    [SerializeField] private GameObject down; // �������� ����Ʈ ���� Ȯ���ϴ� ��ư GameObject
    [SerializeField] private GameObject progressingQuest; // �������� ����Ʈ â GameObject
    [SerializeField] private GameObject talkUI; // ��ȭâ GameObject
    [SerializeField] private GameObject questUI; // ����Ʈ ����â GameObject
    [SerializeField] private GameObject skillImage; // �÷��̾ ���� ��� ������ ��ų �̹��� GameObject
    [SerializeField] private GameObject inven; // �κ��丮 GameObject
    [SerializeField] private GameObject useItem; // ������ ��� ��ư GameObject
    [SerializeField] private GameObject[] invenItemSpaces; // �κ��丮 �� ������ ĭ GameObject �迭
    [SerializeField] private GameObject[] invenItemQuantitySpaces; // �κ��丮 �� ������ ���� ĭ GameObject �迭
    [SerializeField] private GameObject[] reqList; // ����Ʈ �䱸���� ���� GameObject �迭
    [SerializeField] private GameObject selectMagicStoneUI; // ������ ����â GameObject
    [SerializeField] private GameObject shopBack; // ���� ��׶��� â GameObject
    [SerializeField] private GameObject shopUI; // ���� â GameObject
    [SerializeField] private GameObject enterNumberUI; // ������ ������ ���� �Է�â GameObject
    [SerializeField] private GameObject blackOut; // ȭ�� ���ƿ� �����ϴ� �̹��� GameObject
    [SerializeField] private GameObject dragImage; // �巡�� �̹��� �纻 GameObject
    [SerializeField] private TMP_InputField itemQuantityInput; //������ ���� �Է�ĭ
    [SerializeField] private TMPro.TMP_Text level; // �÷��̾� ���� 
    [SerializeField] private TMPro.TMP_Text gold; //  �÷��̾ �������� ���
    [SerializeField] private Image hpGraph; // �÷��̾� ü�� �׷���
    [SerializeField] private TMPro.TMP_Text maxHp; // �÷��̾� ü�� �ִ�
    [SerializeField] private TMPro.TMP_Text hp; // �÷��̾� ü��
    [SerializeField] private Image mpGraph; // �÷��̾� ���� �׷���
    [SerializeField] private TMPro.TMP_Text maxMp; // �÷��̾� ���� �ִ�
    [SerializeField] private TMPro.TMP_Text mp; // �÷��̾� ���� ����
    [SerializeField] private Image expGraph; // �÷��̾� ����ġ �׷���
    [SerializeField] private TMPro.TMP_Text maxExp; // �÷��̾� ����ġ �ִ�
    [SerializeField] private TMPro.TMP_Text exp; // �÷��̾� ����ġ
    [SerializeField] private Image skill; // �÷��̾ ���� ��� ������ ��ų �̹���
    [SerializeField] private TMPro.TMP_Text coolTime; // ��ų ��Ÿ��
    [SerializeField] private Image npcImage; // npc �̹���
    [SerializeField] private TMPro.TMP_Text npcName; // npc �̸�
    [SerializeField] private TMPro.TMP_Text talk; // npc�� ���
    [SerializeField] private TMPro.TMP_Text questT; // ����Ʈ ����
    [SerializeField] private TMPro.TMP_Text questTitle; // ����Ʈ ����
    [SerializeField] private TMPro.TMP_Text questNpc; // ����Ʈ�� �� npc
    [SerializeField] private TMPro.TMP_Text questNpcName; // ����Ʈ�� �� npc �̸�
    [SerializeField] private TMPro.TMP_Text questInfo; // ����Ʈ ����
    [SerializeField] private TMPro.TMP_Text[] questReqName; // ����Ʈ �䱸���� ���� ���� ������ �迭
    [SerializeField] private TMPro.TMP_Text itemName; // ������ �̸�
    [SerializeField] private TMPro.TMP_Text itemInfo; // ������ ����
    [SerializeField] private Image[] shopSelectedItem; // �������� ���õ� �������� �������� ǥ���ϱ� ���� ���� �̹��� �迭
    [SerializeField] private Image[] invenItemList; // �κ��丮 �� ĭ�� �� ������ �̹��� �迭
    [SerializeField] private TMPro.TMP_Text[] invenItemQuantityList; // �κ��丮 �� ĭ�� �� ������ ���� �迭
    [SerializeField] private Sprite[] npcImages; // npc ��������Ʈ(�̹���) �迭
    [SerializeField] private Sprite[] skImages; // ��ų ��������Ʈ(�̹���) �迭
    [SerializeField] private Sprite[] itemImages; // ������ ��������Ʈ(�̹���) �迭

    IEnumerator talkAni; // ��ȭ ��� ��� ������ �����ϴ� TalkAnime �ڷ�ƾ ����
    IEnumerator waitForTeleport; // �ڷ���Ʈ ��� ������ �����ϴ� WaitForTeleport �ڷ�ƾ ����
    WaitForSeconds wfs; // ��� �ð�
    WaitForSeconds wfs2; // ��� �ð�

    int curMapNum = 0; // ���� �� ��ȣ
    int curNpcId = 0; // ���� �÷��̾�� �������� npc ���̵�
    int curLevel = 1; // �÷��̾��� ���� ����
    int curGold = 1000; //  �÷��̾ ���� �������� ���
    int curHp = 100; // �÷��̾��� ���� ü��
    int curMaxHp = 100; // �÷��̾��� ���� ü�� �ִ�
    int curMp = 100; // �÷��̾��� ���� ����
    int curMaxMp = 100; // �÷��̾��� ���� ���� �ִ�
    int curExp = 0; // �÷��̾��� ���� ����ġ
    int curMaxExp = 100; // �÷��̾��� �������� �ʿ��� ���� ����ġ �ִ�
    int curQuestNum = 1; // ���� ���� ������ Ȥ�� �������� ����Ʈ ��ȣ
    int curQuestNpcId; // ���� ���� ������ Ȥ�� �������� ����Ʈ�� ������ �ִ� npc�� ���̵�
    int finishReqNum = 0; // ����Ʈ ���ǵ鿡�� �Ϸ��� ���� ��
    int selectedInvenItemLoc = -1; // ���õ� �������� ĭ ��ġ(�κ��丮), -1 : ���õ� �������� ������ �ǹ�
    int selectedShopItemLoc = -1; // ���õ� �������� ĭ ��ġ(����), -1 : ���õ� �������� ������ �ǹ�
    int curUsedItemLoc = -1; // ���� ����� �������� ĭ ��ġ(�κ��丮), -1 : ����� �������� ����
    int curDragItemLoc = -1; // ���� �巡�� �ǰ� �ִ� ������ �κ��丮 �� ��ġ, -1 : �巡�� ���� �������� ����
    int monsterPower = 0; // ������ ���ݷ�
    int[] curShortCutPotions = new int[]{ -1, -1 }; // ���� ����Ű�� �÷��� �ִ� ������ ������ ���̵� ���
    float percent = 0; // �׷��� ���� Ȥ�� ���� ������ �ۼ�Ʈ ��
    string tmp = null; // ��� ��¿� ���� �ӽ� ���ڿ�
    char[] textData = null; // npc�� ��縦 ��Ÿ���� ���� �迭
    bool isTalking = false; // ��ȭ�� �� ��簡 ��µǴ� ������ ����
    bool teleportReady = false; // �ڷ���Ʈ �غ� �Ϸ� ����
    bool success = false; // ����Ʈ ���� ����
    bool finishTalk = false; // ��ȭ ���� ����
    bool skDisable = false; // ��ų ���Ұ��� ����
    bool accept = false; // ����Ʈ ���� ����
    bool waterSelected = false; // �� ������ ���� ����
    bool dirtSelected = false; // �� ������ ���� ����
    bool windSelected = false; // �ٶ� ������ ���� ����
    bool noEmptySpace = false; // �κ��丮 �� ������� ������ ����
    bool invenOn = false; // �κ��丮 â�� ���� �ִ��� ����
    bool shopExit = false; // ���� ���� ����
    bool clickLock = false; // �� �տ� �ٸ� UI â�� ���������� ���� UI�鿡 ���� ���콺 Ŭ�� �̺�Ʈ �����ϵ��� ��
    List<int> curQuestReqNum = new List<int>(); // ���� �������� ����Ʈ ������ �ʿ��� ���� ���
    Vector3 playerPos; // �÷��̾��� ���� ��ġ
    InventoryManager im; // �κ��丮 ������ InventoryManager Ŭ���� ��ü
    Animator ani; // ����Ƽ �ִϸ��̼� ������Ʈ

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.ani = blackOut.GetComponent<Animator>(); // blackOut GameObject ���� Animator ������Ʈ�� �����ɴϴ�.
        this.talkAni = TalkAnime(); // �ڷ�ƾ �Ҵ�
        this.waitForTeleport = WaitForTeleport(); // �ڷ�ƾ �Ҵ�
        this.wfs = new WaitForSeconds(0.05f); // ��� �ð�
        this.wfs2 = new WaitForSeconds(0.5f); // ��� �ð�
        this.im = inven.GetComponent<InventoryManager>(); // inven GameObject ���� Inventory Manager Ŭ���� ������Ʈ�� �����ɴϴ�.

        //���� ������ ����&�ε� ���� �� �����ؾ� ��
        this.curGold = Convert.ToInt32(gold.text); // ���� �÷��̾ �������� ���
        this.curHp = Convert.ToInt32(hp.text); // ���� �÷��̾��� ü��
        this.curMaxHp = Convert.ToInt32(maxHp.text); // ���� �÷��̾��� ü�� �ִ�
        this.curMp = Convert.ToInt32(mp.text); // ���� �÷��̾��� ����
        this.curMaxMp = Convert.ToInt32(maxMp.text); // ���� �÷��̾��� ���� �ִ�
        this.curMaxExp = Convert.ToInt32(maxExp.text); // ���� �÷��̾��� ����ġ �ִ�
        this.curQuestNpcId = QuestManager.instance.QuestDataList[curQuestNum - 1].NpcId; // ���� ���డ���� ����Ʈ�� ������ �ִ� npc ���̵�
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
     * Description : �ڷ���Ʈ �غ� �Ϸ���� ��ٸ��� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : �ڷ���Ʈ �� ���� �� ��ȣ�� �̵� �� �� ��ȣ�� �ٲٴ� ������ �����ϴ� �޼����Դϴ�.
     * Parameter : int n - �� ��ȣ
     * Return Value : void
     */
    public void TeleportMap(int n)
    {
        curMapNum = n;
        teleportReady = false;
    }

    /* Coroutine : WaitForTeleport
     * Description : �ڷ���Ʈ �غ���� �����ص� ��� �ð���ŭ ��ٸ��� �غ� �Ϸ�Ǹ� teleportReady�� true��, �׸��� �ڷ���Ʈ�� ���������� �ش� �ð���ŭ ����ϴ� ������ �����ϴ� �ڷ�ƾ�Դϴ�.
     * �ڷ���Ʈ �� ������ ȭ�� ���ƿ� ������ ����Ǿ �ڷ���Ʈ�� ������ �ش� ��ü�� ��Ȱ��ȭ�ϵ��� �����߽��ϴ�.
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
     * Description : ��ų ��� �Ұ��� ���·� ��ȯ�ϴ� �޼����Դϴ�. �׸��� ��ų ��� �Ұ��� ���¶�� ���� UI�� ���������� ǥ���ϴ� �̹����� Ȱ��ȭ�ϵ��� �����߽��ϴ�.
     * Return Value : void
     */
    public void SetSkillDisable()
    {
        skDisable = true;
        skillLocked.SetActive(true);
    }

    /* Method : ResetSkillDisable
     * Description : ��ų ��� ���� ���·� ��ȯ�ϴ� �޼����Դϴ�. �׸��� ��ų ��� �Ұ��� ���¸� ��Ÿ���� �̹����� ��Ȱ��ȭ�ϵ��� �����߽��ϴ�.
     * Return Value : void
     */
    public void ResetSkillDisable()
    {
        skDisable = false;
        skillLocked.SetActive(false);
    }

    /* Method : CheckCoolTime
     * Description : �Ű������� ���� ���� ��Ÿ���� ��Ÿ�� UI�� ǥ���ϴ� �޼����Դϴ�. �� ���� 0�̸� ��Ÿ���� �����ٴ� ���̹Ƿ� UI�� ���ϴ�. 
     * Parameter : float num - ���� ��Ÿ�� �ð�(�� ����)
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
     * Description : �Ű������� ���� ����ŭ ��带 ������Ű�� �޼����Դϴ�.
     * Parameter : int n - ��� ��
     * Return Value : void
     */
    public void GoldIncrease(int n)
    {
        curGold += n;
        gold.text = Convert.ToString(curGold);
    }

    /* Method : GoldDecrease
     * Description : �Ű������� ���� ����ŭ ��带 ���ҽ�Ű�� �޼����Դϴ�.
     * Parameter : int n - ��� ��
     * Return Value : void
     */
    public void GoldDecrease(int n)
    {
        curGold -= n;
        gold.text = Convert.ToString(curGold);
    }

    /* Method : HpUp
     * Description : �Ű������� ���� ����ŭ ü���� �ø��� �޼����Դϴ�.
     * Parameter : int n - ü�� ��
     * Return Value : void
     */
    public void HpUp(int n) // �ִ� �ʰ� ���� �˻����� ���߿� �߰��ؾ���
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
     * Description : �Ű������� ���� ����ŭ ü���� ������ �޼����Դϴ�.
     * Parameter : int n - ü�� ��
     * Return Value : void
     */
    public void HpDown(int n) // 0 ���� ���� �˻����� ���߿� �߰��ؾ���
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
            //���� ���� �޼��� �߰��ؾ� ��
        }
        hp.text = Convert.ToString(curHp);
    }

    /* Method : MpUp
     * Description : �Ű������� ���� ����ŭ ������ �ø��� �޼����Դϴ�.
     * Parameter : int n - ���� ��
     * Return Value : void
     */
    public void MpUp(int n) // �ִ� �ʰ� ���� �˻����� ���߿� �߰��ؾ���
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
     * Description : �Ű������� ���� ����ŭ ������ ������ �޼����Դϴ�.
     * Parameter : int n - ���� ��
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
     * Description : �Ű������� ���� ����ŭ ����ġ�� �ø��� �޼����Դϴ�. �������� 100���� ���� ������ �޼��尡 ȣ��˴ϴ�.
     * Parameter : int n - ����ġ ��
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
     * Description : ������ �� ������ �����ϴ� �޼����Դϴ�. ����ġ�� 0���� �ʱ�ȭ�ϰ� ������ 1 ������ŵ�ϴ�.
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
     * Description : �κ��丮 â�� ���� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : �κ��丮 ������ ���� ��Ȳ�� ������Ʈ �ϴ� �޼����Դϴ�.
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
     * Description : �κ��丮 ������ ���� �� ������ ������ �����ִ� ������ �ϴ� �޼����Դϴ�.
     * Parameter : string name - ������ �̸�, string info - ������ ����
     * Return Value : void
     */
    public void ShowItemInfo(string name, string info)
    {
        itemName.text = name;
        itemInfo.text = info;
    }

    /* Method : ClearItemInfo
     * Description : �κ��丮 ������ ����â�� �� ���·� �ʱ�ȭ�մϴ�.
     * Return Value : void
     */
    public void ClearItemInfo()
    {
        itemName.text = null;
        itemInfo.text = null;
        CloseUseItemButton();
    }

    /* Method : OpenUseItemButton
    * Description : ������ ��� ��ư�� Ȱ��ȭ�մϴ�.
    * Return Value : void
    */
    public void OpenUseItemButton()
    {
        useItem.SetActive(true);
    }

    /* Method : CloseUseItemButton
    * Description : ������ ��� ��ư�� ��Ȱ��ȭ�մϴ�.
    * Return Value : void
    */
    public void CloseUseItemButton()
    {
        useItem.SetActive(false);
    }

    /* Method : UsePotion
     * Description : �÷��̾ ���� �������� ��� �� ���� ������ �����ϴ� �޼����Դϴ�. ���� ����� �������� �κ��丮 �� ��� ��ġ�� �ִ��� ��Ÿ���� curUsedItemLoc�� �̿��� �ش� �������� ������ ���ҽ�Ű��
     * �������� UseItem �Լ��� ȣ���մϴ�. �׸��� �κ��丮 ������ ���� ��Ȳ�� ������Ʈ�մϴ�.
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
     * Description : �κ��丮 â�� �ݴ� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void InventoryOff()
    {
        invenOn = false;
        if(selectedInvenItemLoc != -1) SelectedItemFromInven(selectedInvenItemLoc);
        inven.SetActive(false);
    }

    /* Method : ShopOn
     * Description : ���� â�� ���� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void ShopOn()
    {
        shopExit = false;
        shopBack.SetActive(true);
        shopUI.SetActive(true);
    }

    /* Method : ShopOff
     * Description : ���� â�� �ݴ� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : �������� �÷��̾��� ������ ���� ��û�� �߻��ϸ� ���� npc���� ���õ� �����۰� �Է¹��� ������ �Ű������� ������ �Ǹ� ��û ������ �����ϴ� �޼����Դϴ�. 
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
     * Description : �÷��̾ �������� ������ ���Ÿ� ���������� �Ϸ��߰ų� �� ������ �������� ȹ���� ��� ������ ȹ�濡 ���� ������ �����ϴ� �޼����Դϴ�.
     * ȹ���� �������� �̹� �κ��丮 �� �������̾��� ���������� FindItem �޼���� �ľ��� �� �������� �ƴϾ����� ItemInsert �޼��带, �������̾����� ItemItemQuantityIncrease �޼��带 ȣ���ϵ��� �����߽��ϴ�.
     * Parameter Item item - ������, int num - ����
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
     * Description : �������� ������ �������� �󸶳� �������� �� ������ �Է¹޴� ������ �����ϴ� �޼����Դϴ�. �� �� ���õ� �������� ������ ���� �Է�â�� ���� ���� �޼��带 ������������ �����߽��ϴ�
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
     * Description : �������� �����ϴ� ������ �Ͼ �� �ִ� ���� �κ��丮�� �����Դϴ�. �׷��� ������ ���̽��� ����� �˸��� ������ ���� ���� ������ �����ϵ��� �����߽��ϴ�. 
     * Parameter : int idx - ��ġ
     * Return Value : void
     */
    public void SelectedItem(int idx)
    {
        if (invenOn) SelectedItemFromInven(idx);
        else SelectedItemFromShop(idx);
    }

    /* Method : SelectedItemFromInven
     * Description : �κ��丮���� ���콺 ���� ��ư���� ���õ� �������� ��ġ�� �������� �ش� �������� ���õǾ��� �ǹ̿��� �̹��� �������� �����ϰ� ���õ� ��ġ ���� selectedInvenItemLoc�� �����ϴ� �޼����Դϴ�.
     * �̹� ���õǾ� �ִ� �̹����� �ٽ� ���õǰų� �ٸ� �̹����� �����ϸ� ������ ���õ� �̹����� ���õ��� ���� ���·� ���ư����� �����߽��ϴ�.
     * Parameter : int idx - ��ġ
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
     * Description : �������� ���콺 ���� ��ư���� ���õ� �������� ��ġ�� �������� �ش� �������� ���õǾ��� �ǹ̿��� �̹��� �������� �����ϰ� ���õ� ��ġ ���� selectedShopItemLoc�� �����ϴ� �޼����Դϴ�.
     * �̹� ���õǾ� �ִ� �̹����� �ٽ� ���õǰų� �ٸ� �̹����� �����ϸ� ������ ���õ� �̹����� ���õ��� ���� ���·� ���ư����� �����߽��ϴ�.
     * Parameter : int idx - ��ġ
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
     * Description : �÷��̾ npc�� ��ȭ �ϰ��� �� �� ����Ǵ� �޼����Դϴ�. ��ȭâ�� Ȱ��ȭ�� �� npcTalk �븮�ڸ� ���� ������ ����ִ� npc�� DialogueReady �Լ��� ȣ���ϴµ� �� �� ��ȯ���� false�� �ٷ� �޼��带
     * �������ɴϴ�. �̷��� ������ ��縦 �� �� �ǰ����� �����ϱ� ���� �� ���ھ� �ð����� �ΰ� ��µǵ��� TalkAnime �ڷ�ƾ�� �����ϴ� ������� �����߽��ϴ�. �׸��� ��簡 ������� ������ �� �� �޼��尡 ȣ��Ǹ� 
     * ���� ��縦 �������� �۾��� �������� �ʰ� ���� ������� ��簡 �ѹ��� ��µǵ��� ��ŵ ������ �����ϰ� �����߽��ϴ�. finishTalk�� true�� �� �� �޼��尡 ȣ��Ǹ� ��ȭ�� �����϶�� �ǹ��̹Ƿ� TalkDone �޼��带
     * ȣ���մϴ�.
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
     * Description : ���� ��ȭâ�� ������� ��縦 �ѹ��� ����ϴ� ��ŵ ������ �����ϴ� �޼����Դϴ�.
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
     * Description : ���� ������� ��縦 �ߴܽ�Ű�� �ӽ� ���ڿ� tmp�� �� ���·� ���� �޼����Դϴ�.
     * Return Value : void
     */
    public void ClearTalk()
    {
        isTalking = false;
        StopCoroutine(talkAni);
        tmp = null;
    }

    /* Method : TalkDone
     * Description : ��ȭ�� �����ϴ� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void TalkDone()
    {
        unLockAct();
        talkUI.SetActive(false);
        finishTalk = false;
    }

    /* Coroutine : TalkAnime
     * Description : ��縦 �ѱ��ھ� ������� �ð����� �θ鼭 ����ϴ� ������ �����ϴ� �ڷ�ƾ�Դϴ�.
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
     * Description : ���� �������� ����Ʈ ��� â�� ���� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void OpenCurQuestList()
    {
        down.SetActive(false);
        up.SetActive(true);
        progressingQuest.SetActive(true);
    }

    /* Method : CloseCurQuestList
     * Description : ���� �������� ����Ʈ ��� â�� �ݴ� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void CloseCurQuestList()
    {
        down.SetActive(true);
        up.SetActive(false);
        progressingQuest.SetActive(false);
    }

    /* Method : ChangeTalkNpc
     * Description : �Ű������� ���� npc ���̵�� �̸����� ��ȭâ���� npc�� �̹��� �κа� �̸��� �˸°� �����ϴ� �޼����Դϴ�.
     * Parameter : int id - ���̵�, string name - �̸�
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
     * Description : ����Ʈ ����, ���� ��ư�� Ȱ��ȭ�ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void OpenYesOrNoButton()
    {
        yes.SetActive(true);
        no.SetActive(true);
    }

    /* Method : CloseYesOrNoButton
     * Description : ����Ʈ ����, ���� ��ư�� ��Ȱ��ȭ�ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void CloseYesOrNoButton()
    {
        yes.SetActive(false);
        no.SetActive(false);
    }

    /* Method : QuestAccept
     * Description : �÷��̾ ����Ʈ ���� �� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : �÷��̾ ����Ʈ ���� �� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : �÷��̾ ������ ����Ʈ�� ���� ������ ����Ʈ ����â�� ��Ÿ���� �޼����Դϴ�.
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
     * Description : �������� ����Ʈ�� ���� �� �÷��̾ �ش� ����Ʈ �Ϸ����ǿ� �ش��ϴ� ������ �����Ͽ����� Ȯ���ϰ� ���� �ش��ϸ� ���� ������ ���� ���¸� ������Ʈ�ϴ� ������ �����ϴ� �޼����Դϴ�.
     * ���� ���� ���� ����Ʈ�� ������ �޼��带 �������ɴϴ�. ������ �˻��� �� id�� ����Ʈ ���� 0���� ���� ��� ������ �ĺ��ؾ� �� ���̵� ������ ���ٴ� ���� �ǹ��մϴ�. ��, type�� ��ġ�ϸ� ����Ʈ �䱸 ���ǿ�
     * �ش��Ѵٴ� �ǹ��Դϴ�.
     * Parameter : string type - ����Ʈ ���� ����, int id - ����Ʈ ���ǿ� �ش��ϴ� ������ �ĺ��ϴ� ���̵�(ex : ���̵� 1 �� ���� óġ)
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
     * Description : ����Ʈ ���� �� ������ �����ϴ� �޼����Դϴ�. �����ߴٴ� ���� ���������� �����ֱ� ���� ����Ʈ ����â�� ����Ʈ ������ ǥ���ϴ� �ؽ�Ʈ ������ ��� ��������� ����ǵ��� �����߽��ϴ�.
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
     * Description : ����Ʈ �Ϸ� �� ������ �����ϴ� �޼����Դϴ�. ����Ʈ ������ ǥ�õǾ� �ִ� ����â�� ��Ȱ��ȭ�ϰ� ���� �Ϸ��� ����Ʈ ��ȣ�� 1�̸� ��ų �̹����� UI�� ǥ�õǵ��� �����߽��ϴ�.
     * �׸��� ����Ʈ ������ �÷��̾ �޵��� QuestReward �޼��带 ȣ���մϴ�. �� �ش� ����Ʈ ���� ������ �����ϴ� curQuestReqNum ����Ʈ�� ����ִ� ������ ��� �ʱ�ȭ�մϴ�. �׸��� ���� ����Ʈ�� �����ϸ�
     * qm���Լ� �ش� ����Ʈ�� ���� npc ���̵� ������ curQuestNpcId�� �����ϵ��� �����߽��ϴ�. 
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
     * Description : �÷��̾ �Ϸ��� ����Ʈ ������ �����ϴ� �޼����Դϴ�.(���� �Ϻ� �̱��� ����)
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
     * Description : ������ ����â�� ���� �޼����Դϴ�. 1�� ����Ʈ�� ���� ������ ���°� �ƴϸ� �޼��带 �������ɴϴ�.
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
     * Description : ������ ����â�� �ݴ� �޼����Դϴ�. 
     * Return Value : void
     */
    public void SelectMagicStoneUIOff()
    {
        selectMagicStoneUI.SetActive(false);
        unLockAct();
    }

    /* Method : SwitchingMagicStone
     * Description : ������ ���� �� ���õ� �������� ������ �ľ��ϰ� �ش� ��ų �̹����� UI�� ��Ÿ���� �÷��̾ �ش� ��ų�� ����� �� �ֵ��� �Ű������� �ش� ��ų ��ü�� �����ϴ� �޼����Դϴ�. 
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
        QuestUpdate("����������");
        SelectMagicStoneUIOff();
    }
}
