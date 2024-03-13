using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvablePawn : Pawn, IEvolvable
{
    public PawnData EvolutionData;


    private bool _hasEvolved = false;
    public bool HasEvolved => _hasEvolved;

    
    private bool _canEvolve = true;
    public bool CanEvolve => _canEvolve;

    


    public void OnEvolution()
    {
        _hasEvolved = true;
        _spriteRenderer.sprite = EvolutionData.PawnSprite;
    }

    public void ResetPawn()
    {
        if (_hasEvolved == false) return;

        _hasEvolved = false;
        _spriteRenderer.sprite = Data.PawnSprite;
    }


}
