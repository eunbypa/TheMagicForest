using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Class : Monster
 * Description : 게임의 몬스터를 담당하는 클래스입니다. 
 */
public class Monster : MonoBehaviour
{
    [SerializeField] private int monsterId; // 몬스터를 식별하는 아이디
    [SerializeField] private int realMapNum; // 맵 기존 번호
    [SerializeField] private int expValue; // 물리치면 얻을 수 있는 경험치
    [SerializeField] private int attackArea; // 공격 범위
    [SerializeField] protected int locatedMapNum; // 몬스터가 있는 맵만 가지는 별도의 번호(기존 맵 번호와는 별도의 값, 몬스터가 있는 맵만을 기준으로 함)
    [SerializeField] protected int attackPower; // 공격력
    [SerializeField] protected int maxHp; // 전체 체력
    [SerializeField] protected int speed; // 속도
    [SerializeField] protected int defaultDirection; // 몬스터의 기본 시선 방향 (-1 : 왼쪽, 1 : 오른쪽)
    [SerializeField] protected Image hpGraph; // 체력 그래프
    [SerializeField] protected GameObject hpBar; // 체력 바 GameObject
    [SerializeField] protected GameObject monsterOwnedGold; // 몬스터를 물리치면 얻을 수 있는 골드 GameObject
    [SerializeField] protected GameObject monsterOwnedItem; // 몬스터를 물리치면 얻을 수 있는 아이템 GameObject
    [SerializeField] protected GameObject mainBody; // 몬스터의 실제 위치를 나타내는 객체의 자식으로 속해있는 몬스터 본체

    int curHp; // 현재 체력
    bool attackDone; // 공격이 끝난 상태인지
    bool getHurt = false; // 다친 상태인지
    bool dyingDone; // 죽는 동작이 끝났는지
    float wait = 0; // 동작 대기 시간을 나타냄
    Vector3 curPos; // 현재 위치
    Vector3 targetPos; // 최단 경로를 정하는 순간의 타겟인 플레이어의 위치
    protected float realSpeed; // speed를 1000으로 나눈 값
    protected StateMachine<Monster> stateMachine = new StateMachine<Monster>();
    protected State<Monster>[] states = new State<Monster>[5];
    protected PathFinding pathFinding = new PathFinding();
    protected Rigidbody2D rb; // 유니티에서 객체의 물리이동에 필요한 RigidBody 컴포넌트
    protected Animator ani; // 유니티 애니메이션 컴포넌트
    protected Vector3 minPos; // 몬스터가 이동할 수 있는 범위 최소값
    protected Vector3 maxPos; // 몬스터가 이동할 수 있는 범위 최대값
    protected int lastXDirection; // 가장 최근에 움직인 x축 방향 (-1 : 왼쪽, 1 : 오른쪽)
    protected int direction; // 움직일 방향 index
    protected int hurtDamage; // 스킬에 맞았을 때 받은 데미지 값
    protected Vector3 firstPos; // 처음 위치
    protected IEnumerator movingChoice; // RandomChoice 코루틴 변수
    protected WaitForSeconds wfs; // 코루틴에서 제어권을 돌려주고 기다리는 시간
    protected List<Vector3> shortestPath; // 추격 시 이동할 최단 경로

    /* Property */
    public int MonsterId
    {
        get
        {
            return monsterId;
        }
    }
    /* Property */
    public int RealMapNum
    {
        get
        {
            return realMapNum;
        }
    }
    /* Property */
    public int LocatedMapNum
    {
        get
        {
            return locatedMapNum;
        }
    }
    /* Property */
    public int ExpValue
    {
        get
        {
            return expValue;
        }
    }
    /* Property */
    public int AttackArea
    {
        get
        {
            return attackArea;
        }
    }
    /* Property */
    public int AttackPower
    {
        get
        {
            return attackPower;
        }
    }
    /* Property */
    public int CurHp
    {
        get
        {
            return curHp;
        }
        set
        {
            curHp = value;
        }
    }
    /* Property */
    public bool AttackDone
    {
        get
        {
            return attackDone;
        }
        set
        {
            attackDone = value;
        }
    }
    /* Property */
    public bool GetHurt
    {
        get
        {
            return getHurt;
        }
        set
        {
            getHurt = value;
        }
    }
    /* Property */
    public bool DyingDone
    {
        get
        {
            return dyingDone;
        }
        set
        {
            dyingDone = value;
        }
    }
    /* Property */
    public float Wait
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
    /* Property */
    public Vector3 CurPos
    {
        get
        {
            return curPos;
        }
        set
        {
            curPos = value;
        }
    }
    /* Property */
    public Vector3 TargetPos
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = value;
        }
    }
    /* Method : SettingStates
     * Description : FSM에서 다룰 상태들을 states배열에 차례대로 할당하는 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public void SettingStates()
    {
        states[(int)MonsterStates.MonsterState.normal] = new MonsterStates.Normal();
        states[(int)MonsterStates.MonsterState.chasePlayer] = new MonsterStates.ChasePlayer();
        states[(int)MonsterStates.MonsterState.attackPlayer] = new MonsterStates.AttackPlayer();
        states[(int)MonsterStates.MonsterState.hurt] = new MonsterStates.Hurt();
        states[(int)MonsterStates.MonsterState.die] = new MonsterStates.Die();
    }
    /* Method : ChangeState
     * Description : 상태 전이를 위해 다음 상태로 바꾸는 동작을 수행하는 메서드입니다.
     * Parameter : MonsterStates.MonsterState nextState - MonsterStates namespace에 MonsterState라는 이름의 enum 클래스가 있는데 몬스터의 상태를 나타냄, 즉 nextState는 몬스터의 다음 상태를 의미
     * Return Value : void
     */
    public void ChangeState(MonsterStates.MonsterState nextState)
    {
        stateMachine.ChangeState(states[(int)nextState]);
    }
    /* Method : ResetState
     * Description : 몬스터가 리스폰 되는 순간 체력 값이나 현재 상태 등을 기본값으로 초기화하는 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void ResetState()
    {

    }
    /* Method : MovingChoice
     * Description : Normal 상태에서 다음에 이동할 곳을 선택하는 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void MovingChoice()
    {

    }
    /* Method : Flip
     * Description : x축 방향에 따라 왼쪽 또는 오른쪽으로 뒤집는 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Parameter : int x - x축 방향을 의미, -1 : 왼쪽, 1 : 오른쪽
     * Return Value : void
     */
    public virtual void Flip(int x)
    {
    }
    /* Method : Moving
     * Description : MovingChoice 메서드를 먼저 호출해 다음으로 이동할 목적지에 대한 최단 경로를 세팅하고 Chasing 메서드를 호출해서 해당 경로를 따라 이동하도록 하는 메서드입니다. 
     * 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Moving()
    {

    }
    /* Method : Stop
     * Description : 정지 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Stop()
    {

    }
    /* Method : SetShortestPath
     * Description : 몬스터의 현재 위치에서 플레이어의 현재 위치까지 최단 경로를 세팅하기 위한 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void SetShortestPath()
    {

    }
    /* Method : Chasing
     * Description : 계산된 최단 경로를 따라 목적지까지 이동하는 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Chasing()
    {

    }
    /* Method : Attack
     * Description : 몬스터의 공격 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Attack()
    {

    }
    /* Method : Hurt
     * Description : 몬스터가 다칠 때 관련된 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Hurt()
    {

    }
    /* Method : Die
     * Description : 몬스터가 죽을 때 관련된 동작을 수행하는 메서드입니다. 하위 클래스에서 재정의 가능하도록 만들었습니다.
     * Return Value : void
     */
    public virtual void Die()
    {

    }
    /* Method : GiveReward
     * Description : 몬스터를 물리치면 얻을 수 있는 보상들을 필드에 활성화하는 메서드입니다.
     * Return Value : void
     */
    public void GiveReward()
    {
        monsterOwnedGold.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        monsterOwnedItem.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        monsterOwnedGold.SetActive(true);
        monsterOwnedItem.SetActive(true);
    }
}
