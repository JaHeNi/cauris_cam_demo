using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSpawnerController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject deerPrefab;
    public int numberOfDeersToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDeers();
    }

    void SpawnDeers()
    {
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < numberOfDeersToSpawn; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);
            Instantiate(deerPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        }
    }
}
