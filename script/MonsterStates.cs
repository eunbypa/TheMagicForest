using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterStates
{
    public enum MonsterState { normal, chasePlayer, attackPlayer, hurt, die };
    //�⺻
    public class Normal : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.MovingChoice();
        }
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
                entity.ChangeState(MonsterState.chasePlayer); // �÷��̾ ���Ͱ� �ν��� �� �ִ� ���� ���� ������ �߰� ����
            }
        }
 
        public override void Exit(Monster entity)
        {
            entity.MovingChoice();
            entity.Stop();
        }
    }
    //�÷��̾� ����
    public class ChasePlayer : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.SetShortestPath();
        }
        public override void Execute(Monster entity)
        {
            if (entity.GetHurt) // ��ģ ����
            {
                entity.ChangeState(MonsterState.hurt);
                return;
            }
            if (entity.RealMapNum != GameManager.instance.CurMapNum) // �÷��̾ �ٸ� ������ �̵��� ��� normal ���·� �ٲ�
            {
                entity.ChangeState(MonsterState.normal);
                return;
            }
            entity.Chasing();
            if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 5)
            {
                entity.ChangeState(MonsterState.attackPlayer); // �÷��̾ ���Ͱ� ������ �� �ִ� ���� ���� ������ ���� ����
                return;
            }
            if (Math.Sqrt(Math.Pow(entity.TargetPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.TargetPos.y - GameManager.instance.PlayerPos.y, 2)) > 2)
            {
                entity.SetShortestPath();
            }
        }

        public override void Exit(Monster entity)
        {
            entity.Stop();
        }
    }
    //�÷��̾� ����
    public class AttackPlayer : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.Attack();
        }
        public override void Execute(Monster entity)
        {
            entity.Wait += Time.deltaTime;
            if (entity.Wait >= 1f)
            {
                entity.AttackDone = true;
                entity.Wait = 0;
            }
            if (entity.GetHurt) entity.ChangeState(MonsterState.hurt);
            if (entity.AttackDone) // ���� ���� �ൿ�� ���� ���
            {
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) > 5) // ���� �������� �־������Ƿ� �ٽ� �߰�
                {
                    entity.ChangeState(MonsterState.chasePlayer);
                    return;
                }
                entity.Attack();
            }
        }

        public override void Exit(Monster entity)
        {
            entity.Wait = 0;
        }
    }
    //��ħ
    public class Hurt : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.Hurt();
        }
        public override void Execute(Monster entity)
        {
            if (!entity.GetHurt) // ��ģ ������ �Ϸ��ϸ� �ٸ� state�� �Ѿ
            {
                if (entity.RealMapNum != GameManager.instance.CurMapNum) // �÷��̾ �ٸ� ������ �̵��� ��� normal ���·� �ٲ�
                {
                    entity.ChangeState(MonsterState.normal); // �÷��̾ ���Ͱ� ������ �� �ִ� ���� ���� ������ ���� ����
                    return;
                }
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 4)
                {
                    entity.ChangeState(MonsterState.attackPlayer); // �÷��̾ ���Ͱ� ������ �� �ִ� ���� ���� ������ ���� ����
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

        public override void Exit(Monster entity)
        {
            entity.GetHurt = false; // die�� state ��ȯ �� �ʿ�
            entity.Wait = 0;
        }
    }
    //����
    public class Die : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.Die();
            entity.GiveReward();
        }
        public override void Execute(Monster entity)
        {
            entity.Die();
        }

        public override void Exit(Monster entity)
        {
            
        }
    }
}
