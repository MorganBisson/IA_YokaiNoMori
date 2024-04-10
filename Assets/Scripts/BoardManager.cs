using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    private static BoardManager _instance;

    public static BoardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BoardManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private MinimaxIA _minimaxIA;
    public int IADepth;

    [SerializeField] private CustomGrid _customGrid;
    public CustomGrid CustomGrid => _customGrid;

    [SerializeField] private float _timeBetweenClicks;
    [SerializeField] private Transform _listPawnsTransform;

    [Header("PAWN START")]
    [Tooltip("The pawns which every players start with")]
    [SerializeField] private List<Pawn> _startPawns;
    public List<Pawn> StartPawns => _startPawns;

    [SerializeField] float _timeBeforeAILaunch = 0.5f;

    public Transform ListPawnsTransform => _listPawnsTransform;

    public GameObject directionIndicatorPrefab;
    public RectTransform directionIndicatorParent;

    private Pawn _selectedPawn;
    public Pawn SelectedPawn
    {
        get { return _selectedPawn; }
        set { _selectedPawn = value; }
    }


    private List<Pawn> _allPawns = new();
    public List<Pawn> AllPawns => _allPawns;



    private Camera _camera;

    private Cell _previousClickedCell;

    private float _clickCooldown; 


    


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _camera = Camera.main;
    }

    private void Start()
    {
        if(_minimaxIA != null) IADepth = PlayerPrefs.GetInt("Difficulty");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _clickCooldown <= Time.time)
        {
            if (GameManager.Instance.CurrentPlayer.PlayerID == 1 && GameManager.Instance.CurrentGameMode == GameMode.AI) return; 


            if (GameManager.Instance.IsGameInProgress == true)
            {
                _clickCooldown = Time.time + _timeBetweenClicks;
                OnPlayerClick();
            }
        }
    }




    public void SpawnPawns(Player player)
    {
        List<Pawn> copyList = new List<Pawn>(_startPawns);

        foreach (Pawn pawn in copyList)
        {
            PawnData.SpawnPosition spawnPosData = pawn.Data.PawnSpawnPosition;
            Quaternion spawnRotation;
            Cell spawnCell;


            if (player.PlayerID == 0)
            {
                spawnCell = _customGrid.GridCells[spawnPosData.Player1.x, spawnPosData.Player1.y];
                spawnRotation = Quaternion.identity;
            }
            else
            {
                spawnCell = _customGrid.GridCells[spawnPosData.Player2.x, spawnPosData.Player2.y];
                spawnRotation = Quaternion.Euler(180, 0, 0);
            }


            Pawn newPawn = Instantiate(pawn, spawnCell.WorldPos, spawnRotation, _listPawnsTransform);
            newPawn.OwningPlayer = player;
            newPawn.CurrentGridPos = spawnCell.GridPos;
            newPawn.InitializePawn();


            spawnCell.CurrentPawn = newPawn;

            player.PawnsList.Add(newPawn);

            _allPawns.Add(newPawn);

        }
    }



    // This function handle situation when the player click on the custom grid;
    private void OnPlayerClick()
    {       
        Cell clickedCell = GetClickedCell();
        
        // If it returns true, it means the player has parachuted a pawn, so we don't need to go further
        if(HandleParachute(clickedCell)) return;

        if (clickedCell == null) return;

        // If it returns true, it means the player has selected a pawn, so we don't need to go further
        if (HandlePawnSelection(clickedCell)) return;

        if (_selectedPawn == null) return;

        // If it returns true, it means the player has moved the selected pawn, so we don't need to go further
        if (HandlePawnMovement(clickedCell)) return;

    }

    public IEnumerator LaunchAI()
    {
        yield return new WaitForSeconds(_timeBeforeAILaunch);

        _minimaxIA.CopyCustomGrid();

        _minimaxIA.Minimax(IADepth, Mathf.NegativeInfinity, Mathf.Infinity, false);


        _selectedPawn = _customGrid.GridCells[_minimaxIA.PawnToMoveGridPos.x, _minimaxIA.PawnToMoveGridPos.y].CurrentPawn;
        Cell moveToCell = _customGrid.GridCells[_minimaxIA.BestMoveCell.GridPos.x, _minimaxIA.BestMoveCell.GridPos.y];

        if (HandleParachute(moveToCell))
        {
            StopCoroutine(LaunchAI());
            yield return null;
        }

        if (moveToCell == null)
        {
            StopCoroutine(LaunchAI());
            yield return null;
        }

        if (_selectedPawn == null)
        {
            StopCoroutine(LaunchAI());
            yield return null;
        }
        // If it returns true, it means the player has moved the selected pawn, so we don't need to go further

        Cell previousPawnPos = _customGrid.GridCells[_selectedPawn.CurrentGridPos.x, _selectedPawn.CurrentGridPos.y];
        HandleAIPawnMovement(moveToCell, previousPawnPos);

        _minimaxIA.DestroyGridCopy();
        yield return null;
    }


    #region Handle On Click

    private bool HandleParachute(Cell clickedCell)
    {
        if (CheckReservePawnSelected())
        {
            if (clickedCell == null) return false;

            if (CanParachute(clickedCell))
            {
                MovePawn(clickedCell);

                if (_selectedPawn.Type == YokaiType.Kodama)
                {
                    CheckCanEvolve(clickedCell);
                }

                GameManager.Instance.CurrentPlayer.OnPawnParachute(_selectedPawn);
                _selectedPawn.OnParachute();

                _selectedPawn = null;

                GameManager.Instance.NextTurn();

                return true;
            }
        }
        return false;
    }

    private bool HandlePawnMovement(Cell clickedCell)
    {
        if (CanMovePawn(clickedCell))
        {
            ClearDirectionIndicators();

            GameManager.Instance.AddPawnMove(_selectedPawn, _previousClickedCell.GridPos, clickedCell.GridPos);

            MovePawn(clickedCell);

            if (_selectedPawn.Type == YokaiType.Kodama)
            {
                CheckCanEvolve(clickedCell);
            }

            GameManager.Instance.NextTurn();

            _selectedPawn = null;
            _previousClickedCell = null;

            return true;
        }

        return false;
    }

    private void HandleAIPawnMovement(Cell selectedCell, Cell previousPawnCell)
    {


        HandleCapturePawnAI(selectedCell);

        GameManager.Instance.AddPawnMove(_selectedPawn, previousPawnCell.GridPos, selectedCell.GridPos);

        previousPawnCell.CurrentPawn = null;
        MovePawn(selectedCell);

        if (_selectedPawn.Type == YokaiType.Kodama)
        {
            CheckCanEvolve(selectedCell);
        }

        GameManager.Instance.NextTurn();

        _selectedPawn = null;
        
    }

    private bool HandlePawnSelection(Cell clickedCell)
    {
        if (CanSelectPawn(clickedCell))
        {
            _selectedPawn = clickedCell.CurrentPawn;
            ShowAvailableDirections(_selectedPawn);
            _previousClickedCell = clickedCell;
            return true;
        }

        return false;
    }

    #endregion

    private void ShowAvailableDirections(Pawn selectedPawn)
    {
        // Efface les indicateurs précédents
        ClearDirectionIndicators();

        Vector2Int[] availableDirections = selectedPawn.AvailableDirections;

        foreach (Vector2Int direction in availableDirections)
        {
            Vector3 indicatorPosition = selectedPawn.transform.position + new Vector3(direction.x, direction.y, 0) * 1.82f;

            if (_customGrid.CheckClickedOnGrid(indicatorPosition))
            {
                GameObject directionIndicator = Instantiate(directionIndicatorPrefab, directionIndicatorParent);
                RectTransform indicatorRectTransform = directionIndicator.GetComponent<RectTransform>();
                indicatorRectTransform.position = indicatorPosition;
            }
        }
    }

    private void ClearDirectionIndicators()
    {
        foreach (Transform child in directionIndicatorParent)
        {
            Destroy(child.gameObject);
        }
    }

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
            
            if (CanCapturePawn(cell))
            {
                   
                GameManager.Instance.CurrentPlayer.CapturePawn(cell.CurrentPawn);

                GameManager.Instance.RemovePawnMove(cell.CurrentPawn);

                GameManager.Instance.GetOtherPlayer().LosePawn(cell.CurrentPawn);

                cell.CurrentPawn = null;
            }
            return true;
        }
        return false;
    }

    private void HandleCapturePawnAI(Cell cell)
    {
        if (CanCapturePawn(cell))
        {

            GameManager.Instance.CurrentPlayer.CapturePawn(cell.CurrentPawn);

            GameManager.Instance.RemovePawnMove(cell.CurrentPawn);

            GameManager.Instance.GetOtherPlayer().LosePawn(cell.CurrentPawn);

            cell.CurrentPawn = null;
        }
        
    }


    private void MovePawn(Cell newCell)
    {
        _selectedPawn.transform.DOMove(newCell.WorldPos , 0.5f);

        _selectedPawn.CurrentGridPos = newCell.GridPos;
        newCell.CurrentPawn = _selectedPawn;

        if (_previousClickedCell != null)
            _previousClickedCell.CurrentPawn = null;
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
