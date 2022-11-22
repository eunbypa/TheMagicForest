using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Class : MonsterTypeB
* Description : 몬스터 중 특정 물체를 던지는 방식으로 원거리 공격하는 유형의 몬스터를 담당하는 클래스입니다. Monster 클래스를 상속받습니다.
*/
public class MonsterTypeB : Monster
{
    Vector3 curPathPos; // 현재 지나고 있는 경로 위치
    [SerializeField] private GameObject mid; // 몬스터의 중심 GameObject
    [SerializeField] private GameObject[] attackSkill; // 몬스터가 공격 시 던지는 물체 GameObject 배열
    int attackIdx = 0; // 공격 패턴 상하좌우면 0, 대각선이면 4
    int[] moveX = new int[] { -1, 0, 1, 0 }; // 상하좌우
    int[] moveY = new int[] { 0, -1, 0, 1 }; 
    int[] move2X = new int[] { -1, -1, 1, 1 }; // 대각선 
    int[] move2Y = new int[] { -1, 1, -1, 1 };
    IEnumerator hideSkill; // 코루틴 변수
    WaitForSeconds wfs2; // 코루틴에서 제어권을 돌려주고 기다리는 시간
 
    void Start()
    {
        SettingStates(); // states 배열에 몬스터가 가지는 상태들을 할당
        stateMachine.SetState(this, states[(int)MonsterStates.MonsterState.normal]); //처음 상태 설정
        CurHp = maxHp; // 현재 체력을 전체 체력으로 설정
        rb = GetComponent<Rigidbody2D>(); // 현재 클래스가 할당된 GameObject 객체에서 Rigidbody2D 컴포넌트를 가져옵니다.
        ani = mainBody.GetComponent<Animator>(); // 자식 GameObject 객체에서 Animator 컴포넌트를 가져옵니다.
        minPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.min); // 몬스터가 위치한 맵의 min 좌표
        maxPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.max); // 몬스터가 위치한 맵의 max 좌표
        //Debug.Log(minPos + " " + maxPos);
        CurPos = transform.position; // 현재 좌표
        wfs = new WaitForSeconds(3f); // 대기 시간
        wfs2 = new WaitForSeconds(1f); // 대기 시간
        lastXDirection = 1; // 최근 x축 이동 방향 기본값을 1로 설정
        realSpeed = speed / (float)1000;
    }

    void Update()
    {
        CurPos = transform.position;
        stateMachine.Execute();
    }

    /* Method : ResetState
     * Description : 리스폰 되는 순간 현재 체력과 상태 등을 초기화하는 메서드입니다.
     * Return Value : void
     */
    public override void ResetState()
    {
        for (int i = attackIdx; i < attackIdx + 4; i++)
        {
            attackSkill[i].SetActive(false);
            //attackSkill[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0).normalized;
        }
        hpBar.SetActive(false);
        CurHp = maxHp;
        hpGraph.fillAmount = 1f;
        movingChoice = null;
        if (!firstPos.Equals(new Vector3(0, 0, 0))) transform.position = firstPos;
        stateMachine.ChangeState(states[(int)MonsterStates.MonsterState.normal]);
    }
    /* Method : MovingChoice
     * Description : RandomChoice 코루틴을 통해 다음으로 이동할 위치를 랜덤하게 선택하는 메서드입니다.
     * Return Value : void
     */
    public override void MovingChoice()
    {
        if (firstPos.Equals(new Vector3(0, 0, 0)))
        {
            firstPos = transform.position;
        }
        if (movingChoice == null)
        {
            movingChoice = RandomChoice();
            StartCoroutine(movingChoice);
        }

    }
    /* Method : Flip
     * Description : x축 방향에 따라 좌우 뒤집기 동작을 수행하는 메서드입니다.
     * Parameter : int x - x축 방향을 의미, -1 : 왼쪽, 1 : 오른쪽
     * Return Value : void
     */
    public override void Flip(int x)
    {
        if (x == -1)
        {
            mainBody.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            mainBody.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    /* Method : Moving
     * Description : MovingChoice 메서드를 먼저 호출해 다음으로 이동할 목적지에 대한 최단 경로를 세팅하고 Chasing 메서드를 호출해서 해당 경로를 따라 이동하도록 하는 메서드입니다. 
     * Return Value : void
     */
    public override void Moving()
    {
        MovingChoice();
        Chasing();
    }
    /* Method : Stop
     * Description : 정지 동작을 수행하는 메서드입니다.
     * Return Value : void
     */
    public override void Stop()
    {
        ani.SetTrigger("stop");
        rb.velocity = new Vector2(0, 0).normalized * speed;
    }
    /* Method : SetShortestPath
     * Description : 몬스터의 현재 위치에서 플레이어의 현재 위치까지 최단 경로를 세팅하기 위한 메서드입니다. 최단 경로를 세팅하는 시점의 플레이어의 위치를 TargetPos에 저장합니다.
     * curPathPos는 현재 경로 상 위치를 파악하기 위함으로 처음엔 현재 위치를 저장합니다.
     * Return Value : void
     */
    public override void SetShortestPath()
    {
        TargetPos = GameManager.instance.PlayerPos;
        pathFinding.SetTiles();
        shortestPath = pathFinding.FindPath(Vector3Int.FloorToInt(CurPos), Vector3Int.FloorToInt(GameManager.instance.PlayerPos), locatedMapNum);
        curPathPos = transform.position;
    }
    /* Method : Chasing
     * Description : 계산된 최단 경로를 따라 목적지까지 이동하는 동작을 수행하는 메서드입니다. Vector3의 MoveTowards로 현재 위치에서 다음 위치까지 이동합니다. 현재 순서의 이동을 완료하면 경로의 맨 앞에 있는 원소를
     * 제거하고 다음 순서의 위치로 이동합니다. 이동 중에 좌우 방향이 바뀔 때마다 Flip 메서드를 호출해서 해당 방향에 맞게 뒤집어지도록 구현했습니다.
     * Return Value : void
     */
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
            {
                curPathPos = shortestPath[0];
                shortestPath.RemoveAt(0);
            }
            else
            {
                ani.SetTrigger("move");
                if ((shortestPath[0].x - curPathPos.x) * lastXDirection < 0)
                {
                    lastXDirection = (shortestPath[0].x - curPathPos.x) > 0 ? 1 : -1;
                    Flip(lastXDirection);
                }
                transform.position = Vector3.MoveTowards(gameObject.transform.position, shortestPath[0], realSpeed);
            }
        }
        else
        {
            movingChoice = null;
            Stop();
        }
    }
    /* Method : Attack
     * Description : 몬스터의 공격 동작을 수행하는 메서드입니다.플레이어의 위치가 현재 위치를 기준으로 왼쪽인지 오른쪽인지 파악해서 알맞은 방향으로 시선이 가도록 Flip 메서드를 호출합니다.
     * Return Value : void
     */
    public override void Attack()
    {
        if (GameManager.instance.PlayerPos.x >= transform.position.x && lastXDirection == -1)
        {
            lastXDirection = 1;
            Flip(lastXDirection);
        }
        if (GameManager.instance.PlayerPos.x < transform.position.x && lastXDirection == 1)
        {
            lastXDirection = -1;
            Flip(lastXDirection);
        }
        AttackDone = false;
        GameManager.instance.MonsterPower = AttackPower;
        ani.SetTrigger("attack");
        attackIdx = attackIdx == 0 ? 4 : 0;
        for(int i = attackIdx; i < attackIdx + 4; i++)
        {
            //Debug.Log(i + "활성화");
            attackSkill[i].transform.position = mid.transform.position;
            attackSkill[i].SetActive(true);
            if (attackIdx == 0) attackSkill[i].GetComponent<Rigidbody2D>().velocity = new Vector2(moveX[i], moveY[i]).normalized * 7;
            else attackSkill[i].GetComponent<Rigidbody2D>().velocity = new Vector2(move2X[i - attackIdx], move2Y[i - attackIdx]).normalized * 7;
        }
        
        hideSkill = HideSkill();
        StartCoroutine(hideSkill);
    }
    /* Method : Hurt
     * Description : 몬스터가 다칠 때 관련된 동작을 수행하는 메서드입니다. 현재 hp를 받은 데미지만큼 깎고 체력 UI도 감소한 체력만큼 깎습니다. 현재 hp가 0이 아닐 때만 다친 동작의 애니메이션을 실행합니다.
     * Return Value : void
     */
    public override void Hurt()
    {
        EffectSoundManager.instance.PlayEffectSound("hurt");
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
        hpBar.SetActive(true);
        if (CurHp != 0)
        {
            ani.SetTrigger("hurt");
        }
        else DyingDone = false;
    }
    /* Method : Die
     * Description : 몬스터가 죽을 때 관련된 동작을 수행하는 메서드입니다. 죽는 동작의 애니메이션이 다 끝나면 MonsterManager의 DieEvent 메서드를 호출합니다.
     * Return Value : void
     */
    public override void Die()
    {
        if (!DyingDone)
        {
            Wait += Time.deltaTime;
            hpBar.SetActive(false);
            ani.SetTrigger("die");
            if (Wait >= 0.25f)
            {
                DyingDone = true;
                Wait = 0;
            }
        }
        else
        {
            StopCoroutine(hideSkill); // 즉시 비활성화
            for (int i = attackIdx; i < attackIdx + 4; i++)
            {
                attackSkill[i].SetActive(false);
                //Debug.Log(i + "비활성화");
            }
            DyingDone = false;
            MonsterManager.instance.DieEvent(gameObject, ExpValue, MonsterId);
        }
    }
    /* Coroutine : RandomChoice
     * Description : 이동과 멈춤 두 단계를 대기 시간을 주기로 반복하는 형태를 가지는 코루틴입니다. 이동의 경우 몬스터의 위치를 기준으로 주위에 거리가 5 이내인 곳에서 랜덤하게 한 위치를 정하고 해당 위치까지의 
     * 최단 경로를 계산합니다. 이 때, 랜덤하게 정해진 위치가 이동할 수 없는 위치일 수 있으므로 do-while문으로 이동 가능한 위치가 선정될 때까지 반복문을 돌리도록 구현했습니다. 이 과정에서 혹시나 무한루프를 탈
     * 까 싶어서 반복이 최대 10000을 넘어가면 루프를 탈출하게끔 했습니다.
     */
    IEnumerator RandomChoice()
    {
        int x = 0, y = 0, loop = 0;
        Vector3 randomPos;
        if (shortestPath != null)
        {
            shortestPath = null;
            yield return wfs;
            movingChoice = null;
            yield break;
        }
        do
        {
            //Debug.Log("현재 위치 : " + Vector3Int.FloorToInt(transform.position));
            if (Vector3Int.FloorToInt(transform.position).x - 5 > Vector3Int.FloorToInt(minPos).x && Vector3Int.FloorToInt(transform.position).x + 5 < Vector3Int.FloorToInt(maxPos).x)
                x = Random.Range(Vector3Int.FloorToInt(transform.position).x - 5, Vector3Int.FloorToInt(transform.position).x + 5);
            else if (Vector3Int.FloorToInt(transform.position).x - 5 > Vector3Int.FloorToInt(minPos).x)
                x = Random.Range(Vector3Int.FloorToInt(transform.position).x - 5, Vector3Int.FloorToInt(transform.position).x);
            else
                x = Random.Range(Vector3Int.FloorToInt(transform.position).x + 1, Vector3Int.FloorToInt(transform.position).x + 5);
            if (Vector3Int.FloorToInt(transform.position).y - 5 > Vector3Int.FloorToInt(minPos).y && Vector3Int.FloorToInt(transform.position).y + 5 < Vector3Int.FloorToInt(maxPos).y)
                y = Random.Range(Vector3Int.FloorToInt(transform.position).y - 5, Vector3Int.FloorToInt(transform.position).y + 5);
            else if (Vector3Int.FloorToInt(transform.position).y - 5 > Vector3Int.FloorToInt(minPos).y)
                y = Random.Range(Vector3Int.FloorToInt(transform.position).y - 5, Vector3Int.FloorToInt(transform.position).y);
            else
                y = Random.Range(Vector3Int.FloorToInt(transform.position).y + 1, Vector3Int.FloorToInt(transform.position).y + 5);
            //Debug.Log(x);
            if ((minPos.x + 1) % 2 == 0) x = (x % 2 == 0) ? x : x + 1;
            else x = (x % 2 == 1) ? x : x + 1;
            if ((minPos.y + 1) % 2 == 0) y = (y % 2 == 0) ? y : y + 1;
            else y = (y % 2 == 1) ? y : y + 1;
            //Debug.Log(x);
            randomPos = new Vector3(x, y, 0);
            //Debug.Log("다음 위치 " + randomPos);
            loop++;
            if (loop > 10000)
            {
                break;
            }
        }
        while (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(randomPos))); // 해당 위치에 타일이 없으면 재 연산
        pathFinding.SetTiles();
        CurPos = transform.position;
        if (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(CurPos))) yield break;
        //Debug.Log(CurPos + " " + randomPos);
        shortestPath = pathFinding.FindPath(Vector3Int.FloorToInt(CurPos), Vector3Int.FloorToInt(randomPos), locatedMapNum);
        if (shortestPath == null)
        {
            movingChoice = null;
        }
        curPathPos = transform.position;
        yield break;
    }
    /* Coroutine : HideSkill
     * Description : 지정한 시간 이후 사용한 공격 스킬을 비활성화하는 메서드입니다.
     */
    IEnumerator HideSkill()
    {
        int aIdx = attackIdx;
        yield return wfs2;
        for (int i = aIdx; i < aIdx + 4; i++)
        {
            attackSkill[i].SetActive(false);
            //Debug.Log(i + "비활성화");
        }
        yield break;
    }
    //충돌 처리
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "skill") // 플레이어의 스킬 공격 받음
        {
            hurtDamage = Other.gameObject.GetComponent<Skill>().AttackPower;
            GetHurt = true;
        }
        if (Other.gameObject.tag == "Player") GameManager.instance.MonsterPower = AttackPower; // 플레이어와 충돌하면 자신의 공격력을 GameManager에 전달
    }

}
