using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : BackGroundMusicManager
 * Description : 게임 배경음악 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class BackGroundMusicManager : MonoBehaviour
{
    public static BackGroundMusicManager instance; // 싱글톤 패턴
    [SerializeField] private AudioClip[] bgmList; // 각 맵의 배경음악을 나타냄
    AudioSource audioSource; // 오디오 클립을 재생하기 위한 오디오 소스

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 가져옵니다.
        this.audioSource.loop = true; // 배경음악이 반복 재생될 수 있게 루프를 설정합니다.
        PlayBGM(GameManager.instance.CurMapNum); // 현재 위치한 맵의 bgm을 재생합니다.
    }

    /* Method : PlayBGM
     * Description : 플레이어가 현재 위치한 맵의 배경음악을 재생합니다.
     * Parameter : int idx - 맵 번호
     * Return Value : void
     */
    public void PlayBGM(int idx)
    {
        audioSource.clip = bgmList[idx];
        audioSource.Play();
    }
    /* Method : StopBGM
     * Description : 현재 재생 중이던 음악을 중지합니다.
     * Return Value : void
     */
    public void StopBGM()
    {
        if (audioSource.clip != null) audioSource.Stop();
    }

}
