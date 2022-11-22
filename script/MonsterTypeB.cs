using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Class : MonsterTypeB
* Description : ���� �� Ư�� ��ü�� ������ ������� ���Ÿ� �����ϴ� ������ ���͸� ����ϴ� Ŭ�����Դϴ�. Monster Ŭ������ ��ӹ޽��ϴ�.
*/
public class MonsterTypeB : Monster
{
    Vector3 curPathPos; // ���� ������ �ִ� ��� ��ġ
    [SerializeField] private GameObject mid; // ������ �߽� GameObject
    [SerializeField] private GameObject[] attackSkill; // ���Ͱ� ���� �� ������ ��ü GameObject �迭
    int attackIdx = 0; // ���� ���� �����¿�� 0, �밢���̸� 4
    int[] moveX = new int[] { -1, 0, 1, 0 }; // �����¿�
    int[] moveY = new int[] { 0, -1, 0, 1 }; 
    int[] move2X = new int[] { -1, -1, 1, 1 }; // �밢�� 
    int[] move2Y = new int[] { -1, 1, -1, 1 };
    IEnumerator hideSkill; // �ڷ�ƾ ����
    WaitForSeconds wfs2; // �ڷ�ƾ���� ������� �����ְ� ��ٸ��� �ð�
 
    void Start()
    {
        SettingStates(); // states �迭�� ���Ͱ� ������ ���µ��� �Ҵ�
        stateMachine.SetState(this, states[(int)MonsterStates.MonsterState.normal]); //ó�� ���� ����
        CurHp = maxHp; // ���� ü���� ��ü ü������ ����
        rb = GetComponent<Rigidbody2D>(); // ���� Ŭ������ �Ҵ�� GameObject ��ü���� Rigidbody2D ������Ʈ�� �����ɴϴ�.
        ani = mainBody.GetComponent<Animator>(); // �ڽ� GameObject ��ü���� Animator ������Ʈ�� �����ɴϴ�.
        minPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.min); // ���Ͱ� ��ġ�� ���� min ��ǥ
        maxPos = MapManager.instance.MonsterMaps[locatedMapNum].CellToWorld(MapManager.instance.MonsterMaps[locatedMapNum].cellBounds.max); // ���Ͱ� ��ġ�� ���� max ��ǥ
        //Debug.Log(minPos + " " + maxPos);
        CurPos = transform.position; // ���� ��ǥ
        wfs = new WaitForSeconds(3f); // ��� �ð�
        wfs2 = new WaitForSeconds(1f); // ��� �ð�
        lastXDirection = 1; // �ֱ� x�� �̵� ���� �⺻���� 1�� ����
        realSpeed = speed / (float)1000;
    }

    void Update()
    {
        CurPos = transform.position;
        stateMachine.Execute();
    }

    /* Method : ResetState
     * Description : ������ �Ǵ� ���� ���� ü�°� ���� ���� �ʱ�ȭ�ϴ� �޼����Դϴ�.
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
     * Description : RandomChoice �ڷ�ƾ�� ���� �������� �̵��� ��ġ�� �����ϰ� �����ϴ� �޼����Դϴ�.
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
     * Description : x�� ���⿡ ���� �¿� ������ ������ �����ϴ� �޼����Դϴ�.
     * Parameter : int x - x�� ������ �ǹ�, -1 : ����, 1 : ������
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
     * Description : MovingChoice �޼��带 ���� ȣ���� �������� �̵��� �������� ���� �ִ� ��θ� �����ϰ� Chasing �޼��带 ȣ���ؼ� �ش� ��θ� ���� �̵��ϵ��� �ϴ� �޼����Դϴ�. 
     * Return Value : void
     */
    public override void Moving()
    {
        MovingChoice();
        Chasing();
    }
    /* Method : Stop
     * Description : ���� ������ �����ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public override void Stop()
    {
        ani.SetTrigger("stop");
        rb.velocity = new Vector2(0, 0).normalized * speed;
    }
    /* Method : SetShortestPath
     * Description : ������ ���� ��ġ���� �÷��̾��� ���� ��ġ���� �ִ� ��θ� �����ϱ� ���� �޼����Դϴ�. �ִ� ��θ� �����ϴ� ������ �÷��̾��� ��ġ�� TargetPos�� �����մϴ�.
     * curPathPos�� ���� ��� �� ��ġ�� �ľ��ϱ� �������� ó���� ���� ��ġ�� �����մϴ�.
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
     * Description : ���� �ִ� ��θ� ���� ���������� �̵��ϴ� ������ �����ϴ� �޼����Դϴ�. Vector3�� MoveTowards�� ���� ��ġ���� ���� ��ġ���� �̵��մϴ�. ���� ������ �̵��� �Ϸ��ϸ� ����� �� �տ� �ִ� ���Ҹ�
     * �����ϰ� ���� ������ ��ġ�� �̵��մϴ�. �̵� �߿� �¿� ������ �ٲ� ������ Flip �޼��带 ȣ���ؼ� �ش� ���⿡ �°� ������������ �����߽��ϴ�.
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
     * Description : ������ ���� ������ �����ϴ� �޼����Դϴ�.�÷��̾��� ��ġ�� ���� ��ġ�� �������� �������� ���������� �ľ��ؼ� �˸��� �������� �ü��� ������ Flip �޼��带 ȣ���մϴ�.
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
            //Debug.Log(i + "Ȱ��ȭ");
            attackSkill[i].transform.position = mid.transform.position;
            attackSkill[i].SetActive(true);
            if (attackIdx == 0) attackSkill[i].GetComponent<Rigidbody2D>().velocity = new Vector2(moveX[i], moveY[i]).normalized * 7;
            else attackSkill[i].GetComponent<Rigidbody2D>().velocity = new Vector2(move2X[i - attackIdx], move2Y[i - attackIdx]).normalized * 7;
        }
        
        hideSkill = HideSkill();
        StartCoroutine(hideSkill);
    }
    /* Method : Hurt
     * Description : ���Ͱ� ��ĥ �� ���õ� ������ �����ϴ� �޼����Դϴ�. ���� hp�� ���� ��������ŭ ��� ü�� UI�� ������ ü�¸�ŭ ����ϴ�. ���� hp�� 0�� �ƴ� ���� ��ģ ������ �ִϸ��̼��� �����մϴ�.
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
     * Description : ���Ͱ� ���� �� ���õ� ������ �����ϴ� �޼����Դϴ�. �״� ������ �ִϸ��̼��� �� ������ MonsterManager�� DieEvent �޼��带 ȣ���մϴ�.
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
            StopCoroutine(hideSkill); // ��� ��Ȱ��ȭ
            for (int i = attackIdx; i < attackIdx + 4; i++)
            {
                attackSkill[i].SetActive(false);
                //Debug.Log(i + "��Ȱ��ȭ");
            }
            DyingDone = false;
            MonsterManager.instance.DieEvent(gameObject, ExpValue, MonsterId);
        }
    }
    /* Coroutine : RandomChoice
     * Description : �̵��� ���� �� �ܰ踦 ��� �ð��� �ֱ�� �ݺ��ϴ� ���¸� ������ �ڷ�ƾ�Դϴ�. �̵��� ��� ������ ��ġ�� �������� ������ �Ÿ��� 5 �̳��� ������ �����ϰ� �� ��ġ�� ���ϰ� �ش� ��ġ������ 
     * �ִ� ��θ� ����մϴ�. �� ��, �����ϰ� ������ ��ġ�� �̵��� �� ���� ��ġ�� �� �����Ƿ� do-while������ �̵� ������ ��ġ�� ������ ������ �ݺ����� �������� �����߽��ϴ�. �� �������� Ȥ�ó� ���ѷ����� Ż
     * �� �; �ݺ��� �ִ� 10000�� �Ѿ�� ������ Ż���ϰԲ� �߽��ϴ�.
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
            //Debug.Log("���� ��ġ : " + Vector3Int.FloorToInt(transform.position));
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
            //Debug.Log("���� ��ġ " + randomPos);
            loop++;
            if (loop > 10000)
            {
                break;
            }
        }
        while (!MapManager.instance.MonsterMaps[locatedMapNum].HasTile(MapManager.instance.MonsterMaps[locatedMapNum].WorldToCell(randomPos))); // �ش� ��ġ�� Ÿ���� ������ �� ����
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
     * Description : ������ �ð� ���� ����� ���� ��ų�� ��Ȱ��ȭ�ϴ� �޼����Դϴ�.
     */
    IEnumerator HideSkill()
    {
        int aIdx = attackIdx;
        yield return wfs2;
        for (int i = aIdx; i < aIdx + 4; i++)
        {
            attackSkill[i].SetActive(false);
            //Debug.Log(i + "��Ȱ��ȭ");
        }
        yield break;
    }
    //�浹 ó��
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "skill") // �÷��̾��� ��ų ���� ����
        {
            hurtDamage = Other.gameObject.GetComponent<Skill>().AttackPower;
            GetHurt = true;
        }
        if (Other.gameObject.tag == "Player") GameManager.instance.MonsterPower = AttackPower; // �÷��̾�� �浹�ϸ� �ڽ��� ���ݷ��� GameManager�� ����
    }

}
