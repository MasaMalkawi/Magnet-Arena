using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public Button playButton;

    private bool isConnecting = false;


    private void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        if (PhotonNetwork.IsConnected || isConnecting)
        {
            Debug.Log("Already connecting or connected.");
            return;
        }

        playButton.interactable = false;
        isConnecting = true;

        PhotonNetwork.GameVersion = "1"; 
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
        Debug.Log("Joined Lobby. Ready to play.");
        playButton.interactable = true;
    }

    public void BUTTON_Play()
    {
        Debug.Log("Attempting to join random room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room found, creating a new one...");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room! Loading GameScene...");
        PhotonNetwork.LoadLevel("GameScene");
    }

}
