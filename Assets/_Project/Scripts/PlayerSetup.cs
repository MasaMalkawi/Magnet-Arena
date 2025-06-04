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

        if (photonView.IsMine)
        {
            cameraObject.SetActive(true);
            cameraObject.tag = "MainCamera";

            canvasObject.SetActive(true);
            canvasObject.GetComponent<Canvas>().worldCamera = cameraObject.GetComponent<Camera>();

            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;
            nameTag.text = playerName;

            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;
        }
        else
        {
            cameraObject.SetActive(false);
            canvasObject.SetActive(false);

            nameTag.text = photonView.Owner.NickName;
        }
    }
}
