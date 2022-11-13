using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : EffectSoundManager
 * Description : ���� ȿ���� ������ Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class EffectSoundManager : MonoBehaviour
{
    // ȿ������ ������ �ൿ���� ���������� ǥ���ϱ� ���� enum�� ����߽��ϴ�.
    enum EffectSounds { 
        waterMagic, dirtMagic, windMagic, hurt, levelUp, potionUse, buttonClick, sellItem, getItem, getGold
    };
    public static EffectSoundManager instance; // �̱��� ����
    [SerializeField] private AudioClip[] effectSoundList; // ȿ�������� ��Ÿ��
    AudioSource audioSource; // ����� Ŭ���� ����ϱ� ���� ����� �ҽ�

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ�� �����ɴϴ�.
    }

    /* Method : PlayEffectSound
     * Description : �ش��ϴ� ȿ������ 1ȸ ����մϴ�.
     * Parameter : string action - ȿ������ ������ �ൿ
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
