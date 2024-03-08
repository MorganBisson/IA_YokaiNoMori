using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnManager : MonoBehaviour
{

    [SerializeField]
    private CustomGrid _customGrid;


    private Pawn _selectedPawn;

    Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckCell();
        }
    }

    private void CheckCell()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log("Click : ");
        Debug.Log(mouseWorldPos.x);
        Debug.Log(mouseWorldPos.y);

        Cell cell = _customGrid.CellFromWorldPoint(mouseWorldPos);

        Debug.Log("grid pos x : " + cell.GridPos.x);
        Debug.Log("grid pos y : " + cell.GridPos.y);

        if (cell.CurrentPawn == null) return;

        if (cell.CurrentPawn == _selectedPawn) return;
        
        if (cell.CurrentPawn != null)
            _selectedPawn = cell.CurrentPawn;

        
    }

}
