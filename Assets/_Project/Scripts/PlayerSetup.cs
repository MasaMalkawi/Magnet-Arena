using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public Renderer bodyRenderer;
    public TextMeshPro nameTag;

    void Start()
    {
        if (photonView.IsMine)
        {
            
            string playerName = "Player " + Random.Range(1000, 9999);
            PhotonNetwork.NickName = playerName;

           
            nameTag.text = playerName;

            
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            bodyRenderer.material.color = randomColor;
        }
        else
        {
         
            nameTag.text = photonView.Owner.NickName;
        }
    }
}

