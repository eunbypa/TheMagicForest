using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Skill
 * Description : ���� �� �÷��̾ ����ϴ� ���� ��ų�� ����ϴ� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ���� �浹 ó�� �Լ��� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class Skill : MonoBehaviour
{
    [SerializeField] private int attackPower; // ��ų ���ݷ�
    [SerializeField] private int speed; // ��ų ���ǵ�
    [SerializeField] private float coolTime; // ��ų ��Ÿ��
    int goX = 0; // x�� �̵� ����
    int goY = 0; // y�� �̵� ����
    //float coolTime = 2f; // ��ų ��Ÿ��
    bool attack = false; // ��ų ���� ���� ���� ����
    bool finish = false; // ��ų ��Ȱ��ȭ ����
    bool attackSuccess = false; // ��ų ���� ���� ����
    Animator ani; // ����Ƽ �ִϸ��̼� ������Ʈ
    Rigidbody2D rb; // ����Ƽ���� ��ü�� �����̵��� �ʿ��� RigidBody ������Ʈ

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
        this.rb = GetComponent<Rigidbody2D>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� RigidBody2D ������Ʈ�� �����ɴϴ�.
        this.ani = GetComponent<Animator>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� Animator ������Ʈ�� �����ɴϴ�.
        this.attackPower += GameManager.instance.CurLevel * 5;
    }

    void FixedUpdate()
    {
        Moving();
    }

    /* Method : Moving
     * Description : ��ų�� �̵� ������ �����ϴ� �޼����Դϴ�. attack ������ ���� �Ǹ� ���� �ִϸ��̼��� ����ǰ� ������ �����س��� �������� �̵��մϴ�.
     * ��� �������� �̵��ϵ� �Ȱ��� �ӵ��� ������ �ϱ� ���� ���� Vector�� ����ȭ �� �� ���ϴ� �ӵ� ���� ���ϴ� ������� �����Ͽ����ϴ�.
     * finish ������ ���� �Ǹ� ��ų�� ��Ȱ��ȭ �˴ϴ�. ��ų�� ������� ������ �ִϸ��̼��� ����ǰ� ���� ���·� ���մϴ�. 
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
        if (Other.gameObject.tag == "MOP") // �浹�� ���Ͱ� ���� ���
        {
            attackSuccess = true;
            attack = false;
            finish = true;
        }
    }

    /* Method : Setting
     * Description : ��ų�� Ȱ��ȭ �� �� ��ų�� ���� ��ġ�� ��ų�� �̵��ؾ� �� ������ �����ϴ� �޼����Դϴ�.
     * ���� �Ű������� ���� x�� ����� y�� ������ ��� 0�̶�� ����Ʈ ������ ���� �������� ���õǵ��� �����߽��ϴ�.
     * Parameter : float x - x�� ��ġ ��, float y - y�� ��ġ ��, int directionX - x�� ����, int directionY - y�� ����
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
