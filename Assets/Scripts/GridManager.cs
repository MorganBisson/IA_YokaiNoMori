using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private Grid _grid;

    [SerializeField] private GameObject _testGO; 


    //public Cell[,] _grid;

    [SerializeField] private int _width;

    [SerializeField] private int _height;

    [SerializeField] private float _cellSize;

    private Transform _camTransform;

    // Start is called before the first frame update
    void Start()
    {
        var GridCenter = _grid.GetCellCenterWorld(new Vector3Int(0, 1));

        Instantiate(_testGO, GridCenter, Quaternion.identity);

        //_camTransform = Camera.main.transform;

        //CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void CreateGrid()
    //{
    //    _grid = new Cell[_width, _height];
    //    //Vector3 WorldBottomLeft = transform.position - Vector3.right * m_gridWorldSize.x / 2 - Vector3.up * m_gridWorldSize.y / 2;


    //    for (int x = 0 ; x < _grid.GetLength(0); x++)
    //    {
    //        for (int y = 0  ; y < _grid.GetLength(1); y++) 
    //        {
    //            var posX = x * (_cellSize / 2);
    //            var posY = y * (_cellSize / 2);

    //            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 100.0f);
    //            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 100.0f);
    //        }
    //    }

    //    _camTransform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, 10);

    //}

    //private Vector3 GetWorldPosition(int x, int y)
    //{
    //    return new Vector3(x, y) * _cellSize;
    //}

    private void OnDrawGizmos()
    {
        
    }
}
