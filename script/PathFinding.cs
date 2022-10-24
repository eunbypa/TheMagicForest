using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Class : PathFinding
* Description : A* 알고리즘을 이용해서 현재 위치에서 목적지까지의 최단 경로를 계산하는 클래스입니다.
*/
public class PathFinding
{
    List<List<TileData>> tiles; // 2차원 타일 데이터 배열
    List<Vector3> path; // 최단 경로
    int[] moveX = new int[] { -1, 0, 1, 0}; // 상하좌우
    int[] moveY = new int[] { 0, -1, 0, 1};
    int[] move2X = new int[] {-1, -1, 1, 1}; // 대각선 
    int[] move2Y = new int[] {-1, 1, -1, 1};

    /* Method : SetTiles
     * Description : 맵 관리자가 미리 나눠놓은 2차원 타일 데이터 배열을 가져와서 tiles에 할당하는 메서드입니다.
     * Return Value : void
     */
    public void SetTiles
    {
        tiles = MapManager.instance.tiles;

    }
    /* Method : FindPath
     * Description : 출발지에서 목적지까지의 최단 경로를 찾는 동작을 수행하는 메서드입니다. 출발지와 목적지의 월드 좌표로 셀 좌표를 가져온 다음 이동할 수 있는 타일의 셀 좌표 정보를 가지는 openList와
     * 셀 좌표의 타일을 방문했는지 여부를 나타내는 closedList를 생성합니다. 그리고 A* 알고리즘에 쓰이는 F, G, H를 생성합니다. 이때, 2차원 배열의 크기를 최대 100 * 100으로 제한했습니다. 아마 이보다 더
     * 크기가 큰 타일맵을 만들지는 않을 것 같아서 이렇게 구현했습니다. 출발지의 F, G, H 값을 모두 0으로 세팅하고 openList에 출발지 셀 좌표를 넣은 후 A* 알고리즘을 수행합니다.
     * 이동 방향은 상하좌우, 대각선 모두 다 해서 8방향입니다. 상하좌우 이동 시 G값은 10, 대각선 이동 시 G값은 14으로 설정했습니다. 그리고 H값은 맨하탄 거리를 사용했습니다. 
     * 이 8방향에 대해 다음에 갈 수 있는 타일을 파악해 openList에 저장하고, openList를 F값을 기준으로 오름차순 정렬해서 다음으로 이동할 타일은 가장 작은 F값을 가지는 타일이 되도록 구현했습니다. 
     * 계산이 다 끝나면 목적지부터 출발지까지 거슬러 올라가서 path에 경로의 각 단계를 저장합니다. 그리고 Reverse로 역으로 배열한 다음 출발지 부분은 삭제합니다. 왜냐하면 출발지 셀 좌표를 계산하는 과정
     * 에서 결과로 나온 셀 좌표의 타일 월드 좌표를 그대로 넣으면 기존 출발지 월드 좌표에서 목적지 까지 가는 방향에 잠시 역주행하는 형태가 될 수 있기 때문입니다.
     * Parameter : Vector3Int startPos - 출발지, Vector3Int endPos - 목적지, int mapNum - 맵 번호
     * Return Value : List<Vector3> - 목적지까지의 최단 경로
     */
    public List<Vector3> FindPath(Vector3Int startPos, Vector3Int endPos, int mapNum)
    {
        Tuple<int, int> start = MapManager.instance.GetCellFromWorld(startPos, mapNum);
        Tuple<int, int> end = MapManager.instance.GetCellFromWorld(endPos, mapNum);
        path = new List<Vector3>();
        List<Tuple<int, int>> openList = new List<Tuple<int, int>>();
        // 일단 행과 열 둘다 최대 100으로 설정함
        bool[,] closedList = new bool[100, 100];
        int[,] F = new int[100, 100];
        int[,] G = new int[100, 100];
        int[,] H = new int[100, 100];
        int rLength = tiles.Count;
        int cLength = tiles[0].Count;
        tiles[start.Item2][start.Item1].Parent = tiles[start.Item2][start.Item1];
        F[start.Item2, start.Item1] = 0;
        G[start.Item2, start.Item1] = 0;
        H[start.Item2, start.Item1] = 0;
        openList.Add(new Tuple<int, int>(start.Item1, start.Item2));
        int loop = 0;
        while (openList.Count != 0) {
            int curX = openList[0].Item1;
            int curY = openList[0].Item2;
            openList.RemoveAt(0);
            closedList[curY, curX] = true; 
            if (curX == end.Item1 && curY == end.Item2) break; // 도착
            for (int i = 0; i < 4; i++) 
            {
                int nextX = curX + moveX[i];
                int nextY = curY + moveY[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; 
                if (tiles[nextY][nextX].Obstacle) continue;
                if (closedList[nextY, nextX]) continue;
                int g = tiles[curY][curX].G + 10; 
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; ;
                if (F[nextY, nextX] != 0 && F[nextY, nextX] < g + h) continue;
                G[nextY, nextX] = g;
                H[nextY, nextX] = h;
                F[nextY, nextX] = g + h;
                tiles[nextY][nextX].Parent = tiles[curY][curX];
                openList.Add(new Tuple<int, int>(nextX, nextY));
            }
            for (int i = 0; i < 4; i++)
            {
                int nextX = curX + move2X[i];
                int nextY = curY + move2Y[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; 
                if (tiles[nextY][nextX].Obstacle || tiles[curY][nextX].Obstacle || tiles[nextY][curX].Obstacle) continue; 
                if (closedList[nextY, nextX]) continue; 
                int g = tiles[curY][curX].G + 14; 
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; 
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
                else if (F[x.Item2, x.Item1] == F[y.Item2, y.Item1]) return 0; 
                else return 1;
            });
        }
        path.Add(tiles[end.Item2][end.Item1].WorldPosition);
        TileData tmp = tiles[end.Item2][end.Item1];
        if (tmp.Parent == null) return null;

        while (tmp.Parent != tmp) 
        {
            path.Add(tmp.Parent.WorldPosition);
            tmp = tmp.Parent;
        }
        path.Reverse();
        path.RemoveAt(0);
        return path;
    }
}
