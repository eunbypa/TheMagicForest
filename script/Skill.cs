using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Skill
 * Description : 게임 내 플레이어가 사용하는 마법 스킬을 담당하는 클래스입니다. 유니티의 생명 주기 함수들과 충돌 처리 함수를 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class Skill : MonoBehaviour
{
    [SerializeField] private int attackPower; // 스킬 공격력
    [SerializeField] private int speed; // 스킬 스피드
    [SerializeField] private float coolTime; // 스킬 쿨타임
    int goX = 0; // x축 이동 방향
    int goY = 0; // y축 이동 방향
    //float coolTime = 2f; // 스킬 쿨타임
    bool attack = false; // 스킬 공격 가능 상태 여부
    bool finish = false; // 스킬 비활성화 여부
    bool attackSuccess = false; // 스킬 공격 성공 여부
    Animator ani; // 유니티 애니메이션 컴포넌트
    Rigidbody2D rb; // 유니티에서 객체의 물리이동에 필요한 RigidBody 컴포넌트

    /* Property */
    public int AttackPower
    {
        get
        {
            return attackPower;
        }
        set
        {
            attackPower = value;
        }
    }
    /* Property */
    public float CoolTime
    {
        get
        {
            return coolTime;
        }
        set
        {
            coolTime = value;
        }
    }
    /* Property */
    public bool Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }
    /* Property */
    public bool AttackSuccess
    {
        get
        {
            return attackSuccess;
        }
        set
        {
            attackSuccess = value;
        }
    }
    /* Property */
    public bool Finish
    {
        get
        {
            return finish;
        }
        set
        {
            finish = value;
        }
    }

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>(); // 현재 클래스가 할당된 GameObject 객체에서 RigidBody2D 컴포넌트를 가져옵니다.
        this.ani = GetComponent<Animator>(); // 현재 클래스가 할당된 GameObject 객체에서 Animator 컴포넌트를 가져옵니다.
        this.attackPower += GameManager.instance.CurLevel * 5;
    }

    void FixedUpdate()
    {
        Moving();
    }

    /* Method : Moving
     * Description : 스킬의 이동 동작을 수행하는 메서드입니다. attack 변수가 참이 되면 공격 애니메이션이 실행되고 사전에 설정해놓은 방향으로 이동합니다.
     * 어느 방향으로 이동하든 똑같은 속도를 가지게 하기 위해 방향 Vector를 정규화 한 후 원하는 속도 값을 곱하는 방식으로 구현하였습니다.
     * finish 변수가 참이 되면 스킬이 비활성화 됩니다. 스킬이 사라지는 연출의 애니메이션이 실행되고 정지 상태로 변합니다. 
     * Return Value : void
     */
    void Moving()
    {
        if (attack)
        {
            ani.SetTrigger("set");
            rb.velocity = new Vector2(goX, goY).normalized * speed;
        }
        if (finish)
        {
            ani.SetTrigger("reset");
            rb.velocity = new Vector2(0, 0) * 0;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "MOP") // 충돌한 몬스터가 있을 경우
        {
            attackSuccess = true;
            attack = false;
            finish = true;
        }
    }

    /* Method : Setting
     * Description : 스킬이 활성화 될 때 스킬의 시작 위치와 스킬이 이동해야 할 방향을 세팅하는 메서드입니다.
     * 만약 매개변수로 받은 x축 방향과 y축 방향이 모두 0이라면 디폴트 방향인 왼쪽 방향으로 세팅되도록 구현했습니다.
     * Parameter : float x - x축 위치 값, float y - y축 위치 값, int directionX - x축 방향, int directionY - y축 방향
     * Return Value : void
     */
    public void Setting(float x, float y, int directionX, int directionY)
    {
        this.transform.position = new Vector2(x, y + 2);
        if ((directionX == 0) && (directionY == 0))
        {
            goX = -1;
            goY = 0;
        }
        else
        {
            goX = directionX;
            goY = directionY;
        }
    }

}
