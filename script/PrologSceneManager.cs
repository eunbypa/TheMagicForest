using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* Class : PrologSceneManager
 * Description : 프롤로그 씬 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class PrologSceneManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text prologText; // 프롤로그 문장을 출력하는 곳
    [SerializeField] private GameObject loadingUI; // 게임 로딩창 GameObject
    [SerializeField] private Image loadingBar; // 로딩 그래프 바
    char[] LineSeperate = new char[] { '\n' }; // 줄바꿈
    List<string> prolog = new List<string>(); // 프롤로그 문장 리스트
    WaitForSeconds wfs; // 대기 시간
    WaitForSeconds wfs2; // 대기 시간
    bool loadingStart = false; // 다음 씬 로딩 여부
    AsyncOperation loadingNextScene; // 비동기
    Animator ani;
    string s;
    void Awake()
    {
        TextAsset textset = Resources.Load<TextAsset>("PrologScript");
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in Lines)
        {
            s = line;
            s = s.Replace("쉼표", ",");
            prolog.Add(s);
        }
    }
    void Start()
    {
        this.wfs = new WaitForSeconds(4f); // 대기 시간
        this.wfs2 = new WaitForSeconds(0.25f); // 대기 시간
        this.ani = prologText.GetComponent<Animator>();
        StartCoroutine(ShowPrologStory());
    }

    void Update()
    {
        if (loadingStart) // 로딩이 시작 되었는지
        {
            if (!loadingNextScene.isDone) // 아직 로딩이 완료되지 않았다면
            {
                if (loadingBar.fillAmount < loadingNextScene.progress) loadingBar.fillAmount += loadingNextScene.progress * Time.deltaTime; // 현재 로딩이 진행된 상황까지 그래프를 증가시킴
                if (loadingBar.fillAmount >= 0.9f) // 로딩이 거의 끝나가면
                {
                    loadingBar.fillAmount = 1f; // 로딩 그래프를 1로 설정
                    loadingNextScene.allowSceneActivation = true; // 씬 전환이 이루어지도록 true로 설정
                }
            }
        }
    }
    /* Method : SkipProlog
    * Description : 프롤로그를 스킵하고 메인 씬으로 넘어가는 메서드입니다.
    * Return Value : void
    */
    public void SkipProlog()
    {
        StopCoroutine(ShowPrologStory());
        prologText.text = null;
        LoadMainScene();
    }
    /* Method : LoadMainScene
    * Description : 메인 씬으로 넘어가는 메서드입니다.
    * Return Value : void
    */
    public void LoadMainScene()
    {
        loadingUI.SetActive(true);
        loadingBar.fillAmount = 0f;
        loadingStart = true;
        loadingNextScene = SceneManager.LoadSceneAsync("MainScene");
        loadingNextScene.allowSceneActivation = false;
    }
    /* Coroutine : ShowPrologStory
     * Description : 프롤로그 이야기를 한 문장씩 일정 시간 간격으로 출력하는 코루틴입니다.
     */
    IEnumerator ShowPrologStory()
    {
        for (int i = 0; i < prolog.Count; i++)
        {
            prologText.text = prolog[i];
            ani.SetTrigger("set");
            yield return wfs2;
            yield return wfs;
            ani.SetTrigger("reset");
            yield return wfs2;
            prologText.text = null;
            yield return null;
        }
        LoadMainScene();
    }
}
