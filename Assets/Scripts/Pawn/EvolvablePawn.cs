using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvablePawn : Pawn, IEvolvable
{

    bool _hasEvolved = false;
    public bool HasEvolved => _hasEvolved;

    public PawnData EvolutionData;

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
