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

        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connected to Photon Master Server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Joined Lobby. Joining random room...");
        playButton.interactable = true;
    }

    public void BUTTON_Play()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ Joined Room! Loading GameScene...");
        PhotonNetwork.LoadLevel("GameScene");
    }
}
