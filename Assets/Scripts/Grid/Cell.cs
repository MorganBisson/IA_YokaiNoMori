using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool HasPawnOnIt => CurrentPawn != null;

    public Vector3 WorldPos;

    public Vector2Int GridPos;

    public Pawn CurrentPawn;


    public Cell(Vector2 worldPos, Vector2Int gridPos)
    {
        WorldPos = worldPos;
        
        GridPos = gridPos;

        CurrentPawn = null;
    }
}
