using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;


// Insiration
// https://www.youtube.com/watch?v=l-hh51ncgDI&t=569s
// https://github.com/DarshanMaradiya/Unity-3D-AI-Chess/blob/master/Assets/Scripts/ChessAI.cs#L139

public class MinimaxIA : MonoBehaviour
{

    private List<Pawn> _allPawns => BoardManager.Instance.AllPawns;
    private CustomGrid _customGrid => BoardManager.Instance.CustomGrid;

    private Pawn _pawnToMove;
    public Pawn PawnToMove =>_pawnToMove;

    private Cell _bestMoveCell;
    public Cell BestMoveCell => _bestMoveCell;

    private Cell[,] _gridCellCopy;
    private CustomGrid _gridCopy; 



    // Minimax with Alpha Beta implemented 
    // Set maximizing player to false when we call the function (since the AI will always be player 2)
    public int Minimax(int depth, float alpha, float beta, bool maximizingPlayer)
    {
        if (depth == 0 || !GameManager.Instance.IsGameInProgress)
        {
            int value = StaticEvaluationFunction();

            return value;
        }

        

        _customGrid.GridCells.CopyTo(_gridCellCopy, 0);

        _gridCopy = new(_gridCellCopy);


        if (maximizingPlayer) 
        {
 
            // Get les positions disponibles pour le pion séléctionner ?
            // Ou Get toutes les positions disponibles pour chaque pion
            // créer une fonction qui return les directions positions disponibles ?

            //List<Pawn> pawnsCopy = new List<Pawn>(_allPawns);

            int maxEval = int.MinValue;

            foreach (Pawn pawn in _allPawns)
            {

                if (pawn.OwningPlayer.PlayerID != 0) continue;

                List<Cell> possibleMoves = pawn.GetPossibleMoves(_gridCopy);

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
                        baseCell = _customGrid.GridCells[baseCurrentGridPos.x, baseCurrentGridPos.y];
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
                        _pawnToMove = pawn;
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

            foreach (Pawn pawn in _allPawns)
            {

                if (pawn.OwningPlayer.PlayerID == 0) continue;

                List<Cell> possibleMoves = pawn.GetPossibleMoves(_customGrid);

                if (possibleMoves == null) continue;

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
                        baseCell = _customGrid.GridCells[baseCurrentGridPos.x, baseCurrentGridPos.y];
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


                    if (eval <= minEval)
                    {
                        _pawnToMove = pawn;
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


    //private int AlphaBeta(int depth, bool isMax, int alpha, int beta)
    //{
    //    // If max depth is reached or Game is Over
    //    if (depth == 0 || isGameOver())
    //    {
    //        // Static Evaluation Function
    //        int value = StaticEvaluationFunction();

    //        return value;
    //    }

    //    // string ActiveChessmansDetail = "";

    //    // If it is max turn(NPC turn : Black)
    //    if (isMax)
    //    {
    //        int hValue = System.Int32.MinValue;
    //        // int ind = 0;
    //        // Get list of all possible moves with their heuristic value
    //        // For all chessmans
    //        foreach (Chessman chessman in ActiveChessmans.ToArray())
    //        {
    //            // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

    //            if (chessman.isWhite) continue;

    //            bool[,] allowedMoves = chessman.PossibleMoves();

    //            // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

    //            // For all possible moves
    //            for (int x = 0; x < 8; x++)
    //            {
    //                for (int y = 0; y < 8; y++)
    //                {
    //                    if (allowedMoves[x, y])
    //                    {
    //                        // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

    //                        // Critical Section : 
    //                        // 1) Making the current move to see next possible moves after this move in next calls
    //                        Move(chessman, x, y, depth);

    //                        // 2 ) Calculate heuristic value current move
    //                        int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

    //                        // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
    //                        // else detail = detail + "\n";

    //                        // 3 ) Undo the current move to get back the same state that was there before making the current move
    //                        Undo(depth);

    //                        if (hValue < thisMoveValue)
    //                        {
    //                            hValue = thisMoveValue;

    //                            // Remember which move gave the highest hValue
    //                            if (depth == maxDepth - 1)
    //                            {
    //                                NPCSelectedChessman = chessman;
    //                                moveX = x;
    //                                moveY = y;
    //                            }
    //                        }

    //                        if (hValue > alpha)
    //                            alpha = hValue;

    //                        if (beta <= alpha)
    //                            break;
    //                    }
    //                }

    //                if (beta <= alpha)
    //                    break;
    //            }

    //            if (beta <= alpha)
    //                break;
    //        }

    //        // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

    //        return hValue;
    //    }
    //    // If it is min turn(Player turn : White)
    //    else
    //    {
    //        int hValue = System.Int32.MaxValue;
    //        // int ind = 0;

    //        // Get list of all possible moves with their heuristic value
    //        // For all chessmans
    //        foreach (Chessman chessman in ActiveChessmans.ToArray())
    //        {
    //            // ActiveChessmansDetail = ActiveChessmansDetail + "\n(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

    //            if (!chessman.isWhite) continue;

    //            bool[,] allowedMoves = chessman.PossibleMoves();

    //            // if(depth == 2) detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

    //            // For all possible moves
    //            for (int x = 0; x < 8; x++)
    //            {
    //                for (int y = 0; y < 8; y++)
    //                {
    //                    if (allowedMoves[x, y])
    //                    {
    //                        // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving White " + chessman.GetType() + " to (" + x + ", " + y + ")\n");

    //                        // Critical Section : 
    //                        // 1) Making the current move to see next possible moves after this move in next calls
    //                        Move(chessman, x, y, depth);

    //                        // 2 ) Calculate heuristic value current move
    //                        int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

    //                        // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
    //                        // else detail = detail + "\n";

    //                        // 3 ) Undo the current move to get back the same state that was there before making the current move
    //                        Undo(depth);

    //                        if (hValue > thisMoveValue)
    //                        {
    //                            hValue = thisMoveValue;
    //                            // The following 6-7 lines are commented, that is suggesting that 
    //                            // We won't update NPCSelectedChessman, moveX and moveY in min turn
    //                            // if(depth == maxDepth-1)
    //                            // {
    //                            //     NPCSelectedChessman = chessman;
    //                            //     moveX = x;
    //                            //     moveY = y;
    //                            // }
    //                        }

    //                        if (hValue < beta)
    //                            beta = hValue;

    //                        if (beta <= alpha)
    //                            break;
    //                    }
    //                }

    //                if (beta <= alpha)
    //                    break;
    //            }

    //            if (beta <= alpha)
    //                break;
    //        }

    //        // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

    //        return hValue;
    //    }
    //}





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

        foreach (Cell cell in _customGrid.GridCells)
        {
            if (cell.CurrentPawn == null) continue;

            pawnCount++;

            curr = cell.CurrentPawn.Value;

            if (cell.CurrentPawn.OwningPlayer == GameManager.Instance.Players[0])
                TotalScore += curr;
            else
                TotalScore -= curr; 
        }

        Debug.Log("Pawn count:" + pawnCount +" and Total Score = " + TotalScore);
        
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

}

