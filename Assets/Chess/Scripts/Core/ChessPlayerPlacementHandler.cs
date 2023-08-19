using System;
using UnityEngine;
using System.Collections.Generic;

public class ChessPlayerPlacementHandler : MonoBehaviour
{
    [SerializeField] int row, column;
    private GameObject playerObject;

    private void Start()
    {
        playerObject = gameObject;
        ChessBoardPlacementHandler.Instance.PlayerList(playerObject, row, column);
        transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
    }

    private void OnMouseDown()
    {
        HiglightMoves();
    }

    private void OnMouseUp()
    {
        ClearMove();
    }

    private void HiglightMoves()
    {
        var validMovement = MoveCalculator.ValidMoves(gameObject.name, row, column);
        foreach (var move in validMovement)
        {
            if (!ChessBoardPlacementHandler.Instance.TileOccupied(move.row, move.column))
            {
                ChessBoardPlacementHandler.Instance.Highlight(move.row, move.column);
            }
        }
    }

    private void ClearMove()
    {
        ChessBoardPlacementHandler.Instance.ClearHighlights();
    }   
}
public static class MoveCalculator
{
    public static List<(int row, int column)> ValidMoves(string pieceName, int currentRow, int currentColumn)
    {
        List<(int row, int column)> moves = new List<(int row, int column)>();

        switch (pieceName)
        {
            case string _ when pieceName.StartsWith("Pawn"):
                moves.Add((currentRow + 1, currentColumn));
                break;

            case string _ when pieceName.StartsWith("Rook"):
                straightMove(moves, currentRow, currentColumn);
                break;

            case string _ when pieceName.StartsWith("Knight"):
                knightMove(moves, currentRow, currentColumn);
                break;

            case string _ when pieceName.StartsWith("Bishop"):
                diagnolMove(moves, currentRow, currentColumn);
                break;

            case string _ when pieceName.StartsWith("Queen"):
                straightMove(moves, currentRow, currentColumn);
                diagnolMove(moves, currentRow, currentColumn);
                break;

            case string _ when pieceName.StartsWith("King"):
                addjacentMove(moves, currentRow, currentColumn);
                break;
        }

        return moves;
    }
    private static void straightMove(List<(int, int)> validMoves, int currentRow, int currentColumn)
    {
        for (int i = currentRow + 1; i < 8; i++)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, currentColumn))
            break;
            validMoves.Add((i, currentColumn));
        }
        for (int i = currentRow - 1; i >= 0; i--)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, currentColumn))
            break;
            validMoves.Add((i, currentColumn));
        }
        for (int j = currentColumn + 1; j < 8; j++)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(currentRow, j))
            break;
            validMoves.Add((currentRow, j));

        }
        for (int j = currentColumn - 1; j >= 0; j--)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(currentRow, j))
            break;
            validMoves.Add((currentRow, j));
        }
    }
    private static void diagnolMove(List<(int, int)> validMoves, int currentRow, int currentColumn)
    {
        for (int i = currentRow + 1, j = currentColumn + 1; i < 8 && j < 8; i++, j++)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, j))
            break;
            validMoves.Add((i, j));
        }
        for (int i = currentRow - 1, j = currentColumn - 1; i >= 0 && j >= 0; i--, j--)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, j))
            break;
            validMoves.Add((i, j));
        }
        for (int i = currentRow + 1, j = currentColumn - 1; i < 8 && j >= 0; i++, j--)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, j))
            break;
            validMoves.Add((i, j));
        }
        for (int i = currentRow - 1, j = currentColumn + 1; i >= 0 && j < 8; i--, j++)
        {
            if (ChessBoardPlacementHandler.Instance.TileOccupied(i, j))
            break;
            validMoves.Add((i, j));
        }
    }

    private static void knightMove(List<(int, int)> validMoves, int currentRow, int currentColumn)
    {
        BoundedValue(validMoves, currentRow + 2, currentColumn + 1);
        BoundedValue(validMoves, currentRow + 2, currentColumn - 1);
        BoundedValue(validMoves, currentRow - 2, currentColumn + 1);
        BoundedValue(validMoves, currentRow - 2, currentColumn - 1);
        BoundedValue(validMoves, currentRow + 1, currentColumn + 2);
        BoundedValue(validMoves, currentRow - 1, currentColumn + 2);
        BoundedValue(validMoves, currentRow + 1, currentColumn - 2);
        BoundedValue(validMoves, currentRow - 1, currentColumn - 2);
    }
    private static void addjacentMove(List<(int, int)> validMoves, int currentRow, int currentColumn)
    {
        BoundedValue(validMoves, currentRow + 1, currentColumn);
        BoundedValue(validMoves, currentRow - 1, currentColumn);
        BoundedValue(validMoves, currentRow, currentColumn + 1);
        BoundedValue(validMoves, currentRow, currentColumn - 1);
        BoundedValue(validMoves, currentRow + 1, currentColumn + 1);
        BoundedValue(validMoves, currentRow + 1, currentColumn - 1);
        BoundedValue(validMoves, currentRow - 1, currentColumn + 1);
        BoundedValue(validMoves, currentRow - 1, currentColumn - 1);
    }
    private static void BoundedValue(List<(int, int)> validMoves, int row, int column)
    {
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
            validMoves.Add((row, column));
    }
}