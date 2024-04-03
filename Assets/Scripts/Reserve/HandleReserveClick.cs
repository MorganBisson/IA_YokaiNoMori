using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReserveClick : MonoBehaviour
{

    private BoardManager _boardManager;


    private void Start()
    {
        _boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();

        _selfPawn = GetComponent<Pawn>();
    }

    private Pawn _selfPawn;
    private void OnMouseDown()
    {
       
        if (GameManager.Instance.CurrentPlayer != _selfPawn.OwningPlayer) return;

        _boardManager.SelectedPawn = _selfPawn;

        Debug.Log(_boardManager.SelectedPawn.name);
    }
}
