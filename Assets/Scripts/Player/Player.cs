using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Player
{

    private int _playerID;
    public int PlayerID => _playerID;


    // Pawns the player has on the reserve
    private Dictionary<YokaiType, Pawn> _pawnsInReserve = new();
    public Dictionary<YokaiType, Pawn> PawnsInReserve => _pawnsInReserve;

    private Dictionary<YokaiType, Pawn> _pawns = new();
    public Dictionary<YokaiType, Pawn> Pawns => _pawns;


    // Player's reserve
    private ReserveManager _reserve = new ReserveManager();
    public ReserveManager Reserve => _reserve;


    public bool HasWon = false;


    public Player(int playerId, Dictionary<YokaiType, Pawn> pawns, ReserveManager reserve)
    {
        _playerID = playerId;
        _pawns = pawns;
        _reserve = reserve;
    }

    public void InitPawns()
    {
        foreach (KeyValuePair<YokaiType, Pawn> pawn in _pawns) 
        { 
            pawn.Value.OwningPlayer = this;
            pawn.Value.InitializePawn();
        }
    }

    public bool CanPlay()
    {
        return GameManager.Instance.CurrentPlayer == this;
    }

    public void CapturePawn(Pawn pawn)
    {
        AddPawnToReserve(pawn);

        pawn.OnCapture(this);
    }

    public void AddPawnToReserve(Pawn newPawn)
    {
        _reserve.AddPawnToReserve(_playerID, newPawn);
        PawnsInReserve.Add(newPawn.Data.YokaiType, newPawn);
    }

    public void OnPawnParachute(Pawn pawn)
    {
        _reserve.RemovePawnFromReserve(_playerID, pawn);
        PawnsInReserve.Remove(pawn.Data.YokaiType);
    }

    public void LosePawn(Pawn pawn)
    {
        _pawns.Remove(pawn.Data.YokaiType); 
    }
}
