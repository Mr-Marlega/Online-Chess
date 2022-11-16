using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHelper : MonoBehaviourPunCallbacks
{
    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("Connected to photon server :");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.Debug.Log("Disconnected to photon server :" + cause.ToString());
    }
}
