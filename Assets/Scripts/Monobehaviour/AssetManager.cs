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

    public Sprite GetTroopImage(ChesssHelper.PIECE piece, ChesssHelper.COLOR color)
    {
        if(color == ChesssHelper.COLOR.BLACK)
        {
            if (piece == ChesssHelper.PIECE.PAWN) return BPawn;
            if (piece == ChesssHelper.PIECE.ROOK) return BRook;
            if (piece == ChesssHelper.PIECE.KNIGHT) return BKnight;
            if (piece == ChesssHelper.PIECE.BISHOP) return BBishop;
            if (piece == ChesssHelper.PIECE.QUEEN) return BQueen;
            if (piece == ChesssHelper.PIECE.KING) return BKing;
        }
        if (color == ChesssHelper.COLOR.WHITE)
        {
            if (piece == ChesssHelper.PIECE.PAWN) return WPawn;
            if (piece == ChesssHelper.PIECE.ROOK) return WRook;
            if (piece == ChesssHelper.PIECE.KNIGHT) return WKnight;
            if (piece == ChesssHelper.PIECE.BISHOP) return WBishop;
            if (piece == ChesssHelper.PIECE.QUEEN) return WQueen;
            if (piece == ChesssHelper.PIECE.KING) return WKing;
        }
        return None;
    }
}
