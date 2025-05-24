using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
}

