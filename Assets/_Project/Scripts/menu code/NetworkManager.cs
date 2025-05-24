using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        PhotonNetwork.JoinLobby(); 
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Joining random room...");
        PhotonNetwork.JoinRandomOrCreateRoom(); 
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No available room, creating new one...");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room! Loading game scene...");
        PhotonNetwork.LoadLevel("GameScene"); 
    }

}

