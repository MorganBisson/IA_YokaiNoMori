using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField]
    private bool _debugGrid;

    public GridData gridData;

    private Cell[,] _grid;

    public Cell[,] GridCells => _grid;


    int _gridSizeX;
    int _gridSizeY;


    public void CreateGrid()
    {
        _gridSizeX = Mathf.RoundToInt(gridData.GridSize.x / gridData.CellSize);
        _gridSizeY = Mathf.RoundToInt(gridData.GridSize.y / gridData.CellSize);

        _grid = new Cell[_gridSizeX, _gridSizeY];

        Vector3 WorldBottomLeft = transform.position - Vector3.right * gridData.GridSize.x / 2 - Vector3.up * gridData.GridSize.y / 2;


        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {

                Vector2 WorldPoint = WorldBottomLeft + Vector3.right * (x * gridData.CellSize + gridData.CellRadius) + Vector3.up * (y * gridData.CellSize + gridData.CellRadius);

                //bool walkable = !(Physics2D.OverlapCircle(WorldPoint, m_nodeRadius));

                _grid[x, y] = new Cell(WorldPoint, new Vector2Int(x, y));

                if (_debugGrid)
                    print(_grid[x, y].WorldPos);
            }
        }
    }


    public Cell CellFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridData.GridSize.x / 2) / gridData.GridSize.x;
        float percentY = (worldPos.y + gridData.GridSize.y / 2) / gridData.GridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }


    private void OnDrawGizmos()
    {

        if (_debugGrid == false) return;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridData.GridSize.x, gridData.GridSize.y, -1));
        Gizmos.color = Color.red;

        if (_grid != null)
        {
            foreach (Cell cell in _grid)
            {
                Gizmos.color = (cell.HasPawnOnIt) ? Color.white : Color.red;

                Gizmos.DrawCube(cell.WorldPos, Vector2.one * (gridData.CellSize - .1f));
            }
        }
    }

    public List<Cell> GetNeighbours(Cell cell)
    {
        List<Cell> neighbours = new List<Cell>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = (int)cell.GridPos.x + x;
                int checkY = (int)cell.GridPos.y + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

}
