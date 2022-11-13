using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : EffectSoundManager
 * Description : 게임 효과음 관리자 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다. 
 */
public class EffectSoundManager : MonoBehaviour
{
    // 효과음을 가지는 행동들을 직관적으로 표현하기 위해 enum을 사용했습니다.
    enum EffectSounds { 
        waterMagic, dirtMagic, windMagic, hurt, levelUp, potionUse, buttonClick, sellItem, getItem, getGold
    };
    public static EffectSoundManager instance; // 싱글톤 패턴
    [SerializeField] private AudioClip[] effectSoundList; // 효과음들을 나타냄
    AudioSource audioSource; // 오디오 클립을 재생하기 위한 오디오 소스

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 가져옵니다.
    }

    /* Method : PlayEffectSound
     * Description : 해당하는 효과음을 1회 재생합니다.
     * Parameter : string action - 효과음을 유발한 행동
     * Return Value : void
     */
    public void PlayEffectSound(string action)
    {
        if (audioSource == null) return;
        switch (action)
        {
            case "waterMagic":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.waterMagic]);
                break;
            case "dirtMagic":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.dirtMagic]);
                break;
            case "windMagic":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.windMagic]);
                break;
            case "hurt":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.hurt]);
                break;
            case "levelUp":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.levelUp]);
                break;
            case "potionUse":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.potionUse]);
                break;
            case "buttonClick":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.buttonClick]);
                break;
            case "sellItem":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.sellItem]);
                break;
            case "getItem":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.getItem]);
                break;
            case "getGold":
                audioSource.PlayOneShot(effectSoundList[(int)EffectSounds.getGold]);
                break;
        }
    }
}
