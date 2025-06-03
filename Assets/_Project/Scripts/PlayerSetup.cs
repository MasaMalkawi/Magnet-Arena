using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public Renderer bodyRenderer;
    public TextMeshPro nameTag;
    public GameObject cameraObject;
    public GameObject canvasObject;

    void Start()
    {
        if (photonView.IsMine)
        {
            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;
            nameTag.text = playerName;

            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;

            Camera[] allCameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (Camera cam in allCameras)
            {
                if (cam.CompareTag("MainCamera"))
                    cam.tag = "Untagged";
            }

            cameraObject.SetActive(true);
            cameraObject.tag = "MainCamera";

            if (canvasObject != null)
            {
                canvasObject.SetActive(true);

                Canvas canvas = canvasObject.GetComponent<Canvas>();
                if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    canvas.worldCamera = cameraObject.GetComponent<Camera>();
                }
            }
        }
        else
        {
            nameTag.text = photonView.Owner.NickName;
            cameraObject.SetActive(false);
            if (canvasObject != null) canvasObject.SetActive(false);
        }
    }
}

