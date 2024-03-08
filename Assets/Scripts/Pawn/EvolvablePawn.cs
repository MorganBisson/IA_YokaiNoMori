using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvablePawn : Pawn
{

    bool _hasEvolved = false;

    public PawnData EvolutionData;

    public void OnEvolution()
    {
        _hasEvolved = true;
        _spriteRenderer.sprite = EvolutionData.PawnSprite;
    }
}
