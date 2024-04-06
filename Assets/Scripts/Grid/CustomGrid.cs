using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{

    public CustomGrid()
    {

    }

    public CustomGrid(Cell[,] newgrid)
    {
        _grid = newgrid;
    }

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

    public bool CheckClickedOnGrid(Vector2 M)
    {
        float halfX = gridData.GridSize.x / 2;
        float halfY = gridData.GridSize.y / 2;

        Vector2 A = new(transform.position.x - halfX, transform.position.y + halfY);
        Vector2 B = new(transform.position.x - halfX, transform.position.y - halfY);
        Vector2 D = new(transform.position.x + halfX, transform.position.y + halfY);

        //Vector2 C = new(transform.position.x + halfX, transform.position.y - halfY);

        Vector2 AB = B - A;
        Vector2 AD = D - A;
        Vector2 AM = M - A;


        float mousePosDotProd = Vector2.Dot(AM, AB);

        float ABDotProd = Vector2.Dot(AB, AB);

        if (0 < mousePosDotProd && mousePosDotProd < ABDotProd)
        {
            mousePosDotProd = Vector2.Dot(AM, AD);
            float ADDotProd = Vector2.Dot(AD, AD);

            if (0 < mousePosDotProd && mousePosDotProd < ADDotProd)
            {
                return true;
            }
        }

        return false;
    }

    public List<Cell> GetEmptyCells()
    {
        List<Cell> emptyCells = new();

        foreach (Cell cell in _grid)
        {
            if (!cell.HasPawnOnIt)
                emptyCells.Add(cell);
        }

        return emptyCells;
    }

}
