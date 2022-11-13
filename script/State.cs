using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상 클래스 사용 이유 : 나중에 이 클래스를 상속받은 하위 클래스에서 메서드들을 재정의 할 수 있도록 함
public abstract class State<T> where T : class
{
    // 상태 진입
    public abstract void Enter(T entity);
    //상태 진행
    public abstract void Execute(T entity);
    //상태 탈출
    public abstract void Exit(T entity);
}
