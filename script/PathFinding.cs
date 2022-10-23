using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    List<List<TileData>> tiles;
    List<Vector3> path;
    int[] moveX = new int[] { -1, 0, 1, 0}; // 상하좌우
    int[] moveY = new int[] { 0, -1, 0, 1};
    int[] move2X = new int[] {-1, -1, 1, 1}; // 대각선 
    int[] move2Y = new int[] {-1, 1, -1, 1};
    public void SetTiles(int mapNum)
    {
        tiles = MapManager.instance.tiles;

    }
    public List<Vector3> FindPath(Vector3Int startPos, Vector3Int endPos, int mapNum) // start 는 몬스터의 포지션, end는 플레이어의 포지션
    {
        if (!MapManager.instance.MonsterMaps[mapNum].HasTile(MapManager.instance.MonsterMaps[mapNum].WorldToCell(new Vector3(startPos.x, startPos.y, 0)))) return null;
        if (!MapManager.instance.MonsterMaps[mapNum].HasTile(MapManager.instance.MonsterMaps[mapNum].WorldToCell(new Vector3(endPos.x, endPos.y, 0)))) return null;
        Tuple<int, int> start = MapManager.instance.GetCellFromWorld(startPos, mapNum);
        Tuple<int, int> end = MapManager.instance.GetCellFromWorld(endPos, mapNum);
        path = new List<Vector3>();
        List<Tuple<int, int>> openList = new List<Tuple<int, int>>(); // 현재 이동할 수 있는 타일들 index 쌍(x, y)
        // 일단 행과 열 둘다 최대 100으로 설정함
        bool[,] closedList = new bool[100, 100]; // true면 방문할 수 없는 타일을 의미
        int[,] F = new int[100, 100];
        int[,] G = new int[100, 100];
        int[,] H = new int[100, 100];
        int rLength = tiles.Count; // 행 길이
        int cLength = tiles[0].Count; // 열 길이
        tiles[start.Item2][start.Item1].Parent = tiles[start.Item2][start.Item1]; // 시작 타일의 부모는 자기 자신
        F[start.Item2, start.Item1] = 0;
        G[start.Item2, start.Item1] = 0;
        H[start.Item2, start.Item1] = 0;
        openList.Add(new Tuple<int, int>(start.Item1, start.Item2));
        int loop = 0;
        while (openList.Count != 0) {
            int curX = openList[0].Item1;
            int curY = openList[0].Item2;
            openList.RemoveAt(0);
            closedList[curY, curX] = true; // 방문
            if (curX == end.Item1 && curY == end.Item2) break; // 도착
            for (int i = 0; i < 4; i++) //상하좌우
            {
                int nextX = curX + moveX[i];
                int nextY = curY + moveY[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; // 범위 밖
                if (tiles[nextY][nextX].Obstacle) continue; // 벽인 경우
                if (closedList[nextY, nextX]) continue; // 이미 방문한 타일
                int g = tiles[curY][curX].G + 10; // 상하좌우 이동은 비용이 10
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; ; // 휴리스틱 맨하탄 거리 이용
                if (F[nextY, nextX] != 0 && F[nextY, nextX] < g + h) continue;
                G[nextY, nextX] = g;
                H[nextY, nextX] = h;
                F[nextY, nextX] = g + h;
                tiles[nextY][nextX].Parent = tiles[curY][curX];
                openList.Add(new Tuple<int, int>(nextX, nextY));
            }
            for (int i = 0; i < 4; i++) // 대각선
            {
                int nextX = curX + move2X[i];
                int nextY = curY + move2Y[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; // 범위 밖
                if (tiles[nextY][nextX].Obstacle || tiles[curY][nextX].Obstacle || tiles[nextY][curX].Obstacle) continue; // 대각선으로 이동할 때는 이동방향을 기준으로 도착지, X방향, Y방향 3 타일 모두 장애물이 아닌지 검사
                if (closedList[nextY, nextX]) continue; // 이미 방문한 타일
                int g = tiles[curY][curX].G + 14; // 대각선 이동은 비용이 14
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; // 휴리스틱 맨하탄 거리 이용
                if (F[nextY, nextX] != 0 && F[nextY, nextX] < g + h) continue;
                G[nextY, nextX] = g;
                H[nextY, nextX] = h;
                F[nextY, nextX] = g + h;
                tiles[nextY][nextX].Parent = tiles[curY][curX];
                openList.Add(new Tuple<int, int>(nextX, nextY));
            }
            openList.Sort((x, y) =>
            {
                if (F[x.Item2, x.Item1] < F[y.Item2, y.Item1]) return -1;
                else if (F[x.Item2, x.Item1] == F[y.Item2, y.Item1]) return 0; // 최소 비용을 가지는 타일을 맨 앞에 배치시키기 위해 휴리스틱 f를 기준으로 오름차순 정렬
                else return 1;
            });
        }
        path.Add(tiles[end.Item2][end.Item1].WorldPosition);
        TileData tmp = tiles[end.Item2][end.Item1]; // 도착 타일부터 시작 타일까지 다녀간 경로를 거슬러 올라감
        if (tmp.Parent == null) return null;

        while (tmp.Parent != tmp) // 시작점인지 체크 
        {
            path.Add(tmp.Parent.WorldPosition);
            tmp = tmp.Parent;
        }
        path.Reverse();
        path.RemoveAt(0); // 시작점은 경로에서 삭제(이유 : GetCellFromWorld로 시작점 셀 좌표를 정하는 과정에서 역주행이 발생할 수 있기 때문)
        return path;
    }
}
