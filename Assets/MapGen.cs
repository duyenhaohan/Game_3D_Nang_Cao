using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public Transform player;
    public GameObject[] chunkPrefabs;
    public float chunkLength = 25f;

    private Queue<GameObject> chunks = new Queue<GameObject>();
    private Transform currentEndPoint;

    public int maxChunks = 2;

    void Start()
    {
        SpawnInitialChunks();
    }

    void Update()
    {
        CheckPlayerPassedEndPoint();
    }

    void SpawnInitialChunks()
    {
        GameObject first = SpawnChunk(Vector3.zero);
        chunks.Enqueue(first);

        currentEndPoint = first.transform.Find("EndPoint");

        for (int i = 1; i < maxChunks; i++)
        {
            SpawnNextChunk();
        }
    }

    GameObject SpawnChunk(Vector3 pos)
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        return obj;
    }

    void SpawnNextChunk()
    {
        GameObject newChunk = SpawnChunk(currentEndPoint.position);
        currentEndPoint = newChunk.transform.Find("EndPoint");
        chunks.Enqueue(newChunk);

        if (chunks.Count > maxChunks)
        {
            GameObject old = chunks.Dequeue();
            Destroy(old);
        }
    }

    void CheckPlayerPassedEndPoint()
    {
        if (player.position.z > currentEndPoint.position.z - chunkLength)
        {
            SpawnNextChunk();
        }
    }
}
