using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Class : PathFinding
* Description : A* �˰����� �̿��ؼ� ���� ��ġ���� ������������ �ִ� ��θ� ����ϴ� Ŭ�����Դϴ�.
*/
public class PathFinding
{
    List<List<TileData>> tiles; // 2���� Ÿ�� ������ �迭
    List<Vector3> path; // �ִ� ���
    int[] moveX = new int[] { -1, 0, 1, 0}; // �����¿�
    int[] moveY = new int[] { 0, -1, 0, 1};
    int[] move2X = new int[] {-1, -1, 1, 1}; // �밢�� 
    int[] move2Y = new int[] {-1, 1, -1, 1};

    /* Method : SetTiles
     * Description : �� �����ڰ� �̸� �������� 2���� Ÿ�� ������ �迭�� �����ͼ� tiles�� �Ҵ��ϴ� �޼����Դϴ�.
     * Return Value : void
     */
    public void SetTiles
    {
        tiles = MapManager.instance.tiles;

    }
    /* Method : FindPath
     * Description : ��������� ������������ �ִ� ��θ� ã�� ������ �����ϴ� �޼����Դϴ�. ������� �������� ���� ��ǥ�� �� ��ǥ�� ������ ���� �̵��� �� �ִ� Ÿ���� �� ��ǥ ������ ������ openList��
     * �� ��ǥ�� Ÿ���� �湮�ߴ��� ���θ� ��Ÿ���� closedList�� �����մϴ�. �׸��� A* �˰��� ���̴� F, G, H�� �����մϴ�. �̶�, 2���� �迭�� ũ�⸦ �ִ� 100 * 100���� �����߽��ϴ�. �Ƹ� �̺��� ��
     * ũ�Ⱑ ū Ÿ�ϸ��� �������� ���� �� ���Ƽ� �̷��� �����߽��ϴ�. ������� F, G, H ���� ��� 0���� �����ϰ� openList�� ����� �� ��ǥ�� ���� �� A* �˰����� �����մϴ�.
     * �̵� ������ �����¿�, �밢�� ��� �� �ؼ� 8�����Դϴ�. �����¿� �̵� �� G���� 10, �밢�� �̵� �� G���� 14���� �����߽��ϴ�. �׸��� H���� ����ź �Ÿ��� ����߽��ϴ�. 
     * �� 8���⿡ ���� ������ �� �� �ִ� Ÿ���� �ľ��� openList�� �����ϰ�, openList�� F���� �������� �������� �����ؼ� �������� �̵��� Ÿ���� ���� ���� F���� ������ Ÿ���� �ǵ��� �����߽��ϴ�. 
     * ����� �� ������ ���������� ��������� �Ž��� �ö󰡼� path�� ����� �� �ܰ踦 �����մϴ�. �׸��� Reverse�� ������ �迭�� ���� ����� �κ��� �����մϴ�. �ֳ��ϸ� ����� �� ��ǥ�� ����ϴ� ����
     * ���� ����� ���� �� ��ǥ�� Ÿ�� ���� ��ǥ�� �״�� ������ ���� ����� ���� ��ǥ���� ������ ���� ���� ���⿡ ��� �������ϴ� ���°� �� �� �ֱ� �����Դϴ�.
     * Parameter : Vector3Int startPos - �����, Vector3Int endPos - ������, int mapNum - �� ��ȣ
     * Return Value : List<Vector3> - ������������ �ִ� ���
     */
    public List<Vector3> FindPath(Vector3Int startPos, Vector3Int endPos, int mapNum)
    {
        Tuple<int, int> start = MapManager.instance.GetCellFromWorld(startPos, mapNum);
        Tuple<int, int> end = MapManager.instance.GetCellFromWorld(endPos, mapNum);
        path = new List<Vector3>();
        List<Tuple<int, int>> openList = new List<Tuple<int, int>>();
        // �ϴ� ��� �� �Ѵ� �ִ� 100���� ������
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
            if (curX == end.Item1 && curY == end.Item2) break; // ����
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
