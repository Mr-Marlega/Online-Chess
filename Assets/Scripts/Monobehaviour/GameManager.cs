using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public AssetManager assetManager;
    public BoardViewController boardViewController;
    private void Start()
    {
        instance = this;

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Looby");

            return;
        }
        //Instantiating Player On board
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("On Player entered room : " + other.NickName);
       if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Loading Chess gample scene ");
            //LoadGamePlay();
        }
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("On Player lefted room : " + other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Loading Chess gample scene ");
           // LoadGamePlay();
        }
   }
    public override void OnLeftRoom()
    {
        Debug.Log("Player left the rrom");
        SceneManager.LoadScene("Lobby");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void LoadGamePlay()
    {

    }
}
