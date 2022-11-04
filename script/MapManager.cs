using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Class : MapManager
 * Description : 몬스터가 있는 맵을 설정한 크기의 타일로 나누고 그렇게 생성된 2차원 타일 배열을 바탕으로 해서 월드 좌표로 셀 좌표를 계산하는 작업을 하는 맵 관리자 클래스입니다. 
 * 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받습니다.
 */
public class MapManager : MonoBehaviour
{
    public static MapManager instance; // 싱글톤 패턴
    [SerializeField] private Tilemap[] monsterMaps; // 몬스터가 있는 맵들 
    public List<List<TileData>> tiles; // 맵을 각 타일로 나눈 데이터
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
     * Description : 맵을 2*2 크기의 타일로 나누는 동작을 수행하는 메서드입니다. 이때 각각의 타일을 TileData 객체로 해서 2차원 리스트에 저장하는데, 각 타일의 월드 좌표는 중심 위치로 설정했습니다.
     * 그래서 이중 for문에 min값에서 1씩 더하는 코드가 있는 것입니다. 중심점을 기준으로 2씩 더하는 방식으로 구현했습니다. 또, 해당 좌표를 매개변수로 전달해 타일맵의 HasTile이란 메서드로 맵에 타일
     * 이 있는지를 확인하고 없으면 이동할 수 없다는 뜻이므로 즉 Obstacle(장애물) 여부를 true로, 아니면 false로 설정했습니다.  
     * Parameter : int idx - 맵 index
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
     * Description : 월드 좌표를 셀 좌표로 변환하는 동작을 수행하는 메서드입니다. 해당 맵의 min좌표에서 x와 y값에 1을 더한 다음 짝수인지 홀수인지 파악하고 해당하는 경우에 알맞게 계산합니다. 
     * Parameter : Vector3Int pos - 월드 좌표를 int 형태로 나타낸 값, int idx - 맵 index
     * Return Value : Tuple<int, int> - Tuple.Itme1 : 열, Tuple.Item2 : 행
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