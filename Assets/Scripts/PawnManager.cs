using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PawnManager : MonoBehaviour
{

    [SerializeField] private CustomGrid _customGrid;
    [SerializeField] private float _timeBetweenClicks;

    private Pawn _selectedPawn;
    public Pawn SelectedPawn => _selectedPawn;

    private Camera _camera;

    private Cell _previousClickedCell;

    private float _clickCooldown; 


    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _clickCooldown <= Time.time)
        {
            if (GameManager.Instance.IsGameInProgress == false)
            {
                _clickCooldown = Time.time + _timeBetweenClicks;
                Debug.Log("Clique");
                OnPlayerClick();
            }
        }
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

        if (_selectedPawn == null) return;


        if (CanMovePawn(clickedCell))
        {
            MovePawn(clickedCell);
            GameManager.Instance.NextPlayer();
            _selectedPawn = null;
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
        if (CheckSelectedPawnDirection(cell))
        {
            if (cell.HasPawnOnIt)
            {  
                if (CanCapturePawn(cell))
                {
                    GameManager.Instance.CurrentPlayer.CapturePawn(cell.CurrentPawn);

                    GameManager.Instance.GetOtherPlayer().LosePawn(cell.CurrentPawn);
                }
            }
            return true;
        }
        return false;
    }


    private void MovePawn(Cell newCell)
    {
        _previousClickedCell.CurrentPawn = null;

        _selectedPawn.transform.position = newCell.WorldPos;

        newCell.CurrentPawn = _selectedPawn;
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

        //Debug.Log("grid pos x : " + cell.GridPos.x);
        //Debug.Log("grid pos y : " + cell.GridPos.y);

        return cell;
    }


    private bool CanCapturePawn(Cell cell)
    {
        if (cell.HasPawnOnIt)
        {
            if (cell.CurrentPawn.OwningPlayer != GameManager.Instance.CurrentPlayer)
            {
                return true;
            }
        }
        return false;
    }


}
