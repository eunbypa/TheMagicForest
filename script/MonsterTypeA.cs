using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//몬스터 타입 A : 몸으로 직접 근접 공격하는 타입
public class MonsterTypeA : Monster
{
    Vector3 curPathPos; // 현재 지나고 있는 경로 위치
    void Start()
    {
        SettingStates();
        stateMachine.SetState(this, states[(int)MonsterStates.MonsterState.normal]); //처음 상태
        CurHp = maxHp;
        rb = GetComponent<Rigidbody2D>(); // 현재 클래스가 할당된 GameObject 객체에서 Rigidbody2D 컴포넌트를 가져옵니다.
        ani = mainBody.GetComponent<Animator>(); // 자식 GameObject 객체에서 Animator 컴포넌트를 가져옵니다.
        minPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.min);
        maxPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.max);
        CurPos = transform.position;
        wfs = new WaitForSeconds(3f);
        lastXDirection = 1;
    }

    void Update()
    {
        stateMachine.Execute();
    }
    //리스폰 되는 순간 체력이나 상태 등을 리셋
    public override void ResetState()
    {
        CurHp = maxHp;
        hpGraph.fillAmount = 1f;
        stateMachine.ChangeState(states[(int)MonsterStates.MonsterState.normal]);
        monsterOwnedGold.SetActive(false);
        monsterOwnedItem.SetActive(false);
    }
    //움직일 방향 랜덤하게 선택
    public override void MovingChoice()
    {
        if(movingChoice == null)
        {
            //Debug.Log("랜덤선택");
            movingChoice = RandomChoice();
            StartCoroutine(movingChoice);
        }

    }
    //좌우 뒤집기
    public override void Flip(int x)
    {
        if (x == -1)
        {
            left = true;
            transform.localScale = new Vector3(-1, 1, 1); // 왼쪽 뒤집기
        }
        else
        {
            left = false;
            transform.localScale = new Vector3(1, 1, 1); // 오른쪽 뒤집기
        }
    }
    //랜덤하게 필드 범위 내에서 움직임
    public override void Moving()
    {
        CurPos = transform.position;
        MovingChoice();
        Chasing();
    }
    //정지
    public override void Stop()
    {
        ani.SetTrigger("stop");
        rb.velocity = new Vector2(0, 0).normalized * speed;
    }
    //플레이어의 위치까지의 최단 경로 세팅
    public override void SetShortestPath()
    {
        CurChasingDone = false;
        TargetPos = GameManager.instance.PlayerPos; // 최단 경로를 세팅하는 순간 타겟 플레이어의 위치
        pathFinding.SetTiles(locatedMapNum);
        CurPos = transform.position;
        if (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(TargetPos)))
        {
            CurChasingDone = true; // 경로 재검사
            return;
        }
        shortestPath = pathFinding.FindPath(Vector3Int.FloorToInt(CurPos), Vector3Int.FloorToInt(GameManager.instance.PlayerPos), locatedMapNum);
        curPathPos = transform.position;
    }
    //최단 경로를 따라 플레이어를 추격
    public override void Chasing()
    {
        CurPos = transform.position;
        if (shortestPath == null)
        {
            Stop();
            return;
        }

        if (shortestPath.Count != 0)
        {
            if (transform.position.Equals(shortestPath[0]))
            { // 현재 순서의 위치에 도달했으므로 삭제하고 다음 순서를 가져옴
                curPathPos = shortestPath[0];
                shortestPath.RemoveAt(0);
            }
            else
            {
                ani.SetTrigger("move");
                if ((shortestPath[0].x - curPathPos.x) * lastXDirection < 0) // 최근에 움직인 x 방향 갱신, 곱했을 때 음수가 나오면 방향이 다름을 의미(음수, 양수)
                {
                    lastXDirection = (shortestPath[0].x - curPathPos.x) > 0 ? 1 : -1;
                    Flip(lastXDirection);
                }
                transform.position = Vector3.MoveTowards(gameObject.transform.position, shortestPath[0], 0.055f);
            }
        }
        else
        {
            movingChoice = null;
            CurChasingDone = true;
            Stop();
        }
    }
    //공격
    public override void Attack()
    {
        if (GameManager.instance.PlayerPos.x >= transform.position.x && left) Flip(1); // 오른쪽으로 뒤집기
        if (GameManager.instance.PlayerPos.x < transform.position.x && !left) Flip(-1); // 왼쪽으로 뒤집기
        AttackDone = false;
        ani.SetTrigger("attack");
    }
    //다침
    public override void Hurt()
    { 
        CurHp -= hurtDamage;
        if (CurHp >= 0)
        {
            float percent = (float)((float)(hurtDamage) * (1.0 / (float)(maxHp)));
            hpGraph.fillAmount -= percent;
        }
        else
        {
            CurHp = 0;
            hpGraph.fillAmount = 0f;
        }
        hpBar.SetActive(true); // hp바 활성화
        if (CurHp != 0)
        {
            ani.SetTrigger("hurt");
        }
        else DyingDone = false;
    }
    //죽음
    public override void Die()
    {
        if (!DyingDone)
        {
            Wait += Time.deltaTime;
            hpBar.SetActive(false); // hp바 비활성화
            ani.SetTrigger("die"); // 동작이 끝날때까지 기다리기 위함
            if (Wait >= 0.25f)
            {
                DyingDone = true;
                Wait = 0;
            }
        }
        else
        {
            DyingDone = false;
            MonsterManager.instance.DieEvent(gameObject, ExpValue, MonsterId);
        }
    }
    IEnumerator RandomChoice() // 랜덤하게 상하좌우, 대각선 방향 중 하나의 방향을 선택함
    {
        int x = 0, y = 0, loop = 0;
        Vector3 randomPos;
        if(shortestPath != null)
        {
            shortestPath = null;
            yield return wfs;
            movingChoice = null;
            yield break;
        }
        do
        {
            if (Vector3Int.FloorToInt(transform.position).x - 5 > Vector3Int.FloorToInt(minPos).x && Vector3Int.FloorToInt(transform.position).x + 5 < Vector3Int.FloorToInt(maxPos).x)
                x = rand.Next(Vector3Int.FloorToInt(transform.position).x - 5, Vector3Int.FloorToInt(transform.position).x + 5);
            else if (Vector3Int.FloorToInt(transform.position).x - 5 > Vector3Int.FloorToInt(minPos).x)
                x = rand.Next(Vector3Int.FloorToInt(transform.position).x - 5, Vector3Int.FloorToInt(transform.position).x);
            else
                x = rand.Next(Vector3Int.FloorToInt(transform.position).x + 1, Vector3Int.FloorToInt(transform.position).x + 5);
            if (Vector3Int.FloorToInt(transform.position).y - 5 > Vector3Int.FloorToInt(minPos).y && Vector3Int.FloorToInt(transform.position).y + 5 < Vector3Int.FloorToInt(maxPos).y)
                y = rand.Next(Vector3Int.FloorToInt(transform.position).y - 5, Vector3Int.FloorToInt(transform.position).y + 5);
            else if (Vector3Int.FloorToInt(transform.position).y - 5 > Vector3Int.FloorToInt(minPos).y)
                y = rand.Next(Vector3Int.FloorToInt(transform.position).y - 5, Vector3Int.FloorToInt(transform.position).y);
            else
                y = rand.Next(Vector3Int.FloorToInt(transform.position).y + 1, Vector3Int.FloorToInt(transform.position).y + 5);
            if ((minPos.x + 1) % 2 == 0) x = (x % 2 == 0) ? x : x + 1;
            else x = (x % 2 == 1) ? x : x + 1;
            if ((minPos.y + 1) % 2 == 0) x = (y % 2 == 0) ? y : y + 1;
            else y = (y % 2 == 1) ? y : y + 1;
            randomPos = new Vector3(x, y, 0);
            loop++;
            if (loop > 10000)
            {
                Debug.Log("무한루프");
                break;
            }
        }
        while (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(randomPos))); // 해당 위치에 타일이 없으면 재 연산
        pathFinding.SetTiles(locatedMapNum);
        CurPos = transform.position;
        if (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(CurPos))) yield break;
        shortestPath = pathFinding.FindPath(Vector3Int.FloorToInt(CurPos), Vector3Int.FloorToInt(randomPos), locatedMapNum);
        if(shortestPath == null)
        {
            movingChoice = null;
        }
        curPathPos = transform.position;
        yield break;
    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.gameObject.tag == "skill") // 공격 받음
        {
            hurtDamage = Other.gameObject.GetComponent<Skill>().AttackPower;
            GetHurt = true;
        }
        if (Other.gameObject.tag == "Player") GameManager.instance.MonsterPower = AttackPower;
    }

}
