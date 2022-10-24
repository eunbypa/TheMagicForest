using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : TileData
 * Description : ���� Ÿ�� �����͸� ����ϴ� Ŭ�����Դϴ�.
 */
public class TileData
{
    TileData parent; // �ִ� ��θ� Ž���� �� �� �ܰ迡�� ������ Ÿ���� ����Ŵ
    bool obstacle; // true�� �̵��� �� ���� ���̶�� �ǹ�
    Vector3 worldPosition; // �� Ÿ���� �߽����� �������� �� ���� ��ǥ

    /* Property */
    public TileData Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;
        }
    }
    /* Property */
    public bool Obstacle
    {
        get
        {
            return obstacle;
        }
    }
    /* Property */
    public Vector3 WorldPosition
    {
        get
        {
            return worldPosition;
        }
    }
    
    // TileData ������
    // Parameter : bool obstacle - ��ֹ� ����, Vector3 pos - ��ǥ
    public TileData(bool obstacle, Vector3 pos)
    {
        parent = null;
        this.obstacle = obstacle;
        this.worldPosition = pos;
    }
}
