using Chess.Constants;
using ChesssHelper;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardViewController : MonoBehaviourPun
{
    public GameObject squarePrefab;
    Board m_Board;
    [SerializeField]
    Image chessBoardImage;
    bool isSquareSelected;
    Square SelectedSquare;

    //Data

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void Start()
    {
        //TODO
        // m_Board = GameManager.instance.Board;
        InitBoardUI();
        isSquareSelected = false;
        SelectedSquare = null;
    }
    void InitBoardUI()
    {
        int boardSize = Screen.height > Screen.width ? Screen.width : Screen.height;
        chessBoardImage.rectTransform.sizeDelta = new Vector2(boardSize, boardSize);

        if (m_Board == null)
            m_Board = new Board(ChessConstants.ROW, ChessConstants.COL);
        m_Board.ResetBoard();
        InitAllSquare(m_Board, boardSize);
        //OnChessBoardUpdate(m_Board);
    }
    void InitAllSquare(Board _board, int _boardSize)
    {
        float squareSize = (float)_boardSize / ChessConstants.ROW;
        float origin = -(float)_boardSize / 2 - squareSize /2;
        foreach(Square _sqr in _board.GetSquareList())
        {
            GameObject squareObj = Instantiate(squarePrefab, this.transform);
            SquareViewController SVC = squareObj.GetComponent<SquareViewController>();
            SVC.SetSquare(_sqr);
            SVC.SetImageSize(squareSize);
            SVC.setSquarePos(origin + squareSize*_sqr.colIndex(),origin + squareSize*_sqr.rowIndex());
        }
    }
    void OnChessBoardUpdate(SquareData square1, SquareData square2)
    {
        Square _Sqr = m_Board.GetBoardSquareByIndex(square1.row, square1.col);
        if(square1.color == 0 && square1.piece == 0)
        {
            _Sqr.Troop = new Troop();
        }else
        {
            PIECE piece = (PIECE)(square1.piece);
        }
        
    }
    public void TryMove(Square _square)
    {
        if(!isSquareSelected)
        {
            if (_square.Troop.GetPiece() == PIECE.NONE)
                return;
            ResetAllSquareState(m_Board);
            _square.ChangeState(SQUARE_STATE.SELECTED);
            SelectedSquare = _square;
            List<Square> squareListToMove = _square.Troop.Move(m_Board, _square);
            HighLightSafeSquare(squareListToMove);
            isSquareSelected = true;
        }else
        {
            if(SelectedSquare != null && _square.Troop.GetPiece() != PIECE.NONE && _square.GetSquareState() == SQUARE_STATE.NONE)
            {
                isSquareSelected = false;
                TryMove(_square);
            }
            else if(_square.GetSquareState() == SQUARE_STATE.HIGHLIGHT || _square.GetSquareState() == SQUARE_STATE.ATTACK)
            {
                UnityEngine.Debug.Log("Switching troop : " + _square.Troop.GetPiece() + " " + SelectedSquare.Troop.GetPiece());
                 _square.Troop = SelectedSquare.Troop;
                 SelectedSquare.Troop = new Troop();
                ResetAllSquareState(m_Board);
                SendMoveUnitsToTargetPositionEvent();
            }
        }
    }
    void ResetAllSquareState(Board _board)
    {
        foreach (Square sqr in _board.GetSquareList())
        {
            sqr.ChangeState(SQUARE_STATE.NONE);
        }
    }
    void HighLightSafeSquare(List<Square> _squareList)
    {
        foreach(Square sqr in _squareList)
        {
            if (sqr.Troop.GetPiece() == PIECE.NONE)
                sqr.ChangeState(SQUARE_STATE.HIGHLIGHT);
            else
                sqr.ChangeState(SQUARE_STATE.ATTACK);
        }
    }

    /// <summary>
    /// chess Board syncing
    /// </summary>

    byte updateBoardEventCode = 1;
    private void SendMoveUnitsToTargetPositionEvent()
    {
        object[] content = new object[] {};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(updateBoardEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == updateBoardEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
        }
        
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
