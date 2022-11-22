using Chess.Constants;
using ChessHelper;
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
    COLOR TURN = COLOR.NONE;

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
        TURN = COLOR.WHITE;
    }
    void InitBoardUI()
    {
        int boardSize = Screen.height > Screen.width ? Screen.width : Screen.height;
        chessBoardImage.rectTransform.sizeDelta = new Vector2(boardSize, boardSize);

        if (m_Board == null)
            m_Board = new Board(ChessConstants.ROW, ChessConstants.COL);
        m_Board.ResetBoard();
        if(GameManager.instance.MyPlayer.PLAYER_COLOR == COLOR.BLACK)
            chessBoardImage.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
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
            SVC.SetTroopImageRotation(GameManager.instance.MyPlayer.PLAYER_COLOR);
        }
    }
    void OnChessBoardUpdate(SquareData square1, SquareData square2)
    {
        Square _Sqr1 = m_Board.GetBoardSquareByIndex(square1.row, square1.col);
        _Sqr1.Troop = TroopFactory.MakeTroop((PIECE)square1.piece, (COLOR)square1.color);
        Square _Sqr2 = m_Board.GetBoardSquareByIndex(square2.row, square2.col);
        _Sqr2.Troop = TroopFactory.MakeTroop((PIECE)square2.piece, (COLOR)square2.color);
        ResetAllSquareState(m_Board);
        isSquareSelected = false;
        if (TURN == COLOR.WHITE)
            TURN = COLOR.BLACK;
        else
            TURN = COLOR.WHITE;
        SendPlayerTurnEvent((int)TURN);
    }
    public void TryMove(Square _square)
    {
        if(TURN == GameManager.instance.MyPlayer.PLAYER_COLOR)
        {
            if (!isSquareSelected)
            {
                if (_square.Troop.GetPiece() == PIECE.NONE || _square.Troop.GetPieceColor() != TURN)
                    return;
                ResetAllSquareState(m_Board);
                _square.ChangeState(SQUARE_STATE.SELECTED);
                SelectedSquare = _square;
                List<Square> squareListToMove = _square.Troop.Move(m_Board, _square);
                HighLightSafeSquare(squareListToMove);
                isSquareSelected = true;
            }
            else
            {
                if (SelectedSquare != null && _square.Troop.GetPiece() != PIECE.NONE && _square.GetSquareState() == SQUARE_STATE.NONE)
                {
                    isSquareSelected = false;
                     TryMove(_square);
                }
                else if (_square.GetSquareState() == SQUARE_STATE.HIGHLIGHT || _square.GetSquareState() == SQUARE_STATE.ATTACK)
                {
                    UnityEngine.Debug.Log("Switching troop : " + _square.Troop.GetPiece() + " " + SelectedSquare.Troop.GetPiece());
                    //_square.Troop = SelectedSquare.Troop;
                    SquareData data1 = new SquareData();
                    data1.row = _square.rowIndex();
                    data1.col = _square.colIndex();
                    data1.piece = (int)SelectedSquare.Troop.GetPiece();
                    data1.color = (int)SelectedSquare.Troop.GetPieceColor();

                    SquareData data2 = new SquareData();
                    data2.row = SelectedSquare.rowIndex();
                    data2.col = SelectedSquare.colIndex();
                    data2.piece = (int)PIECE.NONE;
                    data2.color = (int)COLOR.NONE;

                    //SelectedSquare.Troop = new Troop();
                    SendMoveSquareAndSyncEvent(data1, data2);
                }
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

    byte updateBoardEventCode = 1, TurnEventCode = 2;
    private void SendMoveSquareAndSyncEvent(SquareData square1, SquareData square2)
    {
        object[] content = new object[] {square1.row, square1.col, square1.piece, square1.color,
        square2.row, square2.col, square2.piece, square2.col};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(updateBoardEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    private void SendPlayerTurnEvent(int Turn)
    {
        object[] content = new object[] {Turn};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(TurnEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == updateBoardEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            SquareData square1 = new SquareData();
            square1.row = (int)data[0];
            square1.col = (int)data[1];
            square1.piece = (int)data[2];
            square1.color = (int)data[3];

            SquareData square2 = new SquareData();
            square2.row = (int)data[4];
            square2.col = (int)data[5];
            square2.piece = (int)data[6];
            square2.color = (int)data[7];
            OnChessBoardUpdate(square1, square2);
        }
        if(eventCode == TurnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            TURN = (COLOR)data[0];
        }
        
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
