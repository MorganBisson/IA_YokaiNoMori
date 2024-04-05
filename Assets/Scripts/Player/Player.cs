using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Player
{

    private int _playerID;
    public int PlayerID => _playerID;


    private List<Pawn> _pawnsList = new();
    public List<Pawn> PawnsList => _pawnsList;

    private List<Pawn> _reserveList = new();
    public List<Pawn> _ReserveList => _reserveList;

    private List<Pawn> _allOwnedPawns = new();
    public List<Pawn> AllOwnedPawns => _allOwnedPawns;


    // Pawns the player has on the reserve
    //private Dictionary<YokaiType, Pawn> _pawnsInReserve = new();
    //public Dictionary<YokaiType, Pawn> PawnsInReserve => _pawnsInReserve;

    //private Dictionary<YokaiType, Pawn> _pawns = new();
    //public Dictionary<YokaiType, Pawn> Pawns => _pawns;


    // Player's reserve
    private ReserveManager _reserve = new ReserveManager();
    public ReserveManager Reserve => _reserve;


    public bool HasWon = false;


    public Player(int playerId, ReserveManager reserve)
    {
        _playerID = playerId;
        _reserve = reserve;
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
        //_reserve.AddPawnToReserve(_playerID, newPawn);
        //PawnsInReserve.Add(newPawn.Data.YokaiType, newPawn);

        _reserveList.Add(newPawn);
        _reserve.AddPawnToReserve(_playerID, newPawn);
        _allOwnedPawns.Add(newPawn);
    }

    public void OnPawnParachute(Pawn pawn)
    {
        //_reserve.RemovePawnFromReserve(_playerID, pawn);
        //PawnsInReserve.Remove(pawn.Data.YokaiType);

        _reserveList.Remove(pawn);
        _reserve.RemovePawnFromReserve(_playerID, pawn);

        _pawnsList.Add(pawn);
    }

    public void LosePawn(Pawn pawn)
    {
        _allOwnedPawns.Remove(pawn);
        _pawnsList.Remove(pawn);
    }

    public Pawn FindKoropokkuruInPawnsList()
    {
        for (int i = 0; i < _pawnsList.Count; i++) 
        {
            if (_pawnsList[i].Type == YokaiType.Koropokkuru)
            {
                return _pawnsList[i];
            }
        }

        return null;
    }
}
