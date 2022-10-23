using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� Ÿ�� A : ������ ���� ���� �����ϴ� Ÿ��
public class MonsterTypeA : Monster
{
    Vector3 curPathPos; // ���� ������ �ִ� ��� ��ġ
    void Start()
    {
        SettingStates();
        stateMachine.SetState(this, states[(int)MonsterStates.MonsterState.normal]); //ó�� ����
        CurHp = maxHp;
        rb = GetComponent<Rigidbody2D>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� Rigidbody2D ������Ʈ�� �����ɴϴ�.
        ani = mainBody.GetComponent<Animator>(); // �ڽ� GameObject ��ü���� Animator ������Ʈ�� �����ɴϴ�.
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
    //������ �Ǵ� ���� ü���̳� ���� ���� ����
    public override void ResetState()
    {
        CurHp = maxHp;
        hpGraph.fillAmount = 1f;
        stateMachine.ChangeState(states[(int)MonsterStates.MonsterState.normal]);
        monsterOwnedGold.SetActive(false);
        monsterOwnedItem.SetActive(false);
    }
    //������ ���� �����ϰ� ����
    public override void MovingChoice()
    {
        if(movingChoice == null)
        {
            //Debug.Log("��������");
            movingChoice = RandomChoice();
            StartCoroutine(movingChoice);
        }

    }
    //�¿� ������
    public override void Flip(int x)
    {
        if (x == -1)
        {
            left = true;
            transform.localScale = new Vector3(-1, 1, 1); // ���� ������
        }
        else
        {
            left = false;
            transform.localScale = new Vector3(1, 1, 1); // ������ ������
        }
    }
    //�����ϰ� �ʵ� ���� ������ ������
    public override void Moving()
    {
        CurPos = transform.position;
        MovingChoice();
        Chasing();
    }
    //����
    public override void Stop()
    {
        ani.SetTrigger("stop");
        rb.velocity = new Vector2(0, 0).normalized * speed;
    }
    //�÷��̾��� ��ġ������ �ִ� ��� ����
    public override void SetShortestPath()
    {
        CurChasingDone = false;
        TargetPos = GameManager.instance.PlayerPos; // �ִ� ��θ� �����ϴ� ���� Ÿ�� �÷��̾��� ��ġ
        pathFinding.SetTiles(locatedMapNum);
        CurPos = transform.position;
        if (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(TargetPos)))
        {
            CurChasingDone = true; // ��� ��˻�
            return;
        }
        shortestPath = pathFinding.FindPath(Vector3Int.FloorToInt(CurPos), Vector3Int.FloorToInt(GameManager.instance.PlayerPos), locatedMapNum);
        curPathPos = transform.position;
    }
    //�ִ� ��θ� ���� �÷��̾ �߰�
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
            { // ���� ������ ��ġ�� ���������Ƿ� �����ϰ� ���� ������ ������
                curPathPos = shortestPath[0];
                shortestPath.RemoveAt(0);
            }
            else
            {
                ani.SetTrigger("move");
                if ((shortestPath[0].x - curPathPos.x) * lastXDirection < 0) // �ֱٿ� ������ x ���� ����, ������ �� ������ ������ ������ �ٸ��� �ǹ�(����, ���)
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
    //����
    public override void Attack()
    {
        if (GameManager.instance.PlayerPos.x >= transform.position.x && left) Flip(1); // ���������� ������
        if (GameManager.instance.PlayerPos.x < transform.position.x && !left) Flip(-1); // �������� ������
        AttackDone = false;
        ani.SetTrigger("attack");
    }
    //��ħ
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
        hpBar.SetActive(true); // hp�� Ȱ��ȭ
        if (CurHp != 0)
        {
            ani.SetTrigger("hurt");
        }
        else DyingDone = false;
    }
    //����
    public override void Die()
    {
        if (!DyingDone)
        {
            Wait += Time.deltaTime;
            hpBar.SetActive(false); // hp�� ��Ȱ��ȭ
            ani.SetTrigger("die"); // ������ ���������� ��ٸ��� ����
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
    IEnumerator RandomChoice() // �����ϰ� �����¿�, �밢�� ���� �� �ϳ��� ������ ������
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
                Debug.Log("���ѷ���");
                break;
            }
        }
        while (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(randomPos))); // �ش� ��ġ�� Ÿ���� ������ �� ����
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
        if(Other.gameObject.tag == "skill") // ���� ����
        {
            hurtDamage = Other.gameObject.GetComponent<Skill>().AttackPower;
            GetHurt = true;
        }
        if (Other.gameObject.tag == "Player") GameManager.instance.MonsterPower = AttackPower;
    }

}
