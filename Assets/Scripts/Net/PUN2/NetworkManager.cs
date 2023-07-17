using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [ SerializeField]private GameUi gameUi;

    void Update()
    {
        gameUi.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }

    public void Connect(){

        if(PhotonNetwork.IsConnected){
            PhotonNetwork.JoinRandomRoom();
        } else{
            PhotonNetwork.ConnectUsingSettings();
        }  
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to server. Looking for random Room");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Joining randon room failed because of{message}. Creating a new one ");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer.ActorNumber} entered the room");
    }
}
