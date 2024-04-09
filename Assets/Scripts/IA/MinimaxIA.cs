using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;


// Insiration
// https://www.youtube.com/watch?v=l-hh51ncgDI&t=569s
// https://github.com/DarshanMaradiya/Unity-3D-AI-Chess/blob/master/Assets/Scripts/ChessAI.cs#L139

public class MinimaxIA : MonoBehaviour
{


    [SerializeField] private Transform _listCopyPawnsTransform;


    private List<Pawn> _allPawns => BoardManager.Instance.AllPawns;
    private CustomGrid _customGrid => BoardManager.Instance.CustomGrid;

    private Vector2Int _pawnToMoveGridPos;
    public Vector2Int PawnToMoveGridPos => _pawnToMoveGridPos;

    private Cell _bestMoveCell;
    public Cell BestMoveCell => _bestMoveCell;

    private Cell[,] _gridCellCopy;
    private CustomGrid _gridCopy;


    private List<Pawn> _copiedPawn = new();





    // Minimax with Alpha Beta implemented 
    // Set maximizing player to false when we call the function (since the AI will always be player 2)
    public int Minimax(int depth, float alpha, float beta, bool maximizingPlayer)
    {
        if (depth == 0 || !GameManager.Instance.IsGameInProgress)
        {
            int value = StaticEvaluationFunction();

            return value;
        }



        if (maximizingPlayer) 
        {
 
            // Get les positions disponibles pour le pion séléctionner ?
            // Ou Get toutes les positions disponibles pour chaque pion
            // créer une fonction qui return les directions positions disponibles ?

            //List<Pawn> pawnsCopy = new List<Pawn>(_allPawns);

            int maxEval = int.MinValue;

            foreach (Pawn pawn in _copiedPawn)
            {

                if (pawn.OwningPlayer.PlayerID != 0) continue;

                List<Cell> possibleMoves = pawn.GetPossibleMoves(_gridCopy);

                if (possibleMoves.Count == 0) continue;

                foreach (Cell cell in possibleMoves)
                {

                    Vector2Int baseCurrentGridPos;
                    Cell baseCell;

                    if (pawn.IsInReserve)
                    {
                        baseCurrentGridPos = new Vector2Int(-1, -1);
                        baseCell = null;
                        
                    }
                    else
                    {
                        baseCurrentGridPos = pawn.CurrentGridPos;
                        baseCell = _gridCopy.GridCells[baseCurrentGridPos.x, baseCurrentGridPos.y];
                    }
                    

                    // Making the move so we can simulate what will happen next
                    if (baseCell != null)
                        baseCell.CurrentPawn = null;
                    pawn.CurrentGridPos = cell.GridPos;
                    cell.CurrentPawn = pawn;



                    var eval = Minimax(depth - 1, Mathf.NegativeInfinity, Mathf.Infinity, false);



                    // Reseting the board's cells and pawn to their previous state
                    if (baseCell != null)
                        baseCell.CurrentPawn = pawn;
                    pawn.CurrentGridPos = baseCurrentGridPos;
                    cell.CurrentPawn = null;


                    if (eval > maxEval)
                    {
                        _pawnToMoveGridPos = pawn.CurrentGridPos;
                        _bestMoveCell = cell;
                    }

                    maxEval = Mathf.Max(maxEval, eval);
                    beta = Mathf.Max(beta, eval);

                    if (beta <= alpha)
                        break;

                }
            }

            return maxEval;


        }
        else
        {
            //List<Pawn> pawnsCopy = new List<Pawn>(_allPawns);

            int minEval = int.MaxValue;

            foreach (Pawn pawn in _copiedPawn)
            {

                if (pawn.OwningPlayer.PlayerID == 0) continue;

                List<Cell> possibleMoves = pawn.GetPossibleMoves(_gridCopy);

                if (possibleMoves.Count == 0) continue;

                foreach (Cell cell in possibleMoves)
                {
                    Vector2Int baseCurrentGridPos;
                    Cell baseCell;

                    if (pawn.IsInReserve)
                    {
                        baseCurrentGridPos = new Vector2Int(-1, -1);
                        baseCell = null;

                    }
                    else
                    {
                        baseCurrentGridPos = pawn.CurrentGridPos;
                        baseCell = _gridCopy.GridCells[baseCurrentGridPos.x, baseCurrentGridPos.y];
                    }


                    // Making the move so we can simulate what will happen next
                    if (baseCell != null)
                        baseCell.CurrentPawn = null;
                    pawn.CurrentGridPos = cell.GridPos;
                    cell.CurrentPawn = pawn;


                    var eval = Minimax(depth - 1, Mathf.NegativeInfinity, Mathf.Infinity, false);



                    // Reseting the board's cells and pawn to their previous state
                    if (baseCell != null)
                        baseCell.CurrentPawn = pawn;
                    pawn.CurrentGridPos = baseCurrentGridPos;
                    cell.CurrentPawn = null;


                    if (eval < minEval)
                    {
                        _pawnToMoveGridPos = pawn.CurrentGridPos;
                        _bestMoveCell = cell;
                    }

                    minEval = Mathf.Min(minEval, eval);
                    beta = Mathf.Min(beta, eval);

                    if (beta <= alpha)
                        break;

                    //int nextCellValue = Minimax(pawn.CurrentGridPos, depth - 1, Mathf.NegativeInfinity, Mathf.Infinity, false);
                }
            }

            return minEval;
        }
    }


    private int StaticEvaluationFunction()
    {
        int TotalScore = 0;
        int curr;
        //List<Pawn> canReachPawn = new();

        //foreach (Pawn pawn in _allPawns)
        //{
        //    // Select only the pawns which can reach the currentPos;
        //    var direction = currentPos - pawn.CurrentGridPos;
        //    if (pawn.OwningPlayer.PlayerID == 1)
        //        direction *= -1;

        //    if (pawn.AvailableDirections.Contains(direction))
        //        canReachPawn.Add(pawn);
        //}
        int pawnCount = 0; 

        foreach (Cell cell in _gridCopy.GridCells)
        {
            if (cell.CurrentPawn == null) continue;

            pawnCount++;

            curr = cell.CurrentPawn.Value;

            if (cell.CurrentPawn.OwningPlayer == GameManager.Instance.Players[0])
                TotalScore += curr;
            else
                TotalScore -= curr; 
        }

        //Debug.Log("Pawn count:" + pawnCount +" and Total Score = " + TotalScore);
        
        //foreach (Pawn pawn in canReachPawn)
        //{



        //    Debug.Log("------------------------");
        //    curr = pawn.Value;
        //    Cell currentCell = _customGrid.GridCells[currentPos.x, currentPos.y];


        //    // List<Cell> neighbours = _customGrid.GetNeighbours(currentCell);
        //    List<Cell> possibleMoves = pawn.GetPossibleMovesFromCell(_customGrid, currentCell);

        //    if (possibleMoves.Count == 0) break;

        //    foreach (Cell newCell in possibleMoves)
        //    {
        //        if (newCell.HasPawnOnIt)
        //        {
        //            // if player 1, total score value is positive
        //            if (newCell.CurrentPawn.OwningPlayer == GameManager.Instance.Players[0])
        //                TotalScore += curr;
        //            else
        //                TotalScore -= curr;


        //            Debug.Log("Pawn: " + pawn.name);
        //            Debug.Log("Total score:" + TotalScore);

        //        }
        //    }


        //}
        return TotalScore;
    }



    public void CopyCustomGrid()
    {
        int gridLengthX = _customGrid.GridCells.GetLength(0);
        int gridLengthY = _customGrid.GridCells.GetLength(1);

        _gridCellCopy = new Cell[gridLengthX, gridLengthY];

        for (int i = 0; i < gridLengthX; i++)
        {
            for (int j = 0; j < gridLengthY; j++)
            {
                Cell copiedCell = CopyCell(_customGrid.GridCells[i, j]);

                if (_customGrid.GridCells[i, j].HasPawnOnIt)
                {
                    copiedCell.CurrentPawn = _customGrid.GridCells[i, j].CurrentPawn.Clone(_listCopyPawnsTransform);
                    _copiedPawn.Add(copiedCell.CurrentPawn);
                }

                _gridCellCopy[i, j] = copiedCell;
            }
        }

        _gridCopy = Instantiate(_customGrid);
        _gridCopy.GridCells = _gridCellCopy;



    }

    public void DestroyGridCopy()
    {

        for (int i = 0; i < _listCopyPawnsTransform.childCount; i++)
        {
            Destroy(_listCopyPawnsTransform.GetChild(i).gameObject);
        }

        _gridCellCopy = null;
        Destroy(_gridCopy.gameObject);
    }


    private Cell CopyCell(Cell cellToCopy)
    {
        return new Cell(cellToCopy.WorldPos, cellToCopy.GridPos);
    }


}

