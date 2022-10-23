using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterStates
{
    public enum MonsterState { normal, chasePlayer, attackPlayer, hurt, die };
    //기본
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
                entity.ChangeState(MonsterState.chasePlayer); // 플레이어가 몬스터가 인식할 수 있는 범위 내에 들어오면 추격 시작
            }
        }
 
        public override void Exit(Monster entity)
        {
            entity.MovingChoice();
            entity.Stop();
        }
    }
    //플레이어 추적
    public class ChasePlayer : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.SetShortestPath();
        }
        public override void Execute(Monster entity)
        {
            if (entity.GetHurt) // 다친 상태
            {
                entity.ChangeState(MonsterState.hurt);
                return;
            }
            if (entity.RealMapNum != GameManager.instance.CurMapNum) // 플레이어가 다른 맵으로 이동한 경우 normal 상태로 바뀜
            {
                entity.ChangeState(MonsterState.normal);
                return;
            }
            entity.Chasing();
            if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 5)
            {
                entity.ChangeState(MonsterState.attackPlayer); // 플레이어가 몬스터가 공격할 수 있는 범위 내에 들어오면 공격 시작
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
    //플레이어 공격
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
            if (entity.AttackDone) // 현재 공격 행동이 끝난 경우
            {
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) > 5) // 공격 범위에서 멀어졌으므로 다시 추격
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
    //다침
    public class Hurt : State<Monster>
    {
        public override void Enter(Monster entity)
        {
            entity.Hurt();
        }
        public override void Execute(Monster entity)
        {
            if (!entity.GetHurt) // 다친 동작을 완료하면 다른 state로 넘어감
            {
                if (entity.RealMapNum != GameManager.instance.CurMapNum) // 플레이어가 다른 맵으로 이동한 경우 normal 상태로 바뀜
                {
                    entity.ChangeState(MonsterState.normal); // 플레이어가 몬스터가 공격할 수 있는 범위 내에 들어오면 공격 시작
                    return;
                }
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) <= 4)
                {
                    entity.ChangeState(MonsterState.attackPlayer); // 플레이어가 몬스터가 공격할 수 있는 범위 내에 들어오면 공격 시작
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
            entity.GetHurt = false; // die로 state 전환 시 필요
            entity.Wait = 0;
        }
    }
    //죽음
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
