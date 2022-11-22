using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

/* Class : TitleSceneManager
 * Description : 타이틀 씬 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class TitleSceneManager : MonoBehaviour
{
    public static TitleSceneManager instance; // 싱글톤 패턴
    [SerializeField] private GameObject resetAlarmUI; // 기존에 저장된 데이터가 있는 상태에서 새 게임 버튼 클릭 시 데이터 리셋 경고창 UI GameObject
    [SerializeField] private GameObject noDataAlarmUI; // 기존에 저장된 데이터가 없다는 알림창 UI GameObject
    [SerializeField] private GameObject gameInfoUI; // 게임 방법 UI GameObject
    [SerializeField] private GameObject enteringNameUI; // 플레이어 이름 입력 UI GameObject
    [SerializeField] private GameObject loadingUI; // 게임 로딩창 GameObject
    [SerializeField] private TMP_InputField playerNameInput; // 플레이어 이름 입력칸
    [SerializeField] private Image loadingBar; // 로딩 그래프 바

    bool loadingStart = false; // 다음 씬 로딩 여부
    string playerName; // 플레이어 이름
    AsyncOperation loadingNextScene; // 비동기
    /* Property */
    public string PlayerName
    {
        get
        {
            return playerName;
        }
    }
    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }
    void Update()
    {
        if (loadingStart) // 로딩이 시작 되었는지
        {
            if (!loadingNextScene.isDone) // 아직 로딩이 완료되지 않았다면
            {
                if(loadingBar.fillAmount < loadingNextScene.progress) loadingBar.fillAmount += loadingNextScene.progress * Time.deltaTime; // 현재 로딩이 진행된 상황까지 그래프를 증가시킴
                if (loadingBar.fillAmount >= 0.9f) // 로딩이 거의 끝나가면
                { 
                    loadingBar.fillAmount = 1f; // 로딩 그래프를 1로 설정
                    loadingNextScene.allowSceneActivation = true; // 씬 전환이 이루어지도록 true로 설정
                }
            }
        }
    }
    /* Method : StartNewGame
    * Description : 새 게임 버튼을 클릭하여 새로 게임을 시작시 관련 동작을 수행하는 메서드입니다.
    * Return Value : void
    */
    public void StartNewGame()
    {
        if (File.Exists(DataManager.instance.FilePath)) resetAlarmUI.SetActive(true);
        else
        {
            enteringNameUI.SetActive(true);
        }
    }
    /* Method : EnterNickName
    * Description : 입력 받은 닉네임이 조건에 만족하는지 확인하고 다음 동작을 수행하는 메서드입니다.
    * Return Value : void
    */
    public void EnterNickName()
    {
        if (playerNameInput.text.Length == 0 || playerNameInput.text.Length > 6) return;
        playerName = playerNameInput.text;
        DataManager.instance.Init();
        enteringNameUI.SetActive(false);
        LoadGame();
    }
    /* Method : ShowGameKeySettingInfo
    * Description : 게임 방법에 대한 정보를 보여주는 메서드입니다.
    * Return Value : void
    */
    public void ShowGameKeySettingInfo()
    {
       
    }
    /* Method : CloseGameKeySettingInfo
    * Description : 게임 방법 정보창을 닫는 메서드입니다.
    * Return Value : void
    */
    public void CloseGameKeySettingInfo()
    {

    }
    /* Method : LoadGame
    * Description : 기존 데이터를 불러온 다음 메인 씬으로 넘어가는 메서드입니다.
    * Return Value : void
    */
    public void LoadGame()
    {
        if (!File.Exists(DataManager.instance.FilePath))
        {
            noDataAlarmUI.SetActive(true);
            return;
        }
        DataManager.instance.Load();
        loadingUI.SetActive(true);
        loadingBar.fillAmount = 0f;
        loadingStart = true;
        loadingNextScene = SceneManager.LoadSceneAsync("MainScene");
        loadingNextScene.allowSceneActivation = false;
        //SceneManager.LoadScene("MainScene");
    }
    /* Method : LoadPrologScene
    * Description : 프롤로그 씬으로 전환하는 메서드입니다.
    * Return Value : void
    */
    public void LoadPrologScene()
    {
        SceneManager.LoadScene("PrologScene");
    }
}
