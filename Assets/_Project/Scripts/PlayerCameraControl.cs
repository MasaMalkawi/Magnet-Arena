using Photon.Pun;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviourPun
{
    public Camera playerCamera;
    public AudioListener audioListener;

    void Start()
    {
        if (photonView.IsMine)
        {
            
            playerCamera.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
            audioListener.enabled = false;
        }
    }
}
