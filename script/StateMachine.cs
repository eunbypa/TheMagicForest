using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� �ӽ� fsm
public class StateMachine<T> where T : class
{
    T owner; // �� ���¸ӽ��� ������ ��ü
    State<T> currentState; // ���� ����

    // ó�� ���¸� ����
    public void SetState(T owner, State<T> start)
    {
        this.owner = owner;
        currentState = null;
        ChangeState(start);
    }

    //���� ���¸� ����
    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }

    //���� ����
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
