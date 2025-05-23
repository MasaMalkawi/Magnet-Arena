using UnityEngine;

public class MagnetSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public int amountToSpawn = 10;
    public Vector3 spawnArea = new Vector3(10, 1, 10);

    private GameObject[] spawnedObjects;

    void Start()
    {
        RespawnObjects();
    }

    public void RespawnObjects()
    {
        // حذف القديم
        if (spawnedObjects != null)
        {
            foreach (var obj in spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
        }

        // توليد جديد
        spawnedObjects = new GameObject[amountToSpawn];
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                spawnArea.y,
                Random.Range(-spawnArea.z, spawnArea.z)
            );

            spawnedObjects[i] = Instantiate(objectPrefab, pos, Quaternion.identity);
        }
    }
}
