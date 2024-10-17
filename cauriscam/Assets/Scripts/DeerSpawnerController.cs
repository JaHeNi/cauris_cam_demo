using System.Collections;
using UnityEngine;

public class DeerEventController : MonoBehaviour
{
    public GameObject deerPrefab;
    public Transform car;
    public float deerSpeed = 7f;
    public float spawnDistanceFromCar = 30f; // Distance directly in front of the car
    public float bufferDistance = 2f; // Small distance in front of the car
    public float averageSpawnInterval = 10f; // Average time between spawns

    void Start()
    {
        StartCoroutine(SpawnDeer());
    }

    IEnumerator SpawnDeer()
    {
        while (true)
        {
            float spawnInterval = Random.Range(averageSpawnInterval / 2f, averageSpawnInterval * 1.5f); // Randomize interval around the average
            yield return new WaitForSeconds(spawnInterval);

            float carSpeed = car.GetComponent<Rigidbody>().velocity.magnitude;
            float timeToCollision = (spawnDistanceFromCar - bufferDistance) / carSpeed; // Time for the car to reach the point
            float spawnDistanceToRightOrLeft = deerSpeed * timeToCollision; // Distance from the velocity vector of the car to spawn the deer at based on that time

            Vector3 spawnPosition;
            Vector3 direction;
            if (Random.value > 0.5f)
            {
                spawnPosition = car.position + car.forward * spawnDistanceFromCar + car.right * spawnDistanceToRightOrLeft; // Slightly more forward
                direction = -car.right; // Deer runs to the left
            }
            else
            {
                spawnPosition = car.position + car.forward * spawnDistanceFromCar - car.right * spawnDistanceToRightOrLeft; // Slightly more forward
                direction = car.right; // Deer runs to the right
            }

            GameObject deer = Instantiate(deerPrefab, spawnPosition, Quaternion.identity);
            Deer deerScript = deer.GetComponent<Deer>();
            deerScript.SetDirection(direction);
            deerScript.SetSpeed(deerSpeed);
        }
    }
}
