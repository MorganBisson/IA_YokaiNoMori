using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public PawnData Data;

    protected SpriteRenderer _spriteRenderer;

    private CustomGrid _grid;

    private Player _owningPlayer;


    public Player OwningPlayer
    {
        get { return _owningPlayer; }
        set { _owningPlayer = value; }
    }
    

    private Vector2 _currentPosition;
    public Vector2 Position => _currentPosition;

    void Start()
    {
        InitializePawn();
    }

    private void InitializePawn()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Data.PawnSprite;

        //transform.position = _data.SpawnPosition;
    }

    private void OnMouseDown()
    {
        // Vérifiez si c'est le tour du joueur actuel
        //if (GameManager.Instance.currentPlayer == GameManager.Player.Player1)
        //{
        //    // Vérifiez si ce Yokai peut être déplacé par le joueur actuel
        //    if (CanMove())
        //    {
        //        GameManager.Instance.NextPlayer();
        //    }
        //}
    }

    private bool CanMove()
    {
        //Vector2Int targetPosition = gridManager.GetSelectedCell();
        //GameObject targetPawn = gridManager.GetPawnAtCell(targetPosition);

        //if (targetPawn != null)
        //{
        //    // Capturer le Yokai Adverse

        //    return true;
        //}

        return true;
    }


    [Tooltip("The direction in which the pawn would like to move")]
    public void Move(Vector2 direction)
    {
        _currentPosition += direction;
    }
}