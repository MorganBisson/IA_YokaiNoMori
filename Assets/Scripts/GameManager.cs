using System.Collections;
using System.Collections.Generic;
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

        // Passez au joueur suivant
        if (_currentPlayer == _players[0])
            _currentPlayer = _players[1];
        else
            _currentPlayer = _players[0];
    }


    private void InitPlayers()
    {
        for (int i = 0;  i < 2; i++) 
        {
            Player newPlayer = new Player(i, new List<Pawn>(_startPawns), i == 1 ? reservePlayer1 : reservePlayer2);
            //newPlayer.InitPawns();

            _players.Add(newPlayer);

        }
        // Joueur 1 en premier
        _currentPlayer = _players[0];
    }

    private void SpawnPlayerPawns()
    {
        foreach (Player player in _players)
        {
            foreach (Pawn pawn in player.Pawns)
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

    //private bool CheckHasWon()
    //{
        
    //}

    //private bool CheckForDraw()
    //{

    //}

}
