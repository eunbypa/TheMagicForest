using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    TileData parent; // 최단 경로를 탐색할 때 전 단계에서 지나온 타일을 가리킴
    bool obstacle; // true면 이동할 수 없는 벽이라는 의미
    Vector3 worldPosition; // 이 타일의 왼쪽 아래 꼭짓점을 기준으로 한 월드 내 위치

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
