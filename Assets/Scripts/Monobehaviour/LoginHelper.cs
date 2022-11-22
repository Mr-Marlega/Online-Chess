using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHelper : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        Connect();
    }
    private void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("Connected to photon server :");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        UnityEngine.Debug.Log("On JoinedRoom called ");
        PhotonNetwork.LoadLevel("Room");
    }
    public override void OnJoinedLobby()
    {
        UnityEngine.Debug.Log("On OnJoinedLobby called ");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        UnityEngine.Debug.Log("On JoinedRoom faliled called ");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2});
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.Debug.Log("Disconnected to photon server :" + cause.ToString());
    }
}
