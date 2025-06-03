
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
        PhotonNetwork.Instantiate(Player.name, spawnPos, Quaternion.identity);
    }
}

