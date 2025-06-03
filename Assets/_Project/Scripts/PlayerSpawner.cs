using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        if (!PhotonNetwork.InRoom)
            return;

        Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), 9f, Random.Range(0f, 4f));

        GameObject playerObj = PhotonNetwork.Instantiate(Player.name, spawnPos, Quaternion.identity);

        PhotonView view = playerObj.GetComponent<PhotonView>();
        if (view != null && view.IsMine)
        {
          
            Transform cameraTransform = playerObj.transform.Find("Camera");
            Transform canvasTransform = playerObj.transform.Find("Canvas");

            if (cameraTransform != null)
                cameraTransform.gameObject.SetActive(true);

            if (canvasTransform != null)
            {
                canvasTransform.gameObject.SetActive(true);

               
                Canvas canvas = canvasTransform.GetComponent<Canvas>();
                Camera cam = cameraTransform?.GetComponent<Camera>();

                if (canvas != null && cam != null)
                    canvas.worldCamera = cam;
            }
        }
    }
}


