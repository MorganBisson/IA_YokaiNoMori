using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvablePawn : Pawn, IEvolvable
{
    public PawnData EvolutionData;

    private bool _hasEvolved = false;
    public bool HasEvolved => _hasEvolved;

    
    private bool _canEvolve = true;
    public bool CanEvolve
    {  
        get { return _canEvolve; }
        set { _canEvolve = value; } 
    }


    public override void OnCapture(Player newOwningPlayer)
    {
        base.OnCapture(newOwningPlayer);

        ResetPawn();
    }

    public override void OnParachute()
    {
        base.OnParachute();

        AvailableDirections = Data.AvailableDirections;
    }


    public void Evolve()
    {
        _hasEvolved = true;
        _spriteRenderer.sprite = EvolutionData.PawnSprite;
        _type = EvolutionData.YokaiType;
        _canEvolve = false;

        AvailableDirections = EvolutionData.AvailableDirections;

        // 6 because we count the value according to the number of point there is a on a card ( 1 point on the base Kodama, 6 on the Kodama Samurai)
        _value *= 6;
    }

    public void ResetPawn()
    {
        if (_hasEvolved == false) return;

        _hasEvolved = false;
        _spriteRenderer.sprite = Data.PawnSprite;
        _type = Data.YokaiType;

        AvailableDirections = Data.AvailableDirections;

        _value /= 6;

        // don't reset CanEvolve because if the pawn is dropped on last grid pos, it can't evolve anymore
    }
}
