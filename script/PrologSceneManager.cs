using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* Class : PrologSceneManager
 * Description : ���ѷα� �� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class PrologSceneManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text prologText; // ���ѷα� ������ ����ϴ� ��
    [SerializeField] private GameObject loadingUI; // ���� �ε�â GameObject
    [SerializeField] private Image loadingBar; // �ε� �׷��� ��
    char[] LineSeperate = new char[] { '\n' }; // �ٹٲ�
    List<string> prolog = new List<string>(); // ���ѷα� ���� ����Ʈ
    WaitForSeconds wfs; // ��� �ð�
    WaitForSeconds wfs2; // ��� �ð�
    bool loadingStart = false; // ���� �� �ε� ����
    AsyncOperation loadingNextScene; // �񵿱�
    Animator ani;
    string s;
    void Awake()
    {
        TextAsset textset = Resources.Load<TextAsset>("PrologScript");
        string[] Lines = textset.text.Split(LineSeperate, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in Lines)
        {
            s = line;
            s = s.Replace("��ǥ", ",");
            prolog.Add(s);
        }
    }
    void Start()
    {
        this.wfs = new WaitForSeconds(4f); // ��� �ð�
        this.wfs2 = new WaitForSeconds(0.25f); // ��� �ð�
        this.ani = prologText.GetComponent<Animator>();
        StartCoroutine(ShowPrologStory());
    }

    void Update()
    {
        if (loadingStart) // �ε��� ���� �Ǿ�����
        {
            if (!loadingNextScene.isDone) // ���� �ε��� �Ϸ���� �ʾҴٸ�
            {
                if (loadingBar.fillAmount < loadingNextScene.progress) loadingBar.fillAmount += loadingNextScene.progress * Time.deltaTime; // ���� �ε��� ����� ��Ȳ���� �׷����� ������Ŵ
                if (loadingBar.fillAmount >= 0.9f) // �ε��� ���� ��������
                {
                    loadingBar.fillAmount = 1f; // �ε� �׷����� 1�� ����
                    loadingNextScene.allowSceneActivation = true; // �� ��ȯ�� �̷�������� true�� ����
                }
            }
        }
    }
    /* Method : SkipProlog
    * Description : ���ѷα׸� ��ŵ�ϰ� ���� ������ �Ѿ�� �޼����Դϴ�.
    * Return Value : void
    */
    public void SkipProlog()
    {
        StopCoroutine(ShowPrologStory());
        prologText.text = null;
        LoadMainScene();
    }
    /* Method : LoadMainScene
    * Description : ���� ������ �Ѿ�� �޼����Դϴ�.
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
     * Description : ���ѷα� �̾߱⸦ �� ���徿 ���� �ð� �������� ����ϴ� �ڷ�ƾ�Դϴ�.
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
