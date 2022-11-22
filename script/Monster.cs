using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Class : Monster
 * Description : ������ ���͸� ����ϴ� Ŭ�����Դϴ�. 
 */
public class Monster : MonoBehaviour
{
    [SerializeField] private int monsterId; // ���͸� �ĺ��ϴ� ���̵�
    [SerializeField] private int realMapNum; // �� ���� ��ȣ
    [SerializeField] private int expValue; // ����ġ�� ���� �� �ִ� ����ġ
    [SerializeField] private int attackArea; // ���� ����
    [SerializeField] protected int locatedMapNum; // ���Ͱ� �ִ� �ʸ� ������ ������ ��ȣ(���� �� ��ȣ�ʹ� ������ ��, ���Ͱ� �ִ� �ʸ��� �������� ��)
    [SerializeField] protected int attackPower; // ���ݷ�
    [SerializeField] protected int maxHp; // ��ü ü��
    [SerializeField] protected int speed; // �ӵ�
    [SerializeField] protected int defaultDirection; // ������ �⺻ �ü� ���� (-1 : ����, 1 : ������)
    [SerializeField] protected Image hpGraph; // ü�� �׷���
    [SerializeField] protected GameObject hpBar; // ü�� �� GameObject
    [SerializeField] protected GameObject monsterOwnedGold; // ���͸� ����ġ�� ���� �� �ִ� ��� GameObject
    [SerializeField] protected GameObject monsterOwnedItem; // ���͸� ����ġ�� ���� �� �ִ� ������ GameObject
    [SerializeField] protected GameObject mainBody; // ������ ���� ��ġ�� ��Ÿ���� ��ü�� �ڽ����� �����ִ� ���� ��ü

    int curHp; // ���� ü��
    bool attackDone; // ������ ���� ��������
    bool getHurt = false; // ��ģ ��������
    bool dyingDone; // �״� ������ ��������
    float wait = 0; // ���� ��� �ð��� ��Ÿ��
    Vector3 curPos; // ���� ��ġ
    Vector3 targetPos; // �ִ� ��θ� ���ϴ� ������ Ÿ���� �÷��̾��� ��ġ
    protected float realSpeed; // speed�� 1000���� ���� ��
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
    protected Vector3 firstPos; // ó�� ��ġ
    protected IEnumerator movingChoice; // RandomChoice �ڷ�ƾ ����
    protected WaitForSeconds wfs; // �ڷ�ƾ���� ������� �����ְ� ��ٸ��� �ð�
    protected List<Vector3> shortestPath; // �߰� �� �̵��� �ִ� ���

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
     * Description : FSM���� �ٷ� ���µ��� states�迭�� ���ʴ�� �Ҵ��ϴ� ������ �����ϴ� �޼����Դϴ�.
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
     * Description : ���� ���̸� ���� ���� ���·� �ٲٴ� ������ �����ϴ� �޼����Դϴ�.
     * Parameter : MonsterStates.MonsterState nextState - MonsterStates namespace�� MonsterState��� �̸��� enum Ŭ������ �ִµ� ������ ���¸� ��Ÿ��, �� nextState�� ������ ���� ���¸� �ǹ�
     * Return Value : void
     */
    public void ChangeState(MonsterStates.MonsterState nextState)
    {
        stateMachine.ChangeState(states[(int)nextState]);
    }
    /* Method : ResetState
     * Description : ���Ͱ� ������ �Ǵ� ���� ü�� ���̳� ���� ���� ���� �⺻������ �ʱ�ȭ�ϴ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void ResetState()
    {

    }
    /* Method : MovingChoice
     * Description : Normal ���¿��� ������ �̵��� ���� �����ϴ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void MovingChoice()
    {

    }
    /* Method : Flip
     * Description : x�� ���⿡ ���� ���� �Ǵ� ���������� ������ ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Parameter : int x - x�� ������ �ǹ�, -1 : ����, 1 : ������
     * Return Value : void
     */
    public virtual void Flip(int x)
    {
    }
    /* Method : Moving
     * Description : MovingChoice �޼��带 ���� ȣ���� �������� �̵��� �������� ���� �ִ� ��θ� �����ϰ� Chasing �޼��带 ȣ���ؼ� �ش� ��θ� ���� �̵��ϵ��� �ϴ� �޼����Դϴ�. 
     * ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Moving()
    {

    }
    /* Method : Stop
     * Description : ���� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Stop()
    {

    }
    /* Method : SetShortestPath
     * Description : ������ ���� ��ġ���� �÷��̾��� ���� ��ġ���� �ִ� ��θ� �����ϱ� ���� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void SetShortestPath()
    {

    }
    /* Method : Chasing
     * Description : ���� �ִ� ��θ� ���� ���������� �̵��ϴ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Chasing()
    {

    }
    /* Method : Attack
     * Description : ������ ���� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Attack()
    {

    }
    /* Method : Hurt
     * Description : ���Ͱ� ��ĥ �� ���õ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Hurt()
    {

    }
    /* Method : Die
     * Description : ���Ͱ� ���� �� ���õ� ������ �����ϴ� �޼����Դϴ�. ���� Ŭ�������� ������ �����ϵ��� ��������ϴ�.
     * Return Value : void
     */
    public virtual void Die()
    {

    }
    /* Method : GiveReward
     * Description : ���͸� ����ġ�� ���� �� �ִ� ������� �ʵ忡 Ȱ��ȭ�ϴ� �޼����Դϴ�.
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
