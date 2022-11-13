using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using System.Text;

/* Class : DataManager
 * Description : 저장된 게임 데이터를 불러오거나 새로 게임 데이터를 저장하는 데이터 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class DataManager : MonoBehaviour
{
    public static DataManager instance; // 싱글톤 패턴
    SaveData data; // 저장 데이터
    string fileName = "saveData"; // 저장 파일 이름
    string filePath; // 파일 경로
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
    * Description : 타이틀 씬에서 새 게임 시작 시 기존 데이터가 존재하면 초기화하고 새 데이터를 세팅하는 메서드입니다.
    * Return Value : void
    */
    public void Init()
    {
        data = new SaveData();
        data.name = TitleSceneManager.instance.PlayerName;
        WriteOnFile();
    }

    /* Method : Store
    * Description : 저장 버튼을 클릭한 시점의 게임 데이터들을 모두 JSON 파일에 저장하는 메서드입니다.
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
            data.magicStone = "물의마법석";
        }
        else if (GameManager.instance.DirtSelected)
        {
            data.magicStone = "흙의마법석";
        }
        else if (GameManager.instance.WindSelected)
        {
            data.magicStone = "바람의마법석";
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
    * Description : JSON 파일에 쓰는 동작을 하는 메서드입니다.
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
    * Description : JSON 파일에서 저장된 데이터를 불러오는 메서드입니다.
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
