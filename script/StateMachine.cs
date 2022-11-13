using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유한 상태 머신 fsm
public class StateMachine<T> where T : class
{
    T owner; // 이 상태머신을 소유한 객체
    State<T> currentState; // 현재 상태

    // 처음 상태를 설정
    public void SetState(T owner, State<T> start)
    {
        this.owner = owner;
        currentState = null;
        ChangeState(start);
    }

    //현재 상태를 진행
    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }

    //상태 변경
    public void ChangeState(State<T> nextState)
    {
        //Debug.Log(nextState);
        if (nextState == null) return;
        if(currentState != null)
        {
            currentState.Exit(owner);
        }
        currentState = nextState;
        currentState.Enter(owner);
    }
}
