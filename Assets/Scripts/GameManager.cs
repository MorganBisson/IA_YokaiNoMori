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


    [Header("Starting pawns")]
    [Tooltip("The pawns which every players start with")]
    [SerializeField]
    private List<Pawn> _startPawns;
    public List<Pawn> StartPawns => _startPawns;

    private List<Player> _players = new List<Player>();

    private Player _currentPlayer;
    public Player CurrentPlayer => _currentPlayer;

    [SerializeField] private CustomGrid _grid;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        InitPlayers();

        // Joueur 1 en premier
        _currentPlayer = _players[0];
    }

    public void NextPlayer()
    {
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
            Player newPlayer = new Player(i, new List<Pawn>(_startPawns));
            _players.Add(newPlayer);
        }
    }

    private void InitPlayerPawn()
    {

       
    }


}
