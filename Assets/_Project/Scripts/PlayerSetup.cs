using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public GameObject cameraObject;           
    public GameObject canvasObject;           
    public Renderer bodyRenderer;             
    public TextMeshPro nameTag;               
    private PhotonView photonView;            

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Check if this object belongs to the local player
        if (photonView.IsMine)
        {
            // Enable and tag the camera for the local player
            cameraObject.SetActive(true);
            cameraObject.tag = "MainCamera";

            // Enable the canvas and assign the world camera to render UI properly in world space
            canvasObject.SetActive(true);
            canvasObject.GetComponent<Canvas>().worldCamera = cameraObject.GetComponent<Camera>();

            // Generate a random name for the player and set it as their Photon nickname
            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;
            nameTag.text = playerName;

            // Assign a random color to the player's body
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;
        }
        else
        {
            // For remote players: disable their camera and UI canvas
            cameraObject.SetActive(false);
            canvasObject.SetActive(false);
            nameTag.text = photonView.Owner.NickName;
        }
    }
}

