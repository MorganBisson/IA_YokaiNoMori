using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Grid Data")]
public class GridData : ScriptableObject
{


    [Header("Grid")]
    [SerializeField] private Vector2 _gridSize;
    public Vector2 GridSize => _gridSize;


    [Header("Cell")]
    [SerializeField] private float _cellSize;
    public float CellSize => _cellSize;

    [HideInInspector]
    public float CellRadius => _cellSize / 2;


}
