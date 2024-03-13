using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [SerializeField]
    private Transform listPawns;

    public List<Pawn> StartPawns => _startPawns;

    private List<Player> _players = new List<Player>();
    public List<Player> Players => _players;


    private Player _currentPlayer;
    public Player CurrentPlayer => _currentPlayer;

    private bool _isGameInProgress = false;

    public bool IsGameInProgress => _isGameInProgress;

    [SerializeField] private ReserveManager reservePlayer1;
    [SerializeField] private ReserveManager reservePlayer2;


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
    }

    public void NextPlayer()
    {
        // Vérifier si un joueur a gagné ou si il y a match nul avant de passer au prochain tour
        //
        if (CheckHasWon())
        {
            _currentPlayer.HasWon = true;
            EndGame();
            return;
        }
            
        // Passez au joueur suivant
        if (_currentPlayer == _players[0])
            _currentPlayer = _players[1];
        else
            _currentPlayer = _players[0];

        CheckHasWon();
    }


    private void InitPlayers()
    {
        for (int i = 0;  i < 2; i++) 
        {
            Dictionary<YokaiType, Pawn> dict = new();

            Player newPlayer = new Player(i, dict, i == 1 ? reservePlayer1 : reservePlayer2);
            //newPlayer.InitPawns();

            _players.Add(newPlayer);
            newPlayer.InitPawns();

        }
        // Joueur 1 en premier
        _currentPlayer = _players[0];
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

                Pawn newPawn = Instantiate(pawn, spawnCell.WorldPos, spawnRotation, listPawns);
                newPawn.OwningPlayer = player;

                spawnCell.CurrentPawn = newPawn;

                player.Pawns.Add(newPawn.Data.YokaiType, newPawn);
            }
        }
       
    }

    public Player GetOtherPlayer()
    {
        if (_currentPlayer == _players[0])
            return _players[1];
        else
            return _players[0];

        
    }

    public bool CheckHasWon()
    {
        if (_currentPlayer.PawnsInReserve.ContainsKey(YokaiType.Koropokkuru))
        {
            return true;
        }

        Pawn koropokkuru = _currentPlayer.Pawns[YokaiType.Koropokkuru];
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

    private bool PawnIsOnGridLastRow(Cell pawnCell)
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
        _isGameInProgress = true;
        Debug.Log("Player " + _currentPlayer.PlayerID + " has won !");
    }

    //private bool CheckForDraw()
    //{

    //}

}
