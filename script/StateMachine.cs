using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : StateMachine<T>
 * Description : �������� ���� ���̸� �����ϴ� ���ѻ��¸ӽ�(FSM) ���׸� Ŭ�����Դϴ�. ��, T�� Ŭ���� ������ ���� �� Ŭ������ ����� �� �ֵ��� �����߽��ϴ�. 
 */
public class StateMachine<T> where T : class
{
    T owner; // �� ���¸ӽ��� ������ ��ü
    State<T> currentState; // ���� ����

    /* Method : SetState
     * Description : FSM�� �����ڸ� ����ϰ�, ó�� ���¸� �����ϴ� �޼����Դϴ�.
     * Parameter : T owner - FSM�� ������, State<T> start - ���� ����
     * Return Value : void
     */
    public void SetState(T owner, State<T> start)
    {
        this.owner = owner;
        currentState = null;
        ChangeState(start);
    }
    /* Method : Execute
     * Description : ���� ������ ������ �����ϴ� �޼����Դϴ�.
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
     * Description : ���� ���¿��� ���� ���·� �����ϴ� ������ �����ϴ� �޼����Դϴ�. ���� ���°� null�� �ƴ϶�� �ش� ������ Exit �޼��带 ȣ���ϰ� �����մϴ�.
     * Parameter : State<T> nextState - ���� ����
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
