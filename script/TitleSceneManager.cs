using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

/* Class : TitleSceneManager
 * Description : Ÿ��Ʋ �� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class TitleSceneManager : MonoBehaviour
{
    public static TitleSceneManager instance; // �̱��� ����
    [SerializeField] private GameObject resetAlarmUI; // ������ ����� �����Ͱ� �ִ� ���¿��� �� ���� ��ư Ŭ�� �� ������ ���� ���â UI GameObject
    [SerializeField] private GameObject noDataAlarmUI; // ������ ����� �����Ͱ� ���ٴ� �˸�â UI GameObject
    [SerializeField] private GameObject gameInfoUI; // ���� ��� UI GameObject
    [SerializeField] private GameObject enteringNameUI; // �÷��̾� �̸� �Է� UI GameObject
    [SerializeField] private GameObject loadingUI; // ���� �ε�â GameObject
    [SerializeField] private TMP_InputField playerNameInput; // �÷��̾� �̸� �Է�ĭ
    [SerializeField] private Image loadingBar; // �ε� �׷��� ��

    bool loadingStart = false; // ���� �� �ε� ����
    string playerName; // �÷��̾� �̸�
    AsyncOperation loadingNextScene; // �񵿱�
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
        if (loadingStart) // �ε��� ���� �Ǿ�����
        {
            if (!loadingNextScene.isDone) // ���� �ε��� �Ϸ���� �ʾҴٸ�
            {
                if(loadingBar.fillAmount < loadingNextScene.progress) loadingBar.fillAmount += loadingNextScene.progress * Time.deltaTime; // ���� �ε��� ����� ��Ȳ���� �׷����� ������Ŵ
                if (loadingBar.fillAmount >= 0.9f) // �ε��� ���� ��������
                { 
                    loadingBar.fillAmount = 1f; // �ε� �׷����� 1�� ����
                    loadingNextScene.allowSceneActivation = true; // �� ��ȯ�� �̷�������� true�� ����
                }
            }
        }
    }
    /* Method : StartNewGame
    * Description : �� ���� ��ư�� Ŭ���Ͽ� ���� ������ ���۽� ���� ������ �����ϴ� �޼����Դϴ�.
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
    * Description : �Է� ���� �г����� ���ǿ� �����ϴ��� Ȯ���ϰ� ���� ������ �����ϴ� �޼����Դϴ�.
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
    * Description : ���� ����� ���� ������ �����ִ� �޼����Դϴ�.
    * Return Value : void
    */
    public void ShowGameKeySettingInfo()
    {
       
    }
    /* Method : CloseGameKeySettingInfo
    * Description : ���� ��� ����â�� �ݴ� �޼����Դϴ�.
    * Return Value : void
    */
    public void CloseGameKeySettingInfo()
    {

    }
    /* Method : LoadGame
    * Description : ���� �����͸� �ҷ��� ���� ���� ������ �Ѿ�� �޼����Դϴ�.
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
    * Description : ���ѷα� ������ ��ȯ�ϴ� �޼����Դϴ�.
    * Return Value : void
    */
    public void LoadPrologScene()
    {
        SceneManager.LoadScene("PrologScene");
    }
}
