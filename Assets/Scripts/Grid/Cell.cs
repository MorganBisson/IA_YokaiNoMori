using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{


    public bool HasPawnOnIt;

    public Vector2 WorldPos;

    public Vector2 GridPos;

    public Pawn CurrentPawn;

    public Cell(bool hasPawnOnIt, Vector2 worldPos, Vector2 gridPos)
    {
        HasPawnOnIt = hasPawnOnIt;
        WorldPos = worldPos;
        CurrentPawn = null;
        GridPos = gridPos;
    }
}
