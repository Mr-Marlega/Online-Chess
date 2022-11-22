using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer //: MonoBehaviourPun
{
    ChessHelper.COLOR PlayerColor;
    string PlayerName = string.Empty;
    public ChessHelper.COLOR PLAYER_COLOR
    {
        get
        {
            return PlayerColor;
        }
    }
    public void SetPlayerColor(ChessHelper.COLOR _color)
    {
        PlayerColor = _color;
    }
}
