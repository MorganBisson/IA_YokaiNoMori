using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private PawnData _data;

    private SpriteRenderer _spriteRenderer;
    private GridManager gridManager;

    void Start()
    {
        InitializePawn();
    }

    private void InitializePawn()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _data.PawnSprite;

        transform.position = _data.SpawnPosition;
    }

    private void OnMouseDown()
    {
        // Vérifiez si c'est le tour du joueur actuel
        if (GameManager.Instance.currentPlayer == GameManager.Player.Player1)
        {
            // Vérifiez si ce Yokai peut être déplacé par le joueur actuel
            if (CanMove())
            {
                GameManager.Instance.NextPlayer();
            }
        }
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
}