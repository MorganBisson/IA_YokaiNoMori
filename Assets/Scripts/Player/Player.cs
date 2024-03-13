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


    // Player's reserve
    private List<Pawn> _reserve = new List<Pawn>();
    public List <Pawn> Reserve => _reserve;


    public bool IsPlayerTurn = false;


    public Player(int playerId, List<Pawn> pawns)
    {
        _playerID = playerId;
        _pawns = pawns;
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
        _reserve.Add(pawn);
        
        IEvolvable evolvablePawn = pawn.GetComponent<IEvolvable>();

        evolvablePawn?.ResetPawn();

    }
    
    public void LosePawn(Pawn pawn)
    {
        _pawns.Remove(pawn);
    }

}
