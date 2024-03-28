using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReserveClick : MonoBehaviour
{

    [SerializeField] private PawnManager _pawnManager;


    private void Start()
    {
        _pawnManager = GameObject.Find("PawnManager").GetComponent<PawnManager>();

        _selfPawn = GetComponent<Pawn>();
    }

    private Pawn _selfPawn;
    private void OnMouseDown()
    {
       
        if (GameManager.Instance.CurrentPlayer != _selfPawn.OwningPlayer) return;

        _pawnManager.SelectedPawn = _selfPawn;

        Debug.Log(_pawnManager.SelectedPawn.name);
    }
}
