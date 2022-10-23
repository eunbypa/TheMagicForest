using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private int monsterId;
    [SerializeField] private int realMapNum; // �� ���� ��ȣ
    [SerializeField] private int expValue; // ����ġ�� ���� �� �ִ� ����ġ
    [SerializeField] protected int locatedMapNum; // ���Ͱ� �ִ� �ʸ� ������ ������ ��ȣ(���� �� ��ȣ�ʹ� ������ ��, ���Ͱ� �ִ� �ʸ��� �������� ��)
    [SerializeField] protected int attackPower;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int speed;
    [SerializeField] protected Image hpGraph;
    [SerializeField] protected GameObject hpBar;
    [SerializeField] protected GameObject monsterOwnedGold;
    [SerializeField] protected GameObject monsterOwnedItem;
    [SerializeField] protected GameObject mainBody; // ������ ���� ��ġ�� ��Ÿ���� ��ü�� �ڽ����� �����ִ� ���� ��ü

    int curHp;
    bool curChasingDone;
    bool attackDone; // ������ ���� ��������
    bool getHurt = false; // ��ģ ��������
    bool dyingDone; // �״� ������ ��������
    float wait = 0;
    Vector3 curPos; // ���� ��ġ
    Vector3 targetPos; // �ִ� ��θ� ���ϴ� ������ Ÿ���� �÷��̾��� ��ġ
    protected StateMachine<Monster> stateMachine = new StateMachine<Monster>();
    protected State<Monster>[] states = new State<Monster>[5];
    protected PathFinding pathFinding = new PathFinding();
    protected Rigidbody2D rb; // ����Ƽ���� ��ü�� �����̵��� �ʿ��� RigidBody ������Ʈ
    protected Animator ani; // ����Ƽ �ִϸ��̼� ������Ʈ
    protected Vector3 minPos; // ���Ͱ� �̵��� �� �ִ� ���� �ּҰ�
    protected Vector3 maxPos; // ���Ͱ� �̵��� �� �ִ� ���� �ִ밪
    protected int lastXDirection; // ���� �ֱٿ� ������ x�� ���� (-1 : ����, 1 : ������)
    protected int direction; // ������ ���� index
    protected int hurtDamage; // ��ų�� �¾��� �� ���� ������ ��
    protected int[] moveX = new int[] { -1, 0, 1, 0, -1, -1, 1, 1, 0};
    protected int[] moveY = new int[] { 0, -1, 0, 1, -1, 1, -1, 1, 0};
    protected bool left = false; // �ü��� ������ ���� �ִ��� ����, �⺻���� ������
    protected System.Random rand = new System.Random();
    protected IEnumerator movingChoice; // HurtEvent �ڷ�ƾ ����
    protected WaitForSeconds wfs; // �ڷ�ƾ���� ������� �����ְ� ��ٸ��� �ð�
    protected List<Vector3> shortestPath; // �߰� �� �̵��� �ִ� ���

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
    //FSM ���� ����
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
    //������ �Ǵ� ���� ü���̳� ���� ���� ����
    public virtual void ResetState()
    {

    }
    //������ ���� �����ϰ� ����
    public virtual void MovingChoice()
    {

    }
    //�¿� ������
    public virtual void Flip(int x)
    {
    }
    //�����ϰ� �ʵ� ���� ������ ������
    public virtual void Moving()
    {

    }
    //����
    public virtual void Stop()
    {

    }
    //�÷��̾��� ��ġ������ �ִ� ��� ����
    public virtual void SetShortestPath()
    {

    }
    //�ִ� ��θ� ���� �÷��̾ �߰�
    public virtual void Chasing()
    {

    }
    //����
    public virtual void Attack()
    {

    }
    //��ħ
    public virtual void Hurt()
    {

    }
    //����
    public virtual void Die()
    {

    }
    //������, ��� Ȱ��ȭ
    public void GiveReward()
    {
        monsterOwnedGold.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        monsterOwnedItem.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        monsterOwnedGold.SetActive(true);
        monsterOwnedItem.SetActive(true);
    }
}
