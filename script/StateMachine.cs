using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : StateMachine<T>
 * Description : 소유자의 상태 전이를 관리하는 유한상태머신(FSM) 제네릭 클래스입니다. 단, T가 클래스 형태일 때만 이 클래스를 사용할 수 있도록 제한했습니다. 
 */
public class StateMachine<T> where T : class
{
    T owner; // 이 상태머신을 소유한 객체
    State<T> currentState; // 현재 상태

    /* Method : SetState
     * Description : FSM의 소유자를 등록하고, 처음 상태를 설정하는 메서드입니다.
     * Parameter : T owner - FSM의 소유자, State<T> start - 시작 상태
     * Return Value : void
     */
    public void SetState(T owner, State<T> start)
    {
        this.owner = owner;
        currentState = null;
        ChangeState(start);
    }
    /* Method : Execute
     * Description : 현재 상태의 동작을 실행하는 메서드입니다.
     * Return Value : void
     */
    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }
    /* Method : ChangeState
     * Description : 현재 상태에서 다음 상태로 전이하는 동작을 수행하는 메서드입니다. 현재 상태가 null이 아니라면 해당 상태의 Exit 메서드를 호출하고 전이합니다.
     * Parameter : State<T> nextState - 다음 상태
     * Return Value : void
     */
    public void ChangeState(State<T> nextState)
    {
        if (nextState == null) return;
        if(currentState != null)
        {
            currentState.Exit(owner);
        }
        currentState = nextState;
        currentState.Enter(owner);
    }
}
