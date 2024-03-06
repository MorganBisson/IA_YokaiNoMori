using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{


    public bool HasPawnOnIt;

    public Vector2 WorldPos;

    public Cell(bool hasPawnOnIt, Vector2 worldPos)
    {
        HasPawnOnIt = hasPawnOnIt;
        WorldPos = worldPos;
    }
}
