using System;
using UnityEngine;
using System.Collections.Generic;

namespace Chess.Scripts.Core
{
    public class ChessPlayerPlacementHandler : MonoBehaviour
    {
        [SerializeField] public int row, column;

        private void Start()
        {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
        }

        private void OnMouseDown()
        {
            HighlightValidMoves();
        }

        private void OnMouseUp()
        {
            ClearValidMoves();
        }

        private void HighlightValidMoves()
        {
            var validMoves = ValidMovesCalculator.GetValidMoves(gameObject.name, row, column);

            foreach (var move in validMoves)
            {
                
                    ChessBoardPlacementHandler.Instance.Highlight(move.row, move.column);              
            }
        }

        private void ClearValidMoves()
        {
            ChessBoardPlacementHandler.Instance.ClearHighlights();
        }
    }

    public static class ValidMovesCalculator
    {
        public static List<(int row, int column)> GetValidMoves(string pieceName, int currentRow, int currentColumn)
        {
            List<(int row, int column)> validMoves = new List<(int row, int column)>();

            if (pieceName.StartsWith("Pawn"))
            {
                // Pawn movement: Move forward one square and capture diagonally
                validMoves.Add((currentRow + 1, currentColumn));
                validMoves.Add((currentRow + 1, currentColumn + 1));
                validMoves.Add((currentRow + 1, currentColumn - 1));
            }
            else if (pieceName.StartsWith("Rook"))
            {
                // Rook movement: Vertically or horizontally any number of squares
                AddStraightMoves(validMoves, currentRow, currentColumn);
            }
            else if (pieceName.StartsWith("Knight"))
            {
                // Knight movement: L-shaped, 2 squares in one direction and 1 square perpendicular
                AddKnightMoves(validMoves, currentRow, currentColumn);
            }
            else if (pieceName.StartsWith("Bishop"))
            {
                // Bishop movement: Diagonally any number of squares
                AddDiagonalMoves(validMoves, currentRow, currentColumn);
            }
            else if (pieceName.StartsWith("Queen"))
            {
                // Queen movement: Combines rook and bishop movement
                AddStraightMoves(validMoves, currentRow, currentColumn);
                AddDiagonalMoves(validMoves, currentRow, currentColumn);
            }
            else if (pieceName.StartsWith("King"))
            {
                // King movement: One square in any direction
                AddAdjacentMoves(validMoves, currentRow, currentColumn);
            }

            return validMoves;
        }

        private static void AddStraightMoves(List<(int, int)> validMoves, int currentRow, int currentColumn)
        {
            // Add vertical moves
            for (int i = currentRow + 1; i < 8; i++)
                validMoves.Add((i, currentColumn));
            for (int i = currentRow - 1; i >= 0; i--)
                validMoves.Add((i, currentColumn));

            // Add horizontal moves
            for (int j = currentColumn + 1; j < 8; j++)
                validMoves.Add((currentRow, j));
            for (int j = currentColumn - 1; j >= 0; j--)
                validMoves.Add((currentRow, j));
        }

        private static void AddDiagonalMoves(List<(int, int)> validMoves, int currentRow, int currentColumn)
        {
            // Add diagonal moves
            for (int i = currentRow + 1, j = currentColumn + 1; i < 8 && j < 8; i++, j++)
                validMoves.Add((i, j));
            for (int i = currentRow - 1, j = currentColumn - 1; i >= 0 && j >= 0; i--, j--)
                validMoves.Add((i, j));
            for (int i = currentRow + 1, j = currentColumn - 1; i < 8 && j >= 0; i++, j--)
                validMoves.Add((i, j));
            for (int i = currentRow - 1, j = currentColumn + 1; i >= 0 && j < 8; i--, j++)
                validMoves.Add((i, j));
        }

        private static void AddKnightMoves(List<(int, int)> validMoves, int currentRow, int currentColumn)
        {
            // Knight moves in L-shape
            AddIfInBounds(validMoves, currentRow + 2, currentColumn + 1);
            AddIfInBounds(validMoves, currentRow + 2, currentColumn - 1);
            AddIfInBounds(validMoves, currentRow - 2, currentColumn + 1);
            AddIfInBounds(validMoves, currentRow - 2, currentColumn - 1);
            AddIfInBounds(validMoves, currentRow + 1, currentColumn + 2);
            AddIfInBounds(validMoves, currentRow - 1, currentColumn + 2);
            AddIfInBounds(validMoves, currentRow + 1, currentColumn - 2);
            AddIfInBounds(validMoves, currentRow - 1, currentColumn - 2);
        }

        private static void AddAdjacentMoves(List<(int, int)> validMoves, int currentRow, int currentColumn)
        {
            // King moves in any direction by one square
            AddIfInBounds(validMoves, currentRow + 1, currentColumn);
            AddIfInBounds(validMoves, currentRow - 1, currentColumn);
            AddIfInBounds(validMoves, currentRow, currentColumn + 1);
            AddIfInBounds(validMoves, currentRow, currentColumn - 1);
            AddIfInBounds(validMoves, currentRow + 1, currentColumn + 1);
            AddIfInBounds(validMoves, currentRow + 1, currentColumn - 1);
            AddIfInBounds(validMoves, currentRow - 1, currentColumn + 1);
            AddIfInBounds(validMoves, currentRow - 1, currentColumn - 1);
        }

        private static void AddIfInBounds(List<(int, int)> validMoves, int row, int column)
        {
            if (row >= 0 && row < 8 && column >= 0 && column < 8)
                validMoves.Add((row, column));
        }
    }
}

