using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public PawnData Data;

    protected SpriteRenderer _spriteRenderer;

    private Player _owningPlayer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Player OwningPlayer
    {
        get { return _owningPlayer; }
        set { _owningPlayer = value; }
    }

    public void InitializePawn()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Data.PawnSprite;

        //transform.position = _data.SpawnPosition;
    }
}