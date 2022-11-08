using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : BackGroundMusicManager
 * Description : ���� ������� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class BackGroundMusicManager : MonoBehaviour
{
    public static BackGroundMusicManager instance; // �̱��� ����
    [SerializeField] private AudioClip[] bgmList; // �� ���� ��������� ��Ÿ��
    AudioSource audioSource; // ����� Ŭ���� ����ϱ� ���� ����� �ҽ�

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ�� �����ɴϴ�.
        this.audioSource.loop = true; // ��������� �ݺ� ����� �� �ְ� ������ �����մϴ�.
        PlayBGM(GameManager.instance.CurMapNum); // ���� ��ġ�� ���� bgm�� ����մϴ�.
    }

    /* Method : PlayBGM
     * Description : �÷��̾ ���� ��ġ�� ���� ��������� ����մϴ�.
     * Parameter : int idx - �� ��ȣ
     * Return Value : void
     */
    public void PlayBGM(int idx)
    {
        audioSource.clip = bgmList[idx];
        audioSource.Play();
    }
    /* Method : StopBGM
     * Description : ���� ��� ���̴� ������ �����մϴ�.
     * Return Value : void
     */
    public void StopBGM()
    {
        if (audioSource.clip != null) audioSource.Stop();
    }

}
