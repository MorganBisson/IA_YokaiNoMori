using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool HasPawnOnIt => CurrentPawn != null;

    public Vector3 WorldPos;

    public Vector2 GridPos;

    public Pawn CurrentPawn;


    public Cell(Vector2 worldPos, Vector2 gridPos)
    {
      
        WorldPos = worldPos;
        CurrentPawn = null;
        GridPos = gridPos;
    }
}
