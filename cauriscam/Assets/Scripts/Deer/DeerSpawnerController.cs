using System.Collections.Generic;
using UnityEngine;

public class DeerSpawnerController : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>(); // Assign your spawn points in the inspector
    public GameObject deerPrefab; // Reference to your deer prefab
    public GameObject car; // Reference to your car object
    public int numberOfDeerToSpawn; // Specify the number of deer to spawn in the inspector
    public float deerSpeed = 5f; // Speed at which the deer moves
    public float distanceToBumper = 10f; // Deer runs x units in front of the car

    void Start()
    {
        SpawnDeer();
    }

    void SpawnDeer()
    {
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < numberOfDeerToSpawn; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Count); // Corrected to Count
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            GameObject deer = Instantiate(deerPrefab, spawnPoints[randomIndex].transform.position, Quaternion.identity);

            DeerController deerController = deer.AddComponent<DeerController>();
            deerController.car = car;
            deerController.speed = deerSpeed;
            deerController.distanceToBumper = distanceToBumper;

            // Get activationDistance from SpawnRadiusHelper component
            SpawnRadiusHelper spawnRadiusHelper = spawnPoints[randomIndex].GetComponent<SpawnRadiusHelper>();
            if (spawnRadiusHelper != null)
            {
                deerController.activationDistance = spawnRadiusHelper.activationDistance;
            }
        }
    }
}
