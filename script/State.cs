using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : State<T>
 * Description : 상태를 나타내는 추상 제네릭 클래스입니다. 단, T가 클래스 형태일 때만 이 클래스를 사용할 수 있도록 제한했습니다. 
 */
public abstract class State<T> where T : class
{
    /* Method : Enter
     * Description : 상태 진입 시 해야 할 동작을 수행하는 추상 메서드입니다. 진입 시점에 한 번만 호출됩니다. 하위 클래스에서 재정의 하도록 abstract를 적용했습니다.
     * Parameter : T entity - 상태를 가지는 개체
     * Return Value : void
     */
    public abstract void Enter(T entity);
    /* Method : Execute
     * Description : 상태 진입 이후 실행 시 해야 할 동작을 수행하는 추상 메서드입니다. 다른 상태로 전이 되지 않는 이상 계속 호출됩니다. 하위 클래스에서 재정의 하도록 abstract를 적용했습니다.
     * Parameter : T entity - 상태를 가지는 개체
     * Return Value : void
     */
    public abstract void Execute(T entity);
    /* Method : Exit
     * Description : 상태 진출 시 해야 할 동작을 수행하는 추상 메서드입니다. 진출 시점에 한 번만 호출됩니다. 하위 클래스에서 재정의 하도록 abstract를 적용했습니다.
     * Parameter : T entity - 상태를 가지는 개체
     * Return Value : void
     */
    public abstract void Exit(T entity);
}
