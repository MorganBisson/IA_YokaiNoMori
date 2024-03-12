using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PawnManager : MonoBehaviour
{

    [SerializeField]
    private CustomGrid _customGrid;


    private Pawn _selectedPawn;

    Camera _camera;

    private Cell _previousClickedCell;


    private void Awake()
    {
        _camera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPlayerClick();
        }
    }

    private bool CanSelectPawn(Cell cell)
    {
        if (cell.HasPawnOnIt)
        {
            if (cell.CurrentPawn.OwningPlayer == GameManager.Instance.CurrentPlayer)
            {
                if (_selectedPawn == null || _selectedPawn.OwningPlayer == cell.CurrentPawn.OwningPlayer)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanMovePawn(Cell cell)
    {
        if (cell.HasPawnOnIt)
        {
            if (cell.CurrentPawn.OwningPlayer != GameManager.Instance.CurrentPlayer && _selectedPawn != null)
            {
                if (CheckSelectedPawnDirection(cell))
                    return true;
            }
            
        }
        else
        {
            if (CheckSelectedPawnDirection(cell))
                return true;
        }

        

        return false;
    }


    private void MovePawn(Cell newCell)
    {
        _previousClickedCell.CurrentPawn = null;

        _selectedPawn.transform.position = newCell.WorldPos;

        newCell.CurrentPawn = _selectedPawn;

        //if (CanCapturePawn(newCell))
        //{

        //    GameManager.Instance.CurrentPlayer.Reserve.Add(newCell.CurrentPawn);

        //    GameManager.Instance.CurrentPlayer.


        //    newCell.CurrentPawn
        //}

    }


    private void OnPlayerClick()
    {
        Cell clickedCell = GetClickedCell();
        if (clickedCell == null) return;


        if (CanSelectPawn(clickedCell))
        {
            _selectedPawn = clickedCell.CurrentPawn;
            _previousClickedCell = clickedCell;
            return;
        }

        if (CanMovePawn(clickedCell))
        {
            MovePawn(clickedCell);
        }
    }



    private bool CheckSelectedPawnDirection(Cell clickedCell)
    {
        Vector2Int direction = new Vector2Int((int)(clickedCell.GridPos.x - _previousClickedCell.GridPos.x), (int)(clickedCell.GridPos.y - _previousClickedCell.GridPos.y));

        if (_selectedPawn.Data.AvailableDirections.Contains(direction))
        {
            return true;
        }
        return false;
    }



    private Cell GetClickedCell()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

        Cell cell = _customGrid.CellFromWorldPoint(mouseWorldPos);

        Debug.Log("grid pos x : " + cell.GridPos.x);
        Debug.Log("grid pos y : " + cell.GridPos.y);

        return cell;
    }


    private bool CanCapturePawn(Cell cell)
    {
        if (cell.HasPawnOnIt)
        {
            if (cell.CurrentPawn.OwningPlayer != GameManager.Instance.CurrentPlayer && _selectedPawn != null)
            {
                return true;
            }
        }
        return false;
    }


}
