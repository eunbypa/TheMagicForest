using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    TileData parent; // �ִ� ��θ� Ž���� �� �� �ܰ迡�� ������ Ÿ���� ����Ŵ
    bool obstacle; // true�� �̵��� �� ���� ���̶�� �ǹ�
    Vector3 worldPosition; // �� Ÿ���� ���� �Ʒ� �������� �������� �� ���� �� ��ġ

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

    public bool Obstacle
    {
        get
        {
            return obstacle;
        }
    }
    public int F
    {
        get
        {
            return f;
        }
        set
        {
            f = value;
        }
    }
    public int G
    {
        get
        {
            return g;
        }
        set
        {
            g = value;
        }
    }
    public int H
    {
        get
        {
            return h;
        }
        set
        {
            h = value;
        }
    }
    public Vector3 WorldPosition
    {
        get
        {
            return worldPosition;
        }
    }

    public TileData(bool obstacle, Vector3 pos)
    {
        parent = null;
        this.obstacle = obstacle;
        f = 0;
        g = 0;
        h = 0;
        this.worldPosition = pos;
    }
}
