using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    List<List<TileData>> tiles;
    List<Vector3> path;
    int[] moveX = new int[] { -1, 0, 1, 0}; // �����¿�
    int[] moveY = new int[] { 0, -1, 0, 1};
    int[] move2X = new int[] {-1, -1, 1, 1}; // �밢�� 
    int[] move2Y = new int[] {-1, 1, -1, 1};
    public void SetTiles(int mapNum)
    {
        tiles = MapManager.instance.tiles;

    }
    public List<Vector3> FindPath(Vector3Int startPos, Vector3Int endPos, int mapNum) // start �� ������ ������, end�� �÷��̾��� ������
    {
        if (!MapManager.instance.MonsterMaps[mapNum].HasTile(MapManager.instance.MonsterMaps[mapNum].WorldToCell(new Vector3(startPos.x, startPos.y, 0)))) return null;
        if (!MapManager.instance.MonsterMaps[mapNum].HasTile(MapManager.instance.MonsterMaps[mapNum].WorldToCell(new Vector3(endPos.x, endPos.y, 0)))) return null;
        Tuple<int, int> start = MapManager.instance.GetCellFromWorld(startPos, mapNum);
        Tuple<int, int> end = MapManager.instance.GetCellFromWorld(endPos, mapNum);
        path = new List<Vector3>();
        List<Tuple<int, int>> openList = new List<Tuple<int, int>>(); // ���� �̵��� �� �ִ� Ÿ�ϵ� index ��(x, y)
        // �ϴ� ��� �� �Ѵ� �ִ� 100���� ������
        bool[,] closedList = new bool[100, 100]; // true�� �湮�� �� ���� Ÿ���� �ǹ�
        int[,] F = new int[100, 100];
        int[,] G = new int[100, 100];
        int[,] H = new int[100, 100];
        int rLength = tiles.Count; // �� ����
        int cLength = tiles[0].Count; // �� ����
        tiles[start.Item2][start.Item1].Parent = tiles[start.Item2][start.Item1]; // ���� Ÿ���� �θ�� �ڱ� �ڽ�
        F[start.Item2, start.Item1] = 0;
        G[start.Item2, start.Item1] = 0;
        H[start.Item2, start.Item1] = 0;
        openList.Add(new Tuple<int, int>(start.Item1, start.Item2));
        int loop = 0;
        while (openList.Count != 0) {
            int curX = openList[0].Item1;
            int curY = openList[0].Item2;
            openList.RemoveAt(0);
            closedList[curY, curX] = true; // �湮
            if (curX == end.Item1 && curY == end.Item2) break; // ����
            for (int i = 0; i < 4; i++) //�����¿�
            {
                int nextX = curX + moveX[i];
                int nextY = curY + moveY[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; // ���� ��
                if (tiles[nextY][nextX].Obstacle) continue; // ���� ���
                if (closedList[nextY, nextX]) continue; // �̹� �湮�� Ÿ��
                int g = tiles[curY][curX].G + 10; // �����¿� �̵��� ����� 10
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; ; // �޸���ƽ ����ź �Ÿ� �̿�
                if (F[nextY, nextX] != 0 && F[nextY, nextX] < g + h) continue;
                G[nextY, nextX] = g;
                H[nextY, nextX] = h;
                F[nextY, nextX] = g + h;
                tiles[nextY][nextX].Parent = tiles[curY][curX];
                openList.Add(new Tuple<int, int>(nextX, nextY));
            }
            for (int i = 0; i < 4; i++) // �밢��
            {
                int nextX = curX + move2X[i];
                int nextY = curY + move2Y[i];
                if (nextX < 0 || nextY < 0 || nextX >= cLength || nextY >= rLength) continue; // ���� ��
                if (tiles[nextY][nextX].Obstacle || tiles[curY][nextX].Obstacle || tiles[nextY][curX].Obstacle) continue; // �밢������ �̵��� ���� �̵������� �������� ������, X����, Y���� 3 Ÿ�� ��� ��ֹ��� �ƴ��� �˻�
                if (closedList[nextY, nextX]) continue; // �̹� �湮�� Ÿ��
                int g = tiles[curY][curX].G + 14; // �밢�� �̵��� ����� 14
                int h = (Math.Abs(nextX - end.Item1) + Math.Abs(nextY - end.Item2)) * 10; // �޸���ƽ ����ź �Ÿ� �̿�
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
                else if (F[x.Item2, x.Item1] == F[y.Item2, y.Item1]) return 0; // �ּ� ����� ������ Ÿ���� �� �տ� ��ġ��Ű�� ���� �޸���ƽ f�� �������� �������� ����
                else return 1;
            });
        }
        path.Add(tiles[end.Item2][end.Item1].WorldPosition);
        TileData tmp = tiles[end.Item2][end.Item1]; // ���� Ÿ�Ϻ��� ���� Ÿ�ϱ��� �ٳణ ��θ� �Ž��� �ö�
        if (tmp.Parent == null) return null;

        while (tmp.Parent != tmp) // ���������� üũ 
        {
            path.Add(tmp.Parent.WorldPosition);
            tmp = tmp.Parent;
        }
        path.Reverse();
        path.RemoveAt(0); // �������� ��ο��� ����(���� : GetCellFromWorld�� ������ �� ��ǥ�� ���ϴ� �������� �������� �߻��� �� �ֱ� ����)
        return path;
    }
}
