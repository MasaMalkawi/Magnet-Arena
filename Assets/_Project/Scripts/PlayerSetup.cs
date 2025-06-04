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
            // 🔹 توليد اسم عشوائي وتخزينه
            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;
            nameTag.text = playerName;

            // 🔹 لون مميز
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;

            // 🔹 تفعيل الكاميرا
            cameraObject.SetActive(true);
            cameraObject.tag = "Untagged"; // تأكدي إنها مش MainCamera

           

            // 🔹 UI Canvas
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
            // 🔹 للاعبين الآخرين
            nameTag.text = photonView.Owner.NickName;

            cameraObject.SetActive(false);
            if (canvasObject != null) canvasObject.SetActive(false);
        }
    }
}
