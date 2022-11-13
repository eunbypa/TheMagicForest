using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�߻� Ŭ���� ��� ���� : ���߿� �� Ŭ������ ��ӹ��� ���� Ŭ�������� �޼������ ������ �� �� �ֵ��� ��
public abstract class State<T> where T : class
{
    // ���� ����
    public abstract void Enter(T entity);
    //���� ����
    public abstract void Execute(T entity);
    //���� Ż��
    public abstract void Exit(T entity);
}
