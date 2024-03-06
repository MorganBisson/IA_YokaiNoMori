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

    public enum Player { Player1, Player2 };

    public Player currentPlayer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        // Joueur 1 en premier
        currentPlayer = Player.Player1;
    }

    void Update()
    {

    }

    public void NextPlayer()
    {
        // Passez au joueur suivant
        if (currentPlayer == Player.Player1)
            currentPlayer = Player.Player2;
        else
            currentPlayer = Player.Player1;
    }
}
