using UnityEngine;
using Photon.Pun; 

public class SceneController : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        PhotonNetwork.LoadLevel("GameScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

