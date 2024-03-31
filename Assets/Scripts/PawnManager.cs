using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PawnManager : MonoBehaviour
{




    [SerializeField] private CustomGrid _customGrid;
    [SerializeField] private float _timeBetweenClicks;

    private Pawn _selectedPawn;
    public Pawn SelectedPawn
    {
        get { return _selectedPawn; }
        set { _selectedPawn = value; }
    }


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
            if (GameManager.Instance.IsGameInProgress == true)
            {
                _clickCooldown = Time.time + _timeBetweenClicks;
                OnPlayerClick();
            }
        }
    }


    // This function handle situation when the player click on the custom grid;
    private void OnPlayerClick()
    {

        Cell clickedCell = GetClickedCell();

        HandleParachute(clickedCell);

        if (clickedCell == null) return;

        HandlePawnSelection(clickedCell);

        if (_selectedPawn == null) return;

        HandlePawnMovement(clickedCell);
    }


    #region Handle On Click

    private void HandleParachute(Cell clickedCell)
    {
        if (CheckReservePawnSelected())
        {
            if (clickedCell == null) return;

            if (CanParachute(clickedCell))
            {
                MovePawn(clickedCell);

                if (_selectedPawn.Data.YokaiType == YokaiType.Kodama)
                {
                    CheckCanEvolve(clickedCell);
                }

                GameManager.Instance.CurrentPlayer.OnPawnParachute(_selectedPawn);
                _selectedPawn.OnParachute();

                _selectedPawn = null;

                GameManager.Instance.NextTurn();
            }
        }
    }

    private void HandlePawnMovement(Cell clickedCell)
    {
        if (CanMovePawn(clickedCell))
        {
            GameManager.Instance.AddPawnMove(_selectedPawn, _previousClickedCell.GridPos, clickedCell.GridPos);

            MovePawn(clickedCell);

            if (_selectedPawn.Data.YokaiType == YokaiType.Kodama)
            {
                CheckCanEvolve(clickedCell);
            }

            GameManager.Instance.NextTurn();



            _selectedPawn = null;
            _previousClickedCell = null;
        }
    }

    private void HandlePawnSelection(Cell clickedCell)
    {
        if (CanSelectPawn(clickedCell))
        {
            _selectedPawn = clickedCell.CurrentPawn;
            _previousClickedCell = clickedCell;
            return;
        }
    }

    #endregion



    private bool CheckReservePawnSelected()
    {
        if (_selectedPawn == null) return false;

        if (_selectedPawn.IsInReserve)
        {
            return true;
        }

        return false;
    }

    private bool CanParachute(Cell clickedCell)
    {
        return !clickedCell.HasPawnOnIt;
    }

    private void CheckCanEvolve(Cell cell)
    {
        EvolvablePawn pawn = _selectedPawn.GetComponent<EvolvablePawn>();

        if (pawn.HasEvolved || !pawn.CanEvolve) return;

        
        if (GameManager.Instance.PawnIsOnGridLastRow(cell))
        {
            // Check if the pawn comes from the reserve (which mean he is being parachuted)
            if (pawn.IsInReserve)
            {
                pawn.CanEvolve = false;
                return;
            }

            pawn?.Evolve();
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

                    GameManager.Instance.RemovePawnMove(cell.CurrentPawn);

                    GameManager.Instance.GetOtherPlayer().LosePawn(cell.CurrentPawn);

                }
            }
            return true;
        }
        return false;
    }


    private void MovePawn(Cell newCell)
    {
        _selectedPawn.transform.DOMove(newCell.WorldPos , 0.5f);

        newCell.CurrentPawn = _selectedPawn;
    }



    private bool CheckSelectedPawnDirection(Cell clickedCell)
    {
        Vector2Int direction = new(clickedCell.GridPos.x - _previousClickedCell.GridPos.x, clickedCell.GridPos.y - _previousClickedCell.GridPos.y);

        // return the opposite direction for player 2, who plays on top of the board
        if (GameManager.Instance.CurrentPlayer.PlayerID == 1)
            direction *= -1;

        if (_selectedPawn.AvailableDirections.Contains(direction))
        {
            return true;
        }
        return false;
    }



    private Cell GetClickedCell()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (!_customGrid.CheckClickedOnGrid(mouseWorldPos))
        {
            //if (_selectedPawn != null && !_selectedPawn.IsInReserve)
            //    _selectedPawn = null;

            return null;
        }

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
