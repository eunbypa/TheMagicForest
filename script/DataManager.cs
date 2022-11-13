using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using System.Text;

/* Class : DataManager
 * Description : ����� ���� �����͸� �ҷ����ų� ���� ���� �����͸� �����ϴ� ������ ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class DataManager : MonoBehaviour
{
    public static DataManager instance; // �̱��� ����
    SaveData data; // ���� ������
    string fileName = "saveData"; // ���� ���� �̸�
    string filePath; // ���� ���
    /* Property */
    public SaveData Data
    {
        get
        {
            return data;
        }
    }
    /* Property */
    public string FilePath
    {
        get
        {
            return filePath;
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        filePath = Application.dataPath + "/" + fileName + ".Json";
    }

    void Start()
    {

    }
    /* Method : Init
    * Description : Ÿ��Ʋ ������ �� ���� ���� �� ���� �����Ͱ� �����ϸ� �ʱ�ȭ�ϰ� �� �����͸� �����ϴ� �޼����Դϴ�.
    * Return Value : void
    */
    public void Init()
    {
        data = new SaveData();
        data.name = TitleSceneManager.instance.PlayerName;
        WriteOnFile();
    }

    /* Method : Store
    * Description : ���� ��ư�� Ŭ���� ������ ���� �����͵��� ��� JSON ���Ͽ� �����ϴ� �޼����Դϴ�.
    * Return Value : void
    */
    public void Store()
    {
        data = new SaveData();
        data.playerPos = GameManager.instance.PlayerPos;
        data.curMapNum = GameManager.instance.CurMapNum;
        data.level = GameManager.instance.CurLevel;
        data.name = GameManager.instance.PlayerName;
        data.gold = GameManager.instance.CurGold;
        data.curHp = GameManager.instance.CurHp;
        data.maxHp = GameManager.instance.CurMaxHp;
        data.curMp = GameManager.instance.CurMp;
        data.maxMp = GameManager.instance.CurMaxMp;
        data.curExp = GameManager.instance.CurExp;
        data.maxExp = GameManager.instance.CurMaxExp;
        if (GameManager.instance.WaterSelected)
        {
            data.magicStone = "���Ǹ�����";
        }
        else if (GameManager.instance.DirtSelected)
        {
            data.magicStone = "���Ǹ�����";
        }
        else if (GameManager.instance.WindSelected)
        {
            data.magicStone = "�ٶ��Ǹ�����";
        }
        if (GameManager.instance.CurShortCutPotions[0] != -1 || GameManager.instance.CurShortCutPotions[1] != -1)
        {
            data.shortCutPotions = GameManager.instance.CurShortCutPotions;
        }
        data.shortCutPotionsQuantity = new int[2];
        for (int i = 0; i < 2; i++)
        {
            if (InventoryManager.instance.FindItem(GameManager.instance.CurShortCutPotions[i]) != -1)
            {
                data.shortCutPotionsQuantity[i] = InventoryManager.instance.InvenItemQuantityList[InventoryManager.instance.FindItem(GameManager.instance.CurShortCutPotions[i])];
            }
        }
        data.invenItemIdList = new List<int>();
        for (int i = 0; i < InventoryManager.instance.InvenItemList.Count; i++)
        {
            data.invenItemIdList.Add(InventoryManager.instance.InvenItemList[i].ItemId);
        }
        data.invenItemQuantityList = InventoryManager.instance.InvenItemQuantityList;
        data.curQuestNum = GameManager.instance.CurQuestNum;
        data.curQuestNpcId = GameManager.instance.CurQuestNpcId;
        data.accept = GameManager.instance.Accept;
        data.success = GameManager.instance.Success;
        if (GameManager.instance.Accept) data.curQuestReqNum = GameManager.instance.CurQuestReqNum;
        WriteOnFile();
    }
    /* Method : WriteOnFile
    * Description : JSON ���Ͽ� ���� ������ �ϴ� �޼����Դϴ�.
    * Return Value : void
    */
    public void WriteOnFile()
    {
        string json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(json);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }
    /* Method : Load
    * Description : JSON ���Ͽ��� ����� �����͸� �ҷ����� �޼����Դϴ�.
    * Return Value : void
    */
    public void Load()
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        byte[] byteData = new byte[fileStream.Length];
        fileStream.Read(byteData, 0, byteData.Length);
        fileStream.Close();
        string json = Encoding.UTF8.GetString(byteData);
        //Debug.Log(json);
        data = JsonUtility.FromJson<SaveData>(json);
    }
}
