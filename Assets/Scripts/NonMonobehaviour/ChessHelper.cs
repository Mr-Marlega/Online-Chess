using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace ChesssHelper
{
    public enum PIECE
    {
        NONE,
        KING,
        KNIGHT,
        BISHOP,
        QUEEN,
        ROOK,
        PAWN
    }
    public enum COLOR
    {
        NONE,
        BLACK,
        WHITE
    }
    public enum SQUARE_STATE
    {
        NONE,
        SELECTED,
        HIGHLIGHT,
        ATTACK
    }
        
    public class Square
    {
        int row, col;
        Troop m_troop;
        COLOR squareColor;
        SQUARE_STATE state;
        public event Action onStateChange;
        public event Action onTroopChange;
        public Square() : this(0, 0, new Troop(), COLOR.NONE) { }
        public Square(int _row, int _col, Troop _troop, COLOR _color)
        {
            row = _row;
            col = _col;
            m_troop = _troop;
            squareColor = _color;
            state = SQUARE_STATE.NONE;
        }
        public Troop Troop
        {
            get
            {
                return m_troop;
            }
            set
            {
                m_troop = value;
                var handler = onTroopChange;
                if (null != onTroopChange)
                    handler();
            }
        }
        public COLOR GetSquareColor()
        {
            return squareColor;
        }
        public int rowIndex()
        {
            return row;
        }
        public int colIndex()
        {
            return col;
        }
        public void ChangeState(SQUARE_STATE _state)
        {
            state = _state;
            var handler = onStateChange;
            if (null != onStateChange)
                handler();
        }
        public SQUARE_STATE GetSquareState()
        {
            return state;
        }
    }
    public class Board
    {
        int mRow, mCol;
        List<Square> mSquares;
        Dictionary<KeyValuePair<int, int>, Square> mSquaresDic;

        public Board(int row, int col)
        {
            mRow = row;
            mCol = col;
            mSquares = new List<Square>();
            mSquaresDic = new Dictionary<KeyValuePair<int, int>, Square>();
            InitEmpty(row, col);
        }
        public void InitEmpty(int row, int col)
        {
            mSquaresDic.Clear();
            mSquares.Clear();
            for (int r = 1; r <= row; r++)
            {
                for(int c = 1; c <= col; c++)
                {
                    if((c + r) % 2 == 0)
                    {
                        Square sqr = new Square(r, c, new Troop(), COLOR.BLACK);
                        mSquaresDic.Add(new KeyValuePair<int, int>(r, c), sqr);
                        mSquares.Add(sqr);
                    }else
                    {
                        Square sqr = new Square(r, c, new Troop(), COLOR.WHITE);
                        mSquaresDic.Add(new KeyValuePair<int, int>(r, c), sqr);
                        mSquares.Add(sqr);
                    }
                    
                }
            }
        }
        public void ResetBoard()
        {
            InitEmpty(mRow, mCol);
            ResetTroop();
        }
        void ResetTroop()
        {
            for(int colIndex = 1; colIndex <= mCol; colIndex++)
            {
                mSquaresDic[new KeyValuePair<int, int>(2, colIndex)].Troop = new Pawn(COLOR.WHITE);
                mSquaresDic[new KeyValuePair<int, int>(mRow - 1, colIndex)].Troop = new Pawn(COLOR.BLACK);
            }

            mSquaresDic[new KeyValuePair<int, int>(1, 1)].Troop = new Rook(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(1, 8)].Troop = new Rook(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(8, 1)].Troop = new Rook(COLOR.BLACK);
            mSquaresDic[new KeyValuePair<int, int>(8, 8)].Troop = new Rook(COLOR.BLACK);

            mSquaresDic[new KeyValuePair<int, int>(1, 2)].Troop = new Knight(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(1, 7)].Troop = new Knight(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(8, 2)].Troop = new Knight(COLOR.BLACK);
            mSquaresDic[new KeyValuePair<int, int>(8, 7)].Troop = new Knight(COLOR.BLACK);

            mSquaresDic[new KeyValuePair<int, int>(1, 3)].Troop = new Bishop(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(1, 6)].Troop = new Bishop(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(8, 3)].Troop = new Bishop(COLOR.BLACK);
            mSquaresDic[new KeyValuePair<int, int>(8, 6)].Troop = new Bishop(COLOR.BLACK);

            mSquaresDic[new KeyValuePair<int, int>(1, 4)].Troop = new Queen(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(8, 4)].Troop = new Queen(COLOR.BLACK);

            mSquaresDic[new KeyValuePair<int, int>(1, 5)].Troop = new King(COLOR.WHITE);
            mSquaresDic[new KeyValuePair<int, int>(8, 5)].Troop = new King(COLOR.BLACK);
        }
        public Square GetBoardSquareByIndex(int _rowIndex, int _colIndex)
        {
            if(_rowIndex <= mRow && _rowIndex > 0 && _colIndex > 0 && _colIndex <= mCol)
                return mSquaresDic[new KeyValuePair<int, int>(_rowIndex, _colIndex)];

            return null;
        }
        public List<Square> GetSquareList()
        {
            return mSquares;// Make copy and return to avoid concistency
        }
        public int GetMaxRow()
        {
            return mRow;
        }
        public int GetMaxCol()
        {
            return mCol;
        }
    }
    public interface ITroop
    {
        List<Square> Move(Board _board, Square _sqr);
    }
    public class Troop : ITroop
    {
        protected COLOR piece_color;
        protected PIECE peice;

        public Troop()
        {
            piece_color = COLOR.NONE;
            peice = PIECE.NONE;
        }
        public void SetPiece(PIECE _piece)
        {
            peice = _piece;
        }
        public PIECE GetPiece()
        {
            return peice;
        }
        public COLOR GetPieceColor()
        {
            return piece_color;
        }
        //Interface Implementation
         public virtual List<Square> Move(Board _board, Square _sqr) { return new List<Square>(); }
    }
    public class Pawn : Troop
    {
        public Pawn(COLOR _color)
        {
            peice = PIECE.PAWN;
            piece_color = _color;
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> _squareList = new List<Square>();
            if(_sqr.Troop.GetPieceColor() == COLOR.WHITE)
            {
                if (_sqr.rowIndex() == 2)
                {
                    Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex());
                    if (sqr.Troop.GetPiece() == PIECE.NONE)
                    {
                        _squareList.Add(sqr);
                        Square sqr2 = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 2, _sqr.colIndex());
                        if (sqr2.Troop.GetPiece() == PIECE.NONE)
                        {
                            _squareList.Add(sqr2);
                        }
                    }
                }else
                {
                    if(_sqr.rowIndex() + 1 <= _board.GetMaxRow())
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex());
                        if (sqr.Troop.GetPiece() == PIECE.NONE)
                        {
                            _squareList.Add(sqr);
                        }
                    }
                    if(_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() + 1 <= _board.GetMaxCol())
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() + 1);
                        if (sqr.Troop.GetPiece() != PIECE.NONE && sqr.Troop.GetPieceColor() != this.GetPieceColor())
                        {
                            _squareList.Add(sqr);
                        }
                    }
                    if (_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() - 1 > 0)
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() -1);
                        if (sqr.Troop.GetPiece() != PIECE.NONE && sqr.Troop.GetPieceColor() != this.GetPieceColor())
                        {
                            _squareList.Add(sqr);
                        }
                    }
                }
            }else
            {
                if (_sqr.rowIndex() == _board.GetMaxRow() -1)
                {
                    Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex());
                    if (sqr.Troop.GetPiece() == PIECE.NONE)
                    {
                        _squareList.Add(sqr);
                        Square sqr2 = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 2, _sqr.colIndex());
                        if (sqr2.Troop.GetPiece() == PIECE.NONE)
                        {
                            _squareList.Add(sqr2);
                        }
                    }
                }
                else
                {
                    if (_sqr.rowIndex() - 1 > 0)
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex());
                        if (sqr.Troop.GetPiece() == PIECE.NONE)
                        {
                            _squareList.Add(sqr);
                        }
                    }
                    if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() + 1 <= _board.GetMaxCol())
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() + 1);
                        if (sqr.Troop.GetPiece() != PIECE.NONE && sqr.Troop.GetPieceColor() != this.GetPieceColor())
                        {
                            _squareList.Add(sqr);
                        }
                    }
                    if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() - 1 > 0)
                    {
                        Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() - 1);
                        if (sqr.Troop.GetPiece() != PIECE.NONE && sqr.Troop.GetPieceColor() != this.GetPieceColor())
                        {
                            _squareList.Add(sqr);
                        }
                    }
                }
            }
            return _squareList;
        }
    }
    public class Rook : Troop
    {
        public Rook(COLOR _color)
        {
            peice = PIECE.ROOK;
            piece_color = _color;
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> _squareList = new List<Square>();
            for(int _row = _sqr.rowIndex() -1; _row > 0; _row--)
            {
               Square sqr = _board.GetBoardSquareByIndex(_row, _sqr.colIndex());
                if(sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if(sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _row = _sqr.rowIndex() + 1; _row <= _board.GetMaxRow(); _row++)
            {
                Square sqr = _board.GetBoardSquareByIndex(_row, _sqr.colIndex());
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _col = _sqr.colIndex() -1; _col > 0; _col--)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex(), _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _col = _sqr.colIndex() + 1; _col <= _board.GetMaxCol(); _col++)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex(), _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            return _squareList;
        }
    }
    public class Knight : Troop
    {
        public Knight(COLOR _color)
        {
            peice = PIECE.KNIGHT;
            piece_color = _color;
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> _squareList = new List<Square>();
            if(_sqr.rowIndex() + 2 <= _board.GetMaxRow() && _sqr.colIndex() + 1 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 2, _sqr.colIndex() + 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() + 2 <= _board.GetMaxRow() && _sqr.colIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 2, _sqr.colIndex() - 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 2 > 0 && _sqr.colIndex() + 1 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 2, _sqr.colIndex() + 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 2 > 0 && _sqr.colIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex()  -2, _sqr.colIndex() - 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() + 2 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() + 2);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() - 2 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() - 2);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() + 2 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() + 2);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() - 2 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() - 2);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }

            return _squareList;
        }
        bool IsValidMove(Square _sqr)
        {
            if (_sqr.Troop.GetPiece() == PIECE.NONE)
            {
                return true;
            }
            if (_sqr.Troop.GetPieceColor() != this.GetPieceColor())
            {
                return true;
            }
            return false;
        }
    }
    public class Bishop : Troop
    {
        public Bishop(COLOR _color)
        {
            peice = PIECE.BISHOP;
            piece_color = _color;
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> _squareList = new List<Square>();
            for (int _row = _sqr.rowIndex() - 1, _col = _sqr.colIndex() - 1; _row > 0 && _col > 0; _row--, _col--)
            {
                Square sqr = _board.GetBoardSquareByIndex(_row, _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _row = _sqr.rowIndex() + 1, _col = _sqr.colIndex() - 1; _row <= _board.GetMaxRow() && _col > 0; _row++, _col--)
            {
                Square sqr = _board.GetBoardSquareByIndex(_row, _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _row = _sqr.rowIndex() - 1, _col = _sqr.colIndex() + 1; _row > 0 && _col <= _board.GetMaxCol(); _row--, _col++)
            {
                Square sqr = _board.GetBoardSquareByIndex(_row, _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            for (int _row = _sqr.rowIndex() + 1, _col = _sqr.colIndex() + 1; _row < _board.GetMaxRow() && _col <= _board.GetMaxCol(); _row++, _col++)
            {
                Square sqr = _board.GetBoardSquareByIndex(_row, _col);
                if (sqr.Troop.GetPiece() == PIECE.NONE)
                {
                    _squareList.Add(sqr);
                    continue;
                }
                if (sqr.Troop.GetPieceColor() != this.GetPieceColor())
                {
                    _squareList.Add(sqr);
                }
                break;
            }
            return _squareList;
        }
    }
    public class Queen : Troop
    {
        Troop mBishop, mRook;
        public Queen(COLOR _color)
        {
            peice = PIECE.QUEEN;
            piece_color = _color;
            mBishop = new Bishop(_color);
            mRook = new Rook(_color);
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> bishpSqr = mBishop.Move(_board, _sqr);
            List<Square> rookSqr = mRook.Move(_board, _sqr);
            List<Square> queenSqr = bishpSqr;

            foreach(Square sqr in rookSqr)
            {
                if(!queenSqr.Contains(sqr))
                    queenSqr.Add(sqr);
            }
            return queenSqr;
        }
    }
    public class King : Troop
    {
        public King(COLOR _color)
        {
            peice = PIECE.KING;
            piece_color = _color;
        }
        public override List<Square> Move(Board _board, Square _sqr)
        {
            List<Square> _squareList = new List<Square>();
            if (_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() + 1 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() + 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() + 1 <= _board.GetMaxRow() && _sqr.colIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex() - 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() + 1 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() + 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 1 > 0 && _sqr.colIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex() - 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.colIndex() + 1 <= _board.GetMaxCol())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex(), _sqr.colIndex() + 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if ( _sqr.colIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex(), _sqr.colIndex() - 1);
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() - 1 > 0)
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() - 1, _sqr.colIndex());
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }
            if (_sqr.rowIndex() + 1 <= _board.GetMaxRow())
            {
                Square sqr = _board.GetBoardSquareByIndex(_sqr.rowIndex() + 1, _sqr.colIndex());
                if (IsValidMove(sqr))
                    _squareList.Add(sqr);
            }

            return _squareList;
        }
        bool IsValidMove(Square _sqr)
        {
            if (_sqr.Troop.GetPiece() == PIECE.NONE)
            {
                return true;
            }
            if (_sqr.Troop.GetPieceColor() != this.GetPieceColor())
            {
                return true;
            }
            return false;
        }
    }
}

