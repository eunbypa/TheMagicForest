using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//몬스터가 있는 맵을 설정한 크기의 타일로 나누는 맵 관리자
public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] private Tilemap[] monsterMaps; // 몬스터가 있는 맵들 
    public List<List<TileData>> tiles; // 맵을 각 타일로 나눈 데이터
    void Awake()
    {
        instance = this;
        DivideMapIntoTiles(0); // 나중에 텔레포트로 맵이 변경될 때마다 해당 맵을 타일 데이터로 나누도록 수정해야 함
    }
    void Start()
    {
       
    }
    public Tilemap[] MonsterMaps
    {
        get
        {
            return monsterMaps;
        }
    }

    public void DivideMapIntoTiles(int idx)
    {
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min)); // 맵 구역의 최소 x와 y 위치를 int 형태로 저장
        Vector3Int maxTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.max)); // 맵 구역의 최대 x와 y 위치를 int 형태로 저장

        tiles = new List<List<TileData>>();
        for (int i = minTile.y + 1; i < maxTile.y; i += 2)
        {
            tiles.Add(new List<TileData>());
            for(int j = minTile.x + 1; j < maxTile.x; j += 2)
            {
                TileData tileData;
                Vector3 pos = new Vector3(j, i, 0);
                if(monsterMaps[idx].HasTile(monsterMaps[idx].WorldToCell(pos))) tileData = new TileData(false, pos); // 타일이 있으면 갈 수 있다는 의미이므로 벽 여부 false
                else tileData = new TileData(true, pos); // 벽 여부 true
                tiles[(i - minTile.y - 1) / 2].Add(tileData);
            }
        }
        //return tiles;
    }

    public Tuple<int, int> GetCellFromWorld(Vector3Int pos, int idx) // 월드 포지션으로 셀 포지션 구하기, pos : 월드 포지션 int형태, idx : 맵 번호, Item1: 행, Item2: 열
    {
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min)); // 맵 구역의 최소 x와 y 위치를 int 형태로 저장
        int row, column;
        if(Math.Abs(minTile.x + 1) % 2 == 0)
        {
            column = Math.Abs(pos.x) % 2 != 0 ? (pos.x - minTile.x - 1 + 1) / 2 : (pos.x - minTile.x - 1) / 2;
        }
        else
        {
            column = Math.Abs(pos.x) % 2 == 0 ? (pos.x - minTile.x - 1 + 1) / 2 : (pos.x - minTile.x - 1) / 2;
        }
        if(Math.Abs(minTile.y + 1) % 2 == 0)
        {
            row = Math.Abs(pos.y) % 2 != 0 ? (pos.y - minTile.y - 1 + 1) / 2 : (pos.y - minTile.y - 1) / 2;
        }
        else
        {
            row = Math.Abs(pos.y) % 2 == 0 ? (pos.y - minTile.y - 1 + 1) / 2 : (pos.y - minTile.y - 1) / 2;
        }
        return new Tuple<int, int>(column, row); 
    }
}
