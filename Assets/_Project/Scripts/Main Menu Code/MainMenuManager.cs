using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

// This script manages the multiplayer main menu, handles connection to Photon server,
// joining or creating rooms, and transitions to the game scene.
public class MainMenuManager : MonoBehaviourPunCallbacks
{
    // UI buttons and text
    public Button joinRandomButton;
    public Button createRoomButton;
    public TextMeshProUGUI infoText;

    // Track connection status to avoid repeated connection attempts
    private bool isConnecting = false;

    void Awake()
    {
        // Ensures that all players in a room load the same scene automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        // Display initial connection message and try to connect to server
        infoText.text = "Connecting to server...";
        ConnectToServer();

        // Disable buttons until connected to the lobby
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;
    }

    // Attempts to connect to the Photon server
    void ConnectToServer()
    {
        // Skip connection if already connected or currently trying to connect
        if (PhotonNetwork.IsConnected || isConnecting)
        {
            infoText.text = "Already connected.";
            return;
        }

        isConnecting = true;
        PhotonNetwork.GameVersion = "1"; // Set the game version (can be used for compatibility)
        PhotonNetwork.ConnectUsingSettings(); // Connect using default settings from Photon
    }

    // Called when connected to the Photon master server
    public override void OnConnectedToMaster()
    {
        infoText.text = "Connected. Joining lobby...";
        PhotonNetwork.JoinLobby(); // Join the default lobby
    }

    // Called when successfully joined the lobby
    public override void OnJoinedLobby()
    {
        infoText.text = "Lobby joined. Choose an option.";

        // Enable the room options
        joinRandomButton.interactable = true;
        createRoomButton.interactable = true;
    }

    // Called when the player clicks the "Join Random Room" button
    public void JoinRandomRoom()
    {
        infoText.text = "Searching for available rooms...";

        // Disable buttons to prevent multiple requests
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;

        // Attempt to join a random existing room
        PhotonNetwork.JoinRandomRoom();
    }

    // Called when no random rooms were found to join
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "No random rooms found. Joining or creating 'MainRoom'...";

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4; // Set maximum number of players per room

        // Try to join an existing room named "MainRoom", or create it if it doesn't exist
        PhotonNetwork.JoinOrCreateRoom("MainRoom", options, TypedLobby.Default);
    }

    // Called when the player clicks the "Create Room" button
    public void CreateRoom()
    {
        infoText.text = "Creating room... Waiting for other players.";

        // Disable buttons while creating the room
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        // Create a new room named "MainRoom"
        PhotonNetwork.CreateRoom("MainRoom", options);
    }

    // Called when successfully joined a room
    public override void OnJoinedRoom()
    {
        infoText.text = "Room joined. Loading game...";

        // Load the game scene for all players in the room
        PhotonNetwork.LoadLevel("GameScene");
    }

    // Called when disconnected from the server
    public override void OnDisconnected(DisconnectCause cause)
    {
        infoText.text = "Disconnected from server.\nCause: " + cause.ToString();

        // Disable buttons as player is no longer connected
        joinRandomButton.interactable = false;
        createRoomButton.interactable = false;
    }
}
