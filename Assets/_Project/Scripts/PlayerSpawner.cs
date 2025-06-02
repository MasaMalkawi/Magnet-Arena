using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject Player;

    public override void OnJoinedRoom()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate(Player.name, spawnPos, Quaternion.identity);
    }
}
/*
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
        if (!PhotonNetwork.InRoom)
            return;

        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate(Player.name, spawnPos, Quaternion.identity);
    }
}
*/
