using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Class : CameraMoving
 * Description : ���� ȭ�� ��� ī�޶��� �������� �����ϴ� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class CameraMoving : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private Tilemap[] maps; // ����Ƽ Ÿ�ϸ� �迭
    [SerializeField] private GameObject player; // �÷��̾�
    //[SerializeField] private GameObject gM; // ���� ������ GameManager

    int curMapNum; // ���� �� ��ȣ
    float height; // ����
    float width; // �ʺ�
    float maxX; // �ִ� x��
    float minX; // �ּ� x��
    float maxY; // �ִ� y��
    float minY; // �ּ� y��

    Camera camera; // ����Ƽ ī�޶� ������Ʈ
    Vector3 playerPos; // �÷��̾��� ��ġ�� ��Ÿ���� 3���� ����
    //GameManager gm; // ���� ������ GameManager Ŭ���� ��ü

    void Start()
    {
        //this.gm = gM.GetComponent<GameManager>(); // gM GameObject ��ü�� �Ҵ�� GameManager Ŭ���� ������Ʈ�� �����ɴϴ�.
        this.curMapNum = GameManager.instance.CurMapNum; // gm���Լ� �÷��̾ ���� �ִ� �� ��ȣ ���� �����ɴϴ�.
        this.camera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Main Camera �̸��� ������ GameObject ��ü���� ī�޶� ������Ʈ�� �����ɴϴ�.
        this.height = 2f * camera.orthographicSize; // camera.orthograpicSize �� ī�޶� ������ ���� ������ ���� ��Ÿ���� ���� ����, �� ���̸� ���ϱ� ���� �ű⿡ 2�踦 �ٽ� ���ϴ� ������� �����߽��ϴ�.
        this.width = height * camera.aspect; // ī�޶� ������ ���� ����, �ʺ�� ���� ���̿� ȭ�� ������ ����ϹǷ� ���̿� ī�޶� ȭ�� ������ ���ϴ� ������� �����߽��ϴ�.
        SetLimit(); // ���� ������ ī�޶� �̵� ���� ������ �����մϴ�.
        CheckPlayerPos(); // ���� ������ �÷��̾��� ��ġ�� ������ ������ ���� ������ ������� �˻��մϴ�.
    }

    void Update()
    {
        Moving();
    }

    /* Method : SetLimit
     * Description : ī�޶��� �̵� ������ �����ϴ� ������ �����ϴ� �޼����Դϴ�. ���� �� ��ȣ�� �������� �ش� Ÿ�ϸ��� �ּ� Ÿ�� ��ġ�� �ִ� Ÿ�� ��ġ�� �ľ��� ���� ī�޶��� ������ ���� ����� �ʰ� �ϱ� ����
     * �̸� ����� ���̿� �ʺ� ���� �̿��ؼ� ī�޶� ��ġ�� x��� y�� �ִ� �ּҸ� �����մϴ�.
     * Return Value : void
     */
    void SetLimit()
    {
        Vector3 minTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.min);
        Vector3 maxTile = maps[curMapNum].CellToWorld(maps[curMapNum].cellBounds.max);
        minX = minTile.x + width / 2;
        maxX = maxTile.x - width / 2;
        minY = minTile.y + height / 2;
        maxY = maxTile.y - height / 2;
    }

    /* Method : CheckPlayerPos
     * Description : �÷��̾��� ���� ��ġ�� �ľ��ؼ� ī�޶��� �̵� �������� ������� �˻��ϴ� �޼����Դϴ�. ����� �ʾ����� �÷��̾��� ��ġ�� �״�� ī�޶� ��ġ�� �����մϴ�.
     * ���� x �Ǵ� y ���� ���� ������ ��� ��� ��� ���� ���� ������ �ּ� �Ǵ� �ִ� ������ �����ϰ� ī�޶� ��ġ�� �����ϴ� ������� �����߽��ϴ�.
     * Return Value : void
     */
    void CheckPlayerPos()
    {
        playerPos = this.player.transform.position;
        if (playerPos.x >= minX && playerPos.x <= maxX)
        {
            transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
        }
        if (playerPos.y >= minY && playerPos.y <= maxY)
        {
            transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
        }
        if (playerPos.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (playerPos.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (playerPos.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        if (playerPos.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }

    /* Method : Moving
     * Description : �÷��̾��� �����ӿ� ���� ī�޶� �����̵��� �ϴ� �޼����Դϴ�. ���� �̵� �� �÷��̾ ��Ż�� �̿��ؼ� ���� �� ��ȣ�� ����Ǵ� ��� �ش� �� ��ȣ�� �ٲٰ� �� �ʿ� �˸��� �̵� ���� ������
     * �������� SetLimit �޼��带 ȣ���ϴ� ������� �����߽��ϴ�.
     * Return Value : void
     */
    void Moving()
    {
        if (this.curMapNum != GameManager.instance.CurMapNum)
        {
            this.curMapNum = GameManager.instance.CurMapNum;
            SetLimit();
        }
        CheckPlayerPos();
    }
}
