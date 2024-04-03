using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pawn : MonoBehaviour
{
    public PawnData Data;

    private Vector2Int _currentGridPos;
    public Vector2Int CurrentGridPos
    {
        get { return _currentGridPos; }
        set { _currentGridPos = value; }
    }


    private Vector2Int[] _availableDirection; 

    public Vector2Int[] AvailableDirections
    {
        get { return _availableDirection; } 
        set { _availableDirection = value; }
    }

    protected SpriteRenderer _spriteRenderer;
    protected BoxCollider2D _boxCollider;
    protected HandleReserveClick _handleReserveClick;

    private Player _owningPlayer;

    private bool _isInReserve;

    public bool IsInReserve
    {  
        get { return _isInReserve; }
        set { _isInReserve = value; } 
    }


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _handleReserveClick = GetComponent<HandleReserveClick>();

        DisableReserveComponents();
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

        _availableDirection = Data.AvailableDirections;
    }

    public virtual void OnCapture(Player newOwningPlayer)
    {
        EnableReserveComponents();

        _isInReserve = true;

        _owningPlayer = newOwningPlayer;

        if (_owningPlayer.PlayerID == 0)
        { 
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(180, 0, 0);
        }
    }

    public virtual void OnParachute()
    {
        DisableReserveComponents();
        _isInReserve = false;
    }


    public List<Vector2Int> GetPossibleMoves(CustomGrid customGrid)
    {
        return new();
    }





    // Enable / Disable Components needed for the reserve
    private void EnableReserveComponents()
    {
        _boxCollider.enabled = true;
        _handleReserveClick.enabled = true;
    }

    private void DisableReserveComponents()
    {
        _boxCollider.enabled = false;
        _handleReserveClick.enabled = false;
    }

}