using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Npc
 * Description : npc를 나타내는 추상 클래스입니다. 하위 클래스에서 유니티의 생명 주기 함수들을 사용할 수 있게 MonoBehaviour 클래스를 상속받았습니다.
 */
public abstract class Npc : MonoBehaviour
{
    // [SerializeField] 는 유니티 Inspector에 해당 변수들이 표시되도록 하기 위해 사용했습니다.
    [SerializeField] private int npcId; // npc를 식별하는 아이디

    bool wait = false; // 대화 대기 상태 여부

    /* Property */
    public int NpcId
    {
        get
        {
            return npcId;
        }
    }
    /* Property */
    public bool Wait
    {
        get
        {
            return wait;
        }
        set
        {
            wait = value;
        }
    }

    /* Method : UnLockWait
     * Description : npc의 대화 대기 상태를 풀어주는 메서드입니다. 
     * Return Value : void
     */
    public virtual void UnLockWait()
    {
        wait = false;
    }

    /* Method : DialogueReady
     * Description : 하위 클래스에서 npc 대화 준비 완료 상태면 true를 대화 대기 상태면 false를 반환하는 동작을 수행하도록 구현될 추상 메서드입니다. 하위 클래스에서 반드시 재정의 하도록 abstract를 적용했습니다.
     * Return Value : bool
     */
    public abstract bool DialogueReady();

    /* Method : SetDiaState
     * Description : 하위 클래스에서 npc의 대화 상태를 설정하는 방식으로 구현될 추상 메서드입니다. 하위 클래스에서 반드시 재정의 하도록 abstract를 적용했습니다.
     * Return Value : void
     */
    public abstract void SetDiaState();

    /* Method : GetDiaData
     * Description : 하위 클래스에서 npc의 대화 상태를 기준으로 대사를 가져오는 방식으로 구현될 추상 메서드입니다. 하위 클래스에서 반드시 재정의 하도록 abstract를 적용했습니다.
     * Return Value : void
     */
    public abstract void GetDiaData();
}
