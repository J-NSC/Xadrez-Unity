using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [ SerializeField]private GameUi gameUi;

    private const int MAX_PLAYER = 2;
    private const string TEAM = "team";
    private int teams = 0;
    private Tabuleiro tabuleiro;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        tabuleiro = FindObjectOfType<Tabuleiro>();
    }

    void Update()
    {
        gameUi.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }

    public void Connect(){

        if(PhotonNetwork.IsConnected){
            PhotonNetwork.JoinRandomRoom(null,MAX_PLAYER);
            
        } else{
            PhotonNetwork.ConnectUsingSettings();
        }  
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to server. Looking for random Room");
        PhotonNetwork.JoinRandomRoom(null, MAX_PLAYER);
        // PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    private void PrepareTeamSelectionOptions(){
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1){
            teams++;
            SelectedTeams(teams);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Joining randon room failed because of{message}. Creating a new one ");
        PhotonNetwork.CreateRoom(null, new RoomOptions{
            MaxPlayers = MAX_PLAYER
        });
    }

    public void SelectedTeams(int team){
        tabuleiro.teamsCount = team;
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
        PrepareTeamSelectionOptions();
        Debug.LogError(tabuleiro.teamsCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer.ActorNumber} entered the room");
        gameUi.DisabelAllScreem();
        
    }
}
