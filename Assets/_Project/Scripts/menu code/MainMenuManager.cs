using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public Button playButton; // الزر من الواجهة

    private bool isConnecting = false;

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
        PhotonNetwork.JoinLobby(); // يتم استدعاؤها مرة واحدة فقط بعد التأكد من الاتصال
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Joined Lobby. Joining random room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("❌ No available rooms, creating one...");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ Joined Room! Loading GameScene...");
        PhotonNetwork.LoadLevel("GameScene");
    }
}
