using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public Renderer bodyRenderer; // ← عشان نغير لونه
    public TextMeshPro nameTag;

    void Start()
    {
        if (photonView.IsMine)
        {
            // عيّن اسم عشوائي
            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;

            // عيّن الاسم في واجهة اللاعب
            nameTag.text = playerName;

            // عيّن لون عشوائي لجسم اللاعب
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;
        }
        else
        {
            // لاعبين آخرين: اظهر اسمهم من PhotonNetwork.NickName
            nameTag.text = photonView.Owner.NickName;
        }
    }
}

