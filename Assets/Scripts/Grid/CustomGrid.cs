using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{

    public GridData gridData;

    private Cell[,] _grid;


    int _gridSizeX;
    int _gridSizeY;


    private void Start()
    {

        _gridSizeX = Mathf.RoundToInt(gridData.GridSize.x / gridData.CellSize);
        _gridSizeY = Mathf.RoundToInt(gridData.GridSize.y / gridData.CellSize);

        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Cell[_gridSizeX, _gridSizeY];

        Vector3 WorldBottomLeft = transform.position - Vector3.right * gridData.GridSize.x / 2 - Vector3.up * gridData.GridSize.y / 2;


        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {

                Vector2 WorldPoint = WorldBottomLeft + Vector3.right * (x * gridData.CellSize + gridData.CellRadius) + Vector3.up * (y * gridData.CellSize + gridData.CellRadius);

                //bool walkable = !(Physics2D.OverlapCircle(WorldPoint, m_nodeRadius));

                _grid[x, y] = new Cell(false, WorldPoint, new Vector2(x, y));

                print(_grid[x, y].WorldPos);
            }
        }
    }


    //public Cell CellFromWorldPoint(Vector2 worldPos)
    //{
    //    float percentX = (worldPos.x / gridData.GridSize.x / 2) / gridData.GridSize.x;
    //    float percentY = (worldPos.y / gridData.GridSize.y / 2) / gridData.GridSize.y;

    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    int x = Mathf.FloorToInt(Mathf.Clamp((_gridSizeX) * percentX, 0, _gridSizeX - 1));
    //    int y = Mathf.FloorToInt(Mathf.Clamp((_gridSizeY) * percentY, 0, _gridSizeY - 1));


    //    //Mathf.FloorToInt(Mathf.Min(gridSizeX * percentX, gridSizeX - 1))

    //    return _grid[x, y];
    //}


    public Cell CellFromWorldPoint(Vector3 worldPos)
    {
        if (_grid == null)
            return null;

        Vector2Int coords = GridPointFromWorldPos(worldPos);
        return _grid[coords.x, coords.y];
    }

    public Vector2Int GridPointFromWorldPos(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridData.GridSize.x / 2) / gridData.GridSize.x;
        float percentY = (worldPos.y + gridData.GridSize.y / 2) / gridData.GridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return new Vector2Int(x, y);
    }


    private void OnDrawGizmos()
    {
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

}
