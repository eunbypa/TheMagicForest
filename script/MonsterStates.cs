using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* namespace : MonsterStates
 * Description : 몬스터가 가질 수 있는 상태 클래스를 묶은 namespace
 */
namespace MonsterStates
{
    // 몬스터가 가질 수 있는 상태를 직관적으로 표현하기 위해 enum을 사용했습니다.
    // normal : 평상시, chasePlayer : 플레이어 추격, attackPlayer : 플레이어 공격, hurt : 다침, die : 죽음
    public enum MonsterState { normal, chasePlayer, attackPlayer, hurt, die };
    /* Class : Normal
     * Description : 평상시 상태 클래스입니다. State<Monster> 클래스를 상속받습니다.
    */
    public class Normal : State<Monster>
    {
        /* Method : Enter
        * Description : Normal 상태 진입 시 1회 호출됩니다. 랜덤하게 움직이기 위해서 entity의 MovingChoice 메서드를 호출합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.MovingChoice();
        }
        /* Method : Execute
        * Description : Normal 상태를 빠져나가지 않는 이상 계속 호출됩니다. Normal 상태에서는 Hurt 상태 또는 ChasePlayer 상태로 전이될 수 있습니다.  
        * 몬스터가 플레이어를 인식할 수 있는 범위를 10으로 설정해서 이 범위 내에 들어오면 ChasePlayer로 전이됩니다.
        * Parameter : Monster entity - 몬스터
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
        * Description : Normal 상태에서 빠져나갈 시 1회 호출됩니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.MovingChoice();
            entity.Stop();
        }
    }
    /* Class : ChasePlayer
     * Description : 플레이어 추격 상태 클래스입니다. State<Monster> 클래스를 상속받습니다.
    */
    public class ChasePlayer : State<Monster>
    {
        /* Method : Enter
        * Description : ChasePlayer 상태 진입 시 1회 호출됩니다. 몬스터의 현재 위치에서 플레이어의 현재 위치까지 최단 경로를 계산하기 위해 entity의 SetShortestPath 메서드를 호출합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.SetShortestPath();
        }
        /* Method : Execute
        * Description : ChasePlayer 상태를 빠져나가지 않는 이상 계속 호출됩니다. Hurt 상태와 Normal 상태 AttackPlayer 상태로 전이될 수 있습니다.
        * Normal 상태로 전이되는 조건은 플레이어가 다른 맵으로 이동한 경우고 AttackPlayer 상태로 전이되는 조건은 플레이어가 몬스터의 공격 범위 내에 들어올 때 입니다. 공격 범위는 5로 정했습니다.
        * Parameter : Monster entity - 몬스터
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
        * Description : ChasePlayer 상태를 빠져나갈 때 1회 호출됩니다. 다음 상태를 시작하기 전에 잠깐 정지합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.Stop();
        }
    }
    /* Class : AttackPlayer
     * Description : 플레이어 공격 상태 클래스입니다. State<Monster> 클래스를 상속받습니다.
    */
    public class AttackPlayer : State<Monster>
    {
        /* Method : Enter
        * Description : AttackPlayer 상태 진입 시 1회 호출됩니다. entity의 Attack 메서드를 호출합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Attack();
        }
        /* Method : Execute
        * Description : AttackPlayer 상태를 빠져나가지 않는 이상 계속 호출됩니다. Hurt 상태와 ChasePlayer 상태로 전이될 수 있습니다. Wait로 1초 기다리는 이유는 몬스터의 공격 동작이 끝나는 데에 
        * 1초가 소요되기 때문입니다. 몬스터가 플레이어를 인식할 수 있는 범위 내에 들어오면 ChasePlayer로 전이됩니다.
        * Parameter : Monster entity - 몬스터
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
                if (Math.Sqrt(Math.Pow(entity.CurPos.x - GameManager.instance.PlayerPos.x, 2) + Math.Pow(entity.CurPos.y - GameManager.instance.PlayerPos.y, 2)) > 5) // 공격 범위에서 멀어졌으므로 다시 추격
                {
                    entity.ChangeState(MonsterState.chasePlayer);
                    return;
                }
                entity.Attack();
            }
        }
        /* Method : Exit
        * Description : AttackPlayer 상태를 빠져나갈 때 1회 호출됩니다. Wait값을 초기화합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.Wait = 0;
        }
    }
    /* Class : Hurt
     * Description : 다친 상태 클래스입니다. State<Monster> 클래스를 상속받습니다.
    */
    public class Hurt : State<Monster>
    {
        /* Method : Enter
        * Description : Hurt 상태 진입 시 1회 호출됩니다. entity의 Hurt 메서드를 호출합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Hurt();
        }
        /* Method : Execute
        * Description : Hurt 상태를 빠져나가지 않는 이상 계속 호출됩니다. Normal 상태와 AttackPlayer 상태, ChasePlayer 상태, Die 상태로 전이될 수 있습니다. Wait로 다친 동작을 완료할 때까지 기다립니다.
        * Normal 상태로 전이되는 조건은 플레이어가 다른 맵으로 이동한 경우고 AttackPlayer 상태로 전이되는 조건은 플레이어가 몬스터의 공격 범위 내에 들어올 때 입니다.
        * 몬스터가 플레이어를 인식할 수 있는 범위 내에 들어오면 ChasePlayer로 전이됩니다. 현재 hp가 0이 되면 Die 상태로 전이됩니다.
        * Parameter : Monster entity - 몬스터
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
        * Description : Hurt 상태를 빠져나갈 때 1회 호출됩니다. GetHurt와 Wait값을 초기화합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            entity.GetHurt = false; 
            entity.Wait = 0;
        }
    }
    /* Class : Die
     * Description : 죽은 상태 클래스입니다. State<Monster> 클래스를 상속받습니다.
    */
    public class Die : State<Monster>
    {
        /* Method : Enter
        * Description : Die 상태 진입 시 1회 호출됩니다. entity의 Die 메서드와 GiveReward 메서드를 호출합니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Enter(Monster entity)
        {
            entity.Die();
            entity.GiveReward();
        }
        /* Method : Execute
        * Description : Die 상태를 빠져나가지 않는 이상 계속 호출됩니다. Die 상태에서는 다른 상태로 전이되지 않습니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Execute(Monster entity)
        {
            entity.Die();
        }
        /* Method : Exit
        * Description : 빠져나갈 때 수행할 동작이 없어서 비워뒀습니다.
        * Parameter : Monster entity - 몬스터
        * Return Value : void
        */
        public override void Exit(Monster entity)
        {
            
        }
    }
}
