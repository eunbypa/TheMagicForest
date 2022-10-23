using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private int monsterId;
    [SerializeField] private int realMapNum; // 맵 기존 번호
    [SerializeField] private int expValue; // 물리치면 얻을 수 있는 경험치
    [SerializeField] protected int locatedMapNum; // 몬스터가 있는 맵만 가지는 별도의 번호(기존 맵 번호와는 별도의 값, 몬스터가 있는 맵만을 기준으로 함)
    [SerializeField] protected int attackPower;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int speed;
    [SerializeField] protected Image hpGraph;
    [SerializeField] protected GameObject hpBar;
    [SerializeField] protected GameObject monsterOwnedGold;
    [SerializeField] protected GameObject monsterOwnedItem;
    [SerializeField] protected GameObject mainBody; // 몬스터의 실제 위치를 나타내는 객체의 자식으로 속해있는 몬스터 본체

    int curHp;
    bool curChasingDone;
    bool attackDone; // 공격이 끝난 상태인지
    bool getHurt = false; // 다친 상태인지
    bool dyingDone; // 죽는 동작이 끝났는지
    float wait = 0;
    Vector3 curPos; // 현재 위치
    Vector3 targetPos; // 최단 경로를 정하는 순간의 타겟인 플레이어의 위치
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
    protected int[] moveX = new int[] { -1, 0, 1, 0, -1, -1, 1, 1, 0};
    protected int[] moveY = new int[] { 0, -1, 0, 1, -1, 1, -1, 1, 0};
    protected bool left = false; // 시선이 왼쪽을 향해 있는지 여부, 기본값은 오른쪽
    protected System.Random rand = new System.Random();
    protected IEnumerator movingChoice; // HurtEvent 코루틴 변수
    protected WaitForSeconds wfs; // 코루틴에서 제어권을 돌려주고 기다리는 시간
    protected List<Vector3> shortestPath; // 추격 시 이동할 최단 경로

    public int MonsterId
    {
        get
        {
            return monsterId;
        }
    }
    public int RealMapNum
    {
        get
        {
            return realMapNum;
        }
    }
    public int ExpValue
    {
        get
        {
            return expValue;
        }
    }
    public int AttackPower
    {
        get
        {
            return attackPower;
        }
    }
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
    public bool CurChasingDone
    {
        get
        {
            return curChasingDone;
        }
        set
        {
            curChasingDone = value;
        }
    }
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
    //FSM 시작 세팅
    public void SettingStates()
    {
        states[(int)MonsterStates.MonsterState.normal] = new MonsterStates.Normal();
        states[(int)MonsterStates.MonsterState.chasePlayer] = new MonsterStates.ChasePlayer();
        states[(int)MonsterStates.MonsterState.attackPlayer] = new MonsterStates.AttackPlayer(); 
        states[(int)MonsterStates.MonsterState.hurt] = new MonsterStates.Hurt();
        states[(int)MonsterStates.MonsterState.die] = new MonsterStates.Die();
    }
    public void ChangeState(MonsterStates.MonsterState nextState)
    {
        stateMachine.ChangeState(states[(int)nextState]);
    }
    //리스폰 되는 순간 체력이나 상태 등을 리셋
    public virtual void ResetState()
    {

    }
    //움직일 방향 랜덤하게 선택
    public virtual void MovingChoice()
    {

    }
    //좌우 뒤집기
    public virtual void Flip(int x)
    {
    }
    //랜덤하게 필드 범위 내에서 움직임
    public virtual void Moving()
    {

    }
    //정지
    public virtual void Stop()
    {

    }
    //플레이어의 위치까지의 최단 경로 세팅
    public virtual void SetShortestPath()
    {

    }
    //최단 경로를 따라 플레이어를 추격
    public virtual void Chasing()
    {

    }
    //공격
    public virtual void Attack()
    {

    }
    //다침
    public virtual void Hurt()
    {

    }
    //죽음
    public virtual void Die()
    {

    }
    //아이템, 골드 활성화
    public void GiveReward()
    {
        monsterOwnedGold.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        monsterOwnedItem.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        monsterOwnedGold.SetActive(true);
        monsterOwnedItem.SetActive(true);
    }
}
