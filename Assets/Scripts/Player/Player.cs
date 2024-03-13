using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Player
{

    private int _playerID;
    public int PlayerID => _playerID;


    // Pawns the player has on the table
    private List<Pawn> _pawns = new List<Pawn>();
    public List<Pawn> Pawns => _pawns;

    // Pawns the player has on the reserve
    private List<Pawn> _pawnsInReserve = new List<Pawn>();
    public List<Pawn> PawnsInReserve => _pawnsInReserve;

    // Player's reserve
    private ReserveManager _reserve = new ReserveManager();
    public ReserveManager Reserve => _reserve;


    public bool IsPlayerTurn = false;


    public Player(int playerId, List<Pawn> pawns, ReserveManager reserve)
    {
        _playerID = playerId;
        _pawns = pawns;
        _reserve = reserve;
    }

    public void InitPawns()
    {
        foreach (Pawn pawn in _pawns)
        {
            pawn.OwningPlayer = this;
        }
    }

    public bool CanPlay()
    {
        return GameManager.Instance.CurrentPlayer == this;
    }

    public void CapturePawn(Pawn pawn)
    {
        AddPawnToReserve(pawn);

        IEvolvable evolvablePawn = pawn.GetComponent<IEvolvable>();

        evolvablePawn?.ResetPawn();
    }

    public void AddPawnToReserve(Pawn newPawn)
    {
        _reserve.AddPawnToReserve(_playerID, newPawn);
        PawnsInReserve.Add(newPawn);
    }

    public void LosePawn(Pawn pawn)
    {
        _pawns.Remove(pawn);
    }
}
