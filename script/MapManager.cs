using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//���Ͱ� �ִ� ���� ������ ũ���� Ÿ�Ϸ� ������ �� ������
public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] private Tilemap[] monsterMaps; // ���Ͱ� �ִ� �ʵ� 
    public List<List<TileData>> tiles; // ���� �� Ÿ�Ϸ� ���� ������
    void Awake()
    {
        instance = this;
        DivideMapIntoTiles(0); // ���߿� �ڷ���Ʈ�� ���� ����� ������ �ش� ���� Ÿ�� �����ͷ� �������� �����ؾ� ��
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
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min)); // �� ������ �ּ� x�� y ��ġ�� int ���·� ����
        Vector3Int maxTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.max)); // �� ������ �ִ� x�� y ��ġ�� int ���·� ����

        tiles = new List<List<TileData>>();
        for (int i = minTile.y + 1; i < maxTile.y; i += 2)
        {
            tiles.Add(new List<TileData>());
            for(int j = minTile.x + 1; j < maxTile.x; j += 2)
            {
                TileData tileData;
                Vector3 pos = new Vector3(j, i, 0);
                if(monsterMaps[idx].HasTile(monsterMaps[idx].WorldToCell(pos))) tileData = new TileData(false, pos); // Ÿ���� ������ �� �� �ִٴ� �ǹ��̹Ƿ� �� ���� false
                else tileData = new TileData(true, pos); // �� ���� true
                tiles[(i - minTile.y - 1) / 2].Add(tileData);
            }
        }
        //return tiles;
    }

    public Tuple<int, int> GetCellFromWorld(Vector3Int pos, int idx) // ���� ���������� �� ������ ���ϱ�, pos : ���� ������ int����, idx : �� ��ȣ, Item1: ��, Item2: ��
    {
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min)); // �� ������ �ּ� x�� y ��ġ�� int ���·� ����
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
