using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Grid Data")]
public class GridData : ScriptableObject
{

    [System.Serializable]
    public struct EvolvableCell
    {
        public Vector2Int[] Player1;
        public Vector2Int[] Player2;
    }

    [Header("Grid")]
    [SerializeField] private Vector2 _gridSize;
    public Vector2 GridSize => _gridSize;


    [Header("Cell")]
    [SerializeField] private float _cellSize;
    public float CellSize => _cellSize;

    [HideInInspector]
    public float CellRadius => _cellSize / 2;


    public EvolvableCell EvolvableCells;


}
