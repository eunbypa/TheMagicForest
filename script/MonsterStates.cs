using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* namespace : MonsterStates
 * Description : ���Ͱ� ���� �� �ִ� ���� Ŭ������ ���� namespace
 */
namespace MonsterStates
{
    // ���Ͱ� ���� �� �ִ� ���¸� ���������� ǥ���ϱ� ���� enum�� ����߽��ϴ�.
    // normal : ����, chasePlayer : �÷��̾� �߰�, attackPlayer : �÷��̾� ����, hurt : ��ħ, die : ����
    public enum MonsterState { normal, chasePlayer, attackPlayer, hurt, die };
    /* Class : Normal
     * Description : ���� ���� Ŭ�����Դϴ�. State<Monster> Ŭ������ ��ӹ޽��ϴ�.
    */
    public class Normal : State<Monster>
    {
        /* Method : Enter
        * Description : Normal ���� ���� �� 1ȸ ȣ��˴ϴ�. �����ϰ� �����̱� ���ؼ� entity�� MovingChoice �޼��带 ȣ���մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.MovingChoice();
        }
        /* Method : Execute
        * Description : Normal ���¸� ���������� �ʴ� �̻� ��� ȣ��˴ϴ�. Normal ���¿����� Hurt ���� �Ǵ� ChasePlayer ���·� ���̵� �� �ֽ��ϴ�.  
        * ���Ͱ� �÷��̾ �ν��� �� �ִ� ������ 10���� �����ؼ� �� ���� ���� ������ ChasePlayer�� ���̵˴ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            entity.Moving();
            if (entity.GetHurt)
            {
                entity.ChangeState(MonsterState.hurt);
                return;
            }
            if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) < 10)
            {
                entity.ChangeState(MonsterState.chasePlayer);
            }
        }
        /* Method : Exit
        * Description : Normal ���¿��� �������� �� 1ȸ ȣ��˴ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.MovingChoice();
            entity.Stop();
        }
    }
    /* Class : ChasePlayer
     * Description : �÷��̾� �߰� ���� Ŭ�����Դϴ�. State<Monster> Ŭ������ ��ӹ޽��ϴ�.
    */
    public class ChasePlayer : State<Monster>
    {
        /* Method : Enter
        * Description : ChasePlayer ���� ���� �� 1ȸ ȣ��˴ϴ�. ������ ���� ��ġ���� �÷��̾��� ���� ��ġ���� �ִ� ��θ� ����ϱ� ���� entity�� SetShortestPath �޼��带 ȣ���մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.SetShortestPath();
        }
        /* Method : Execute
        * Description : ChasePlayer ���¸� ���������� �ʴ� �̻� ��� ȣ��˴ϴ�. Hurt ���¿� Normal ���� AttackPlayer ���·� ���̵� �� �ֽ��ϴ�.
        * Normal ���·� ���̵Ǵ� ������ �÷��̾ �ٸ� ������ �̵��� ���� AttackPlayer ���·� ���̵Ǵ� ������ �÷��̾ ������ ���� ���� ���� ���� �� �Դϴ�. ���� ������ 5�� ���߽��ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            if (entity.GetHurt)
            {
                entity.ChangeState(MonsterState.hurt);
                return;
            }
            if (entity.RealMapNum != GameManager.instance.CurMapNum)
            {
                entity.ChangeState(MonsterState.normal);
                return;
            }
            entity.Chasing();
            if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 5)
            {
                entity.ChangeState(MonsterState.attackPlayer);
                return;
            }
            if (Math.Sqrt(Math.Pow(entity.TargetPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.TargetPos.y - GameManager.instance.PlayerPos.y, 2)) > 2)
            {
                entity.SetShortestPath();
            }
        }
        /* Method : Exit
        * Description : ChasePlayer ���¸� �������� �� 1ȸ ȣ��˴ϴ�. ���� ���¸� �����ϱ� ���� ��� �����մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.Stop();
        }
    }
    /* Class : AttackPlayer
     * Description : �÷��̾� ���� ���� Ŭ�����Դϴ�. State<Monster> Ŭ������ ��ӹ޽��ϴ�.
    */
    public class AttackPlayer : State<Monster>
    {
        /* Method : Enter
        * Description : AttackPlayer ���� ���� �� 1ȸ ȣ��˴ϴ�. entity�� Attack �޼��带 ȣ���մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Attack();
        }
        /* Method : Execute
        * Description : AttackPlayer ���¸� ���������� �ʴ� �̻� ��� ȣ��˴ϴ�. Hurt ���¿� ChasePlayer ���·� ���̵� �� �ֽ��ϴ�. Wait�� 1�� ��ٸ��� ������ ������ ���� ������ ������ ���� 
        * 1�ʰ� �ҿ�Ǳ� �����Դϴ�. ���Ͱ� �÷��̾ �ν��� �� �ִ� ���� ���� ������ ChasePlayer�� ���̵˴ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            entity.Wait += Time.deltaTime;
            if (entity.Wait >= 1f)
            {
                entity.AttackDone = true;
                entity.Wait = 0;
            }
            if (entity.GetHurt) entity.ChangeState(MonsterState.hurt);
            if (entity.AttackDone) 
            {
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) > 5) // ���� �������� �־������Ƿ� �ٽ� �߰�
                {
                    entity.ChangeState(MonsterState.chasePlayer);
                    return;
                }
                entity.Attack();
            }
        }
        /* Method : Exit
        * Description : AttackPlayer ���¸� �������� �� 1ȸ ȣ��˴ϴ�. Wait���� �ʱ�ȭ�մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.Wait = 0;
        }
    }
    /* Class : Hurt
     * Description : ��ģ ���� Ŭ�����Դϴ�. State<Monster> Ŭ������ ��ӹ޽��ϴ�.
    */
    public class Hurt : State<Monster>
    {
        /* Method : Enter
        * Description : Hurt ���� ���� �� 1ȸ ȣ��˴ϴ�. entity�� Hurt �޼��带 ȣ���մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Hurt();
        }
        /* Method : Execute
        * Description : Hurt ���¸� ���������� �ʴ� �̻� ��� ȣ��˴ϴ�. Normal ���¿� AttackPlayer ����, ChasePlayer ����, Die ���·� ���̵� �� �ֽ��ϴ�. Wait�� ��ģ ������ �Ϸ��� ������ ��ٸ��ϴ�.
        * Normal ���·� ���̵Ǵ� ������ �÷��̾ �ٸ� ������ �̵��� ���� AttackPlayer ���·� ���̵Ǵ� ������ �÷��̾ ������ ���� ���� ���� ���� �� �Դϴ�.
        * ���Ͱ� �÷��̾ �ν��� �� �ִ� ���� ���� ������ ChasePlayer�� ���̵˴ϴ�. ���� hp�� 0�� �Ǹ� Die ���·� ���̵˴ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            if (!entity.GetHurt) 
            {
                if (entity.RealMapNum != GameManager.instance.CurMapNum)
                {
                    entity.ChangeState(MonsterState.normal);
                    return;
                }
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 5)
                {
                    entity.ChangeState(MonsterState.attackPlayer);
                    return;
                }
                entity.ChangeState(MonsterState.chasePlayer);
            }
            else
            {
                if (entity.CurHp == 0)
                {
                    entity.Wait = 0;
                    entity.ChangeState(MonsterState.die);
                }
                entity.Wait += Time.deltaTime;
                if (entity.Wait >= 0.3f)
                {
                    entity.GetHurt = false;
                    entity.Wait = 0;
                }
            }
        }
        /* Method : Exit
        * Description : Hurt ���¸� �������� �� 1ȸ ȣ��˴ϴ�. GetHurt�� Wait���� �ʱ�ȭ�մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.GetHurt = false; 
            entity.Wait = 0;
        }
    }
    /* Class : Die
     * Description : ���� ���� Ŭ�����Դϴ�. State<Monster> Ŭ������ ��ӹ޽��ϴ�.
    */
    public class Die : State<Monster>
    {
        /* Method : Enter
        * Description : Die ���� ���� �� 1ȸ ȣ��˴ϴ�. entity�� Die �޼���� GiveReward �޼��带 ȣ���մϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Die();
            entity.GiveReward();
        }
        /* Method : Execute
        * Description : Die ���¸� ���������� �ʴ� �̻� ��� ȣ��˴ϴ�. Die ���¿����� �ٸ� ���·� ���̵��� �ʽ��ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            entity.Die();
        }
        /* Method : Exit
        * Description : �������� �� ������ ������ ��� ����׽��ϴ�.
        * Parameter : Monster entity - ����
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            
        }
    }
}
