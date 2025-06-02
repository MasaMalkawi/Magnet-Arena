using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public Button joinRandomButton;
    public Button createRoomButton;
    public TextMeshProUGUI infoText;

    private bool isConnecting = false;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        infoText.text = "Connecting to server...";
        ConnectToServer();
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;
    }

    void ConnectToServer()
    {
        if (PhotonNetwork.IsConnected || isConnecting)
        {
            infoText.text = "Already connected.";
            return;
        }

        isConnecting = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        infoText.text = "Connected. Joining lobby...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        infoText.text = "Lobby joined. Choose an option.";
        joinRandomButton.interactable = true;
        createRoomButton.interactable = true;
    }

    public void JoinRandomRoom()
    {
        infoText.text = "Searching for available rooms...";
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "No available rooms found.\n You can create a room instead.";
        joinRandomButton.interactable = true;
        createRoomButton.interactable = true;
    }

    public void CreateRoom()
    {
        infoText.text = "Creating room... Waiting for other players.";
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnJoinedRoom()
    {
        infoText.text = "Room joined. Loading game...";
        PhotonNetwork.LoadLevel("GameScene");
    }
   
    public override void OnDisconnected(DisconnectCause cause)
    {
        infoText.text = "Disconnected from server.\nCause: " + cause.ToString();
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;
    }
}