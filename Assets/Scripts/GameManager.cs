using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public class PawnMove
    {
        public PawnMove(Pawn currentPawn, Vector2Int oldPosition, Vector2Int newPosition)
        {
            pawn = currentPawn;
            oldPos = oldPosition;
            currentPos = newPosition;
        }

        public Pawn pawn;
        public Vector2Int currentPos;
        public Vector2Int oldPos;
    }

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }


    [SerializeField] private CustomGrid _grid;
    [SerializeField] private PawnManager _pawnManager;

    [Header("PAWN START")]
    [Tooltip("The pawns which every players start with")]

    [SerializeField]
    private List<Pawn> _startPawns;
    public List<Pawn> StartPawns => _startPawns;

    private Pawn[] _allPawns;
    public Pawn[] AllPawns => _allPawns;

    [SerializeField]
    private Transform listPawnsTransform;
    public Transform ListPawnTransform => listPawnsTransform;



    private List<Player> _players = new();
    public List<Player> Players => _players;



    private Player _currentPlayer;
    public Player CurrentPlayer => _currentPlayer;



    private bool _isGameInProgress = false;
    public bool IsGameInProgress => _isGameInProgress;

    [SerializeField] private ReserveManager reservePlayer1;
    [SerializeField] private ReserveManager reservePlayer2;

    [SerializeField] private TMP_Text txtPlayerTurn;


    private List<PawnMove> _lastMoves = new();
    public List<PawnMove> LastMoves => _lastMoves;

    private int _turnCount;
    List<int> _repetitionTurns = new();

    private bool _drawGame = false;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        _grid.CreateGrid();

        InitPlayers();

        SpawnPlayerPawns();

        _isGameInProgress = true;
    }

    public void NextTurn()
    {

        if (CheckHasWon() || CheckDraw())
        {
            EndGame();
            return;
        }

        NextPlayer();

        txtPlayerTurn.text = _currentPlayer.PlayerID == 0 ? "Player One" : "Player Two";
        txtPlayerTurn.transform.DOShakePosition(1f, 10f, 10);

        // We need to check after switching player to check if the player has won before playing its move
        CheckHasWon();

        _turnCount++;
    }

    private void NextPlayer()
    {
        // Passez au joueur suivant
        if (_currentPlayer == _players[0])
        {
            _currentPlayer = _players[1];
        }
        else
        {
            _currentPlayer = _players[0];

        }
    }

    private void InitPlayers()
    {
        for (int i = 0;  i < 2; i++) 
        {
            Player newPlayer = new(i, i == 1 ? reservePlayer1 : reservePlayer2);
            //newPlayer.InitPawns();

            _players.Add(newPlayer);

        }

        // Joueur 1 en premier
        _currentPlayer = _players[0];

        txtPlayerTurn.text = "Player One";
        txtPlayerTurn.transform.DOShakePosition(1f, 10f, 10);
    }

    private void SpawnPlayerPawns()
    {
        foreach (Player player in _players)
        {
            List<Pawn> copyList = new List<Pawn>(_startPawns);

            foreach (Pawn pawn in copyList)
            {
                PawnData.SpawnPosition spawnPosData = pawn.Data.PawnSpawnPosition;
                Quaternion spawnRotation;
                Cell spawnCell;
    

                if (player.PlayerID == 0)
                {
                    spawnCell = _grid.GridCells[spawnPosData.Player1.x, spawnPosData.Player1.y];
                    spawnRotation = Quaternion.identity;
                } 
                else
                {
                    spawnCell = _grid.GridCells[spawnPosData.Player2.x, spawnPosData.Player2.y];
                    spawnRotation = Quaternion.Euler(180, 0, 0);
                }

                Pawn newPawn = Instantiate(pawn, spawnCell.WorldPos, spawnRotation, listPawnsTransform);
                newPawn.OwningPlayer = player;

                spawnCell.CurrentPawn = newPawn;

                player.PawnsList.Add(newPawn);
                
                AllPawns.Append(newPawn);
            }

            player.InitPawns();
        }
       
    }

    public Player GetOtherPlayer()
    {
        if (_currentPlayer == _players[0])
            return _players[1];
        else
            return _players[0];
    }


    


    #region Draw Functions
    public bool CheckDraw()
    {

        if (_lastMoves.Count >= 4)
        {
            if (!CheckSamePawn()) return false;
        }


        if (_repetitionTurns.Count > 0)
        {
            if (_turnCount == _repetitionTurns.Last() + 4)
            {
                if (CheckForRepetition())
                {
                    if (_repetitionTurns.Count >= 3)
                    {
                        _drawGame = true;

                        return _drawGame;
                    }
                }
                else
                {
                    _repetitionTurns.Clear();
                }
            }
        }
        else
        {
            CheckForRepetition();
        }

        return _drawGame;
    }

    private bool CheckForRepetition()
    {
        if (_lastMoves.Count < 4 && _repetitionTurns.Count > 0)
        {
            _repetitionTurns.Clear();
            return false;
        }
           

        int rep = 0;

        // We only need to check the the two first index of the list
        for (int i = 0; i < _lastMoves.Count - 2; i++)
        { 
            if (_lastMoves[i].oldPos == _lastMoves[i + 2].currentPos)
            {
                rep++;
            }
            else
            {
                if (_repetitionTurns.Count > 0)
                    _repetitionTurns.Clear();
            }
        }

        if (rep == 2)
        {
            _repetitionTurns.Add(_turnCount);
            return true;
        }

        return false;
    }

    private bool CheckSamePawn()
    {
        int i = 0;

        if (_currentPlayer == _players[1])
            i = 1;

        if (_lastMoves[i].pawn == _lastMoves[i + 2].pawn)
            return true;

        _repetitionTurns.Clear();

        return false;
    }


    public PawnMove FindPawnMove(Pawn pawn)
    {
        for (int i = 0; i < _lastMoves.Count; i++)
        {
            if (_lastMoves[i].pawn == pawn)
                return _lastMoves[i];
        }

        return null;
    }

    public void RemovePawnMove(Pawn pawn)
    {
        PawnMove pawnMove = FindPawnMove(pawn);

        if (pawnMove == null) return;

        _lastMoves.Remove(pawnMove);

    }

    public void AddPawnMove(Pawn pawn, Vector2Int oldPos, Vector2Int newPos)
    {
        PawnMove pawnMove = new(pawn, oldPos, newPos);

        _lastMoves.Add(pawnMove);

        if (_lastMoves.Count > 4)
        {
            _lastMoves.RemoveAt(0);
        }
    }

    #endregion



    public bool CheckHasWon()
    {
        if (_currentPlayer._ReserveList.Count > 0)
        {
            Pawn lastCapturedPawn = _currentPlayer._ReserveList.Last();

            if (lastCapturedPawn.Data.YokaiType == YokaiType.Koropokkuru)
            {
                return true;
            }
        }
        

        Pawn koropokkuru = _currentPlayer.FindKoropokkuruInPawnsList();
        Cell pawnCell = _grid.CellFromWorldPoint(koropokkuru.transform.position);

        if (PawnIsOnGridLastRow(pawnCell))
        {
            if (koropokkuru != null && koropokkuru == _pawnManager.SelectedPawn)
            {
                // if neighbour can't capture, player has won so we need to return the opposite value
                return !CheckNeighboursCanCapture(pawnCell);
            }
        }

        return false;
    }

    

    public bool PawnIsOnGridLastRow(Cell pawnCell)
    {
        Vector2Int[] lastRow;

        // Player 1 must reach the player's 2 last row, and vice versa
        if (_currentPlayer == _players[0])
            lastRow = _grid.gridData.lastRow.Player2;
        else
            lastRow = _grid.gridData.lastRow.Player1;


        if (lastRow.Contains(pawnCell.GridPos))
        {
            return true;
        }

        return false;
    }

    private bool CheckNeighboursCanCapture(Cell cell)
    {
        
        List<Cell> neighbours = _grid.GetNeighbours(cell);

        foreach (Cell neighbour in neighbours)
        {

            if (neighbour.HasPawnOnIt == false) continue;

            if (neighbour.CurrentPawn.OwningPlayer == _currentPlayer) continue;

            foreach (Vector2 dir in neighbour.CurrentPawn.Data.AvailableDirections)
            {
                if (neighbour.GridPos + dir == cell.GridPos) 
                    return true;
            }
        }

        return false;
    }

    private void EndGame()
    {
        _isGameInProgress = false;

        if (_drawGame)
        {
            Debug.Log("Draw");
        }
        else
        {
            _currentPlayer.HasWon = true;
            GameWinner.isWin = true;
            GameWinner.WinPlayer = _currentPlayer;
            SceneManager.LoadScene(3);
        }
        

        
    }

}
