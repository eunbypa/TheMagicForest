using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : State<T>
 * Description : ���¸� ��Ÿ���� �߻� ���׸� Ŭ�����Դϴ�. ��, T�� Ŭ���� ������ ���� �� Ŭ������ ����� �� �ֵ��� �����߽��ϴ�. 
 */
public abstract class State<T> where T : class
{
    /* Method : Enter
     * Description : ���� ���� �� �ؾ� �� ������ �����ϴ� �߻� �޼����Դϴ�. ���� ������ �� ���� ȣ��˴ϴ�. ���� Ŭ�������� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Parameter : T entity - ���¸� ������ ��ü
     * Return Value : void
     */
    public abstract void Enter(T entity);
    /* Method : Execute
     * Description : ���� ���� ���� ���� �� �ؾ� �� ������ �����ϴ� �߻� �޼����Դϴ�. �ٸ� ���·� ���� ���� �ʴ� �̻� ��� ȣ��˴ϴ�. ���� Ŭ�������� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Parameter : T entity - ���¸� ������ ��ü
     * Return Value : void
     */
    public abstract void Execute(T entity);
    /* Method : Exit
     * Description : ���� ���� �� �ؾ� �� ������ �����ϴ� �߻� �޼����Դϴ�. ���� ������ �� ���� ȣ��˴ϴ�. ���� Ŭ�������� ������ �ϵ��� abstract�� �����߽��ϴ�.
     * Parameter : T entity - ���¸� ������ ��ü
     * Return Value : void
     */
    public abstract void Exit(T entity);
}
