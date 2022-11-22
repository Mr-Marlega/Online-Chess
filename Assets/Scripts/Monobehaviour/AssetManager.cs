using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public Sprite BPawn;
    public Sprite BRook;
    public Sprite BKnight;
    public Sprite BBishop;
    public Sprite BQueen;
    public Sprite BKing;

    public Sprite WPawn;
    public Sprite WRook;
    public Sprite WKnight;
    public Sprite WBishop;
    public Sprite WQueen;
    public Sprite WKing;

    public Sprite None;

    public Sprite GetTroopImage(ChessHelper.PIECE piece, ChessHelper.COLOR color)
    {
        if(color == ChessHelper.COLOR.BLACK)
        {
            if (piece == ChessHelper.PIECE.PAWN) return BPawn;
            if (piece == ChessHelper.PIECE.ROOK) return BRook;
            if (piece == ChessHelper.PIECE.KNIGHT) return BKnight;
            if (piece == ChessHelper.PIECE.BISHOP) return BBishop;
            if (piece == ChessHelper.PIECE.QUEEN) return BQueen;
            if (piece == ChessHelper.PIECE.KING) return BKing;
        }
        if (color == ChessHelper.COLOR.WHITE)
        {
            if (piece == ChessHelper.PIECE.PAWN) return WPawn;
            if (piece == ChessHelper.PIECE.ROOK) return WRook;
            if (piece == ChessHelper.PIECE.KNIGHT) return WKnight;
            if (piece == ChessHelper.PIECE.BISHOP) return WBishop;
            if (piece == ChessHelper.PIECE.QUEEN) return WQueen;
            if (piece == ChessHelper.PIECE.KING) return WKing;
        }
        return None;
    }
}
