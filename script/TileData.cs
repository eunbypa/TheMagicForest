using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : TileData
 * Description : 맵의 타일 데이터를 담당하는 클래스입니다.
 */
public class TileData
{
    TileData parent; // 최단 경로를 탐색할 때 전 단계에서 지나온 타일을 가리킴
    bool obstacle; // true면 이동할 수 없는 벽이라는 의미
    Vector3 worldPosition; // 이 타일의 중심점을 기준으로 한 월드 좌표

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
    
    // TileData 생성자
    // Parameter : bool obstacle - 장애물 여부, Vector3 pos - 좌표
    public TileData(bool obstacle, Vector3 pos)
    {
        parent = null;
        this.obstacle = obstacle;
        this.worldPosition = pos;
    }
}
