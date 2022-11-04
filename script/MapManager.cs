using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Class : MapManager
 * Description : ���Ͱ� �ִ� ���� ������ ũ���� Ÿ�Ϸ� ������ �׷��� ������ 2���� Ÿ�� �迭�� �������� �ؼ� ���� ��ǥ�� �� ��ǥ�� ����ϴ� �۾��� �ϴ� �� ������ Ŭ�����Դϴ�. 
 * ����Ƽ�� ���� �ֱ� �Լ����� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�.
 */
public class MapManager : MonoBehaviour
{
    public static MapManager instance; // �̱��� ����
    [SerializeField] private Tilemap[] monsterMaps; // ���Ͱ� �ִ� �ʵ� 
    public List<List<TileData>> tiles; // ���� �� Ÿ�Ϸ� ���� ������
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }
    /* Property */
    public Tilemap[] MonsterMaps
    {
        get
        {
            return monsterMaps;
        }
    }
    /* Method : DivideMapIntoTiles
     * Description : ���� 2*2 ũ���� Ÿ�Ϸ� ������ ������ �����ϴ� �޼����Դϴ�. �̶� ������ Ÿ���� TileData ��ü�� �ؼ� 2���� ����Ʈ�� �����ϴµ�, �� Ÿ���� ���� ��ǥ�� �߽� ��ġ�� �����߽��ϴ�.
     * �׷��� ���� for���� min������ 1�� ���ϴ� �ڵ尡 �ִ� ���Դϴ�. �߽����� �������� 2�� ���ϴ� ������� �����߽��ϴ�. ��, �ش� ��ǥ�� �Ű������� ������ Ÿ�ϸ��� HasTile�̶� �޼���� �ʿ� Ÿ��
     * �� �ִ����� Ȯ���ϰ� ������ �̵��� �� ���ٴ� ���̹Ƿ� �� Obstacle(��ֹ�) ���θ� true��, �ƴϸ� false�� �����߽��ϴ�.  
     * Parameter : int idx - �� index
     * Return Value : void
     */
    public void DivideMapIntoTiles(int idx)
    {
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min));
        Vector3Int maxTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.max));

        tiles = new List<List<TileData>>();
        for (int i = minTile.y + 1; i < maxTile.y; i += 2)
        {
            tiles.Add(new List<TileData>());
            for (int j = minTile.x + 1; j < maxTile.x; j += 2)
            {
                TileData tileData;
                Vector3 pos = new Vector3(j, i, 0);
                if (monsterMaps[idx].HasTile(monsterMaps[idx].WorldToCell(pos))) tileData = new TileData(false, pos);
                else tileData = new TileData(true, pos);
                tiles[(i - minTile.y - 1) / 2].Add(tileData);
            }
        }
    }
    /* Method : GetCellFromWorld
     * Description : ���� ��ǥ�� �� ��ǥ�� ��ȯ�ϴ� ������ �����ϴ� �޼����Դϴ�. �ش� ���� min��ǥ���� x�� y���� 1�� ���� ���� ¦������ Ȧ������ �ľ��ϰ� �ش��ϴ� ��쿡 �˸°� ����մϴ�. 
     * Parameter : Vector3Int pos - ���� ��ǥ�� int ���·� ��Ÿ�� ��, int idx - �� index
     * Return Value : Tuple<int, int> - Tuple.Itme1 : ��, Tuple.Item2 : ��
     */
    public Tuple<int, int> GetCellFromWorld(Vector3Int pos, int idx)
    {
        Vector3Int minTile = Vector3Int.FloorToInt(monsterMaps[idx].CellToWorld(monsterMaps[idx].cellBounds.min));
        int row, column;
        if (Math.Abs(minTile.x + 1) % 2 == 0)
        {
            column = Math.Abs(pos.x) % 2 != 0 ? (pos.x - minTile.x - 1 + 1) / 2 : (pos.x - minTile.x - 1) / 2;
        }
        else
        {
            column = Math.Abs(pos.x) % 2 == 0 ? (pos.x - minTile.x - 1 + 1) / 2 : (pos.x - minTile.x - 1) / 2;
        }
        if (Math.Abs(minTile.y + 1) % 2 == 0)
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