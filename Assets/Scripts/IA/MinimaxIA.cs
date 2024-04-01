using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


// Insiration
// https://www.youtube.com/watch?v=l-hh51ncgDI&t=569s
// https://github.com/DarshanMaradiya/Unity-3D-AI-Chess/blob/master/Assets/Scripts/ChessAI.cs#L139

public class MinimaxIA : MonoBehaviour
{

    // Minimax with Alpha Beta implemented 
    int Minimax(Vector2Int position, int depth, float alpha, float beta, bool maximizingPlayer)
    {
        if (depth == 0 || !GameManager.Instance.IsGameInProgress)
        {
            int value = StaticEvaluationFunction();

            return value;
        }
            
        
        if (maximizingPlayer) 
        {
            float maxEval = Mathf.NegativeInfinity;

            // Get les positions disponibles pour le pion séléctionner ?
            // Ou Get toutes les positions disponibles pour chaque pion
            // créer une fonction qui return les directions positions disponibles ? 
            foreach ()
            {
                var eval = Minimax(child, depth - 1, false);

            }
        }



        return hValue;



    }



    private int AlphaBeta(int depth, bool isMax, int alpha, int beta)
    {
        // If max depth is reached or Game is Over
        if (depth == 0 || isGameOver())
        {
            // Static Evaluation Function
            int value = StaticEvaluationFunction();

            return value;
        }

        // string ActiveChessmansDetail = "";

        // If it is max turn(NPC turn : Black)
        if (isMax)
        {
            int hValue = System.Int32.MinValue;
            // int ind = 0;
            // Get list of all possible moves with their heuristic value
            // For all chessmans
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                if (chessman.isWhite) continue;

                bool[,] allowedMoves = chessman.PossibleMoves();

                // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

                            // Critical Section : 
                            // 1) Making the current move to see next possible moves after this move in next calls
                            Move(chessman, x, y, depth);

                            // 2 ) Calculate heuristic value current move
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 3 ) Undo the current move to get back the same state that was there before making the current move
                            Undo(depth);

                            if (hValue < thisMoveValue)
                            {
                                hValue = thisMoveValue;

                                // Remember which move gave the highest hValue
                                if (depth == maxDepth - 1)
                                {
                                    NPCSelectedChessman = chessman;
                                    moveX = x;
                                    moveY = y;
                                }
                            }

                            if (hValue > alpha)
                                alpha = hValue;

                            if (beta <= alpha)
                                break;
                        }
                    }

                    if (beta <= alpha)
                        break;
                }

                if (beta <= alpha)
                    break;
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }
        // If it is min turn(Player turn : White)
        else
        {
            int hValue = System.Int32.MaxValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "\n(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                if (!chessman.isWhite) continue;

                bool[,] allowedMoves = chessman.PossibleMoves();

                // if(depth == 2) detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving White " + chessman.GetType() + " to (" + x + ", " + y + ")\n");

                            // Critical Section : 
                            // 1) Making the current move to see next possible moves after this move in next calls
                            Move(chessman, x, y, depth);

                            // 2 ) Calculate heuristic value current move
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 3 ) Undo the current move to get back the same state that was there before making the current move
                            Undo(depth);

                            if (hValue > thisMoveValue)
                            {
                                hValue = thisMoveValue;
                                // The following 6-7 lines are commented, that is suggesting that 
                                // We won't update NPCSelectedChessman, moveX and moveY in min turn
                                // if(depth == maxDepth-1)
                                // {
                                //     NPCSelectedChessman = chessman;
                                //     moveX = x;
                                //     moveY = y;
                                // }
                            }

                            if (hValue < beta)
                                beta = hValue;

                            if (beta <= alpha)
                                break;
                        }
                    }

                    if (beta <= alpha)
                        break;
                }

                if (beta <= alpha)
                    break;
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }
    }



    private int StaticEvaluationFunction()
    {
        int TotalScore = 0;
        int curr = 0;
        foreach (Pawn pawn in ActiveChessmans)
        {
            if (pawn.GetType() == typeof(King))
                curr = 900;
            if (pawn.GetType() == typeof(Queen))
                curr = 90;
            if (pawn.GetType() == typeof(Rook))
                curr = 50;
            if (pawn.GetType() == typeof(Bishup))
                curr = 30;
            if (pawn.GetType() == typeof(Knight))
                curr = 30;
            if (pawn.GetType() == typeof(Pawn))
                curr = 10;

            if (pawn.OwningPlayer == GameManager.Instance.Players[0])
                TotalScore -= curr;
            else
                TotalScore += curr;
        }
        return TotalScore;
    }

}
