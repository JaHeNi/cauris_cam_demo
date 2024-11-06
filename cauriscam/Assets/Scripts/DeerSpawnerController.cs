using System.Collections;
using UnityEngine;

public class DeerEventController : MonoBehaviour
{
    public GameObject deerPrefab;
    public Transform car;
    public float deerSpeed = 8f;
    public float spawnDistanceFromCar = 20f; // Distance directly in front of the car
    public float bufferDistance = 1f; // Small distance in front of the car
    public float averageSpawnInterval = 15f; // Average time between spawns

    public HitCounter hitcounter;
    public CarControl cc;

    public AudioClip warningAudio;

    private AudioSource activeDeerAudioSource = null;
    private GameObject activeDeer = null;

    void Start()
    {
        StartCoroutine(SpawnDeer());
    }

    void Update()
    {
        if (activeDeerAudioSource != null)
        {
            float deerDistanceToCar = Vector3.Distance(activeDeer.transform.position, car.transform.position);
            Vector3 deerRelativePosition = activeDeer.transform.position - car.transform.position;
            float maxWarningPitch = 3.0f;
            float minWarningPitch = 1.0f;
            float basePitch = 20.0f;
            float despawnDistance = 5.0f;

            bool deerInFront = Vector3.Dot(car.transform.forward, deerRelativePosition) > 0.0f;

            activeDeerAudioSource.pitch = Mathf.Min(maxWarningPitch, Mathf.Max(minWarningPitch, basePitch / deerDistanceToCar));

            if ((deerDistanceToCar > despawnDistance) && !deerInFront)
            {
                Destroy(activeDeer);
                activeDeer = null;
                activeDeerAudioSource = null;
            }
        }
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
            activeDeer = deer;

            // make deer play warning :D:D
            AudioSource warningAudioSource = deer.AddComponent<AudioSource>();
            activeDeerAudioSource = warningAudioSource;
            warningAudioSource.clip = warningAudio;
            warningAudioSource.loop = true;
            warningAudioSource.Play();

            Deer deerScript = deer.GetComponent<Deer>();
            deerScript.SetHitCounter(hitcounter);
            deerScript.SetDirection(direction);
            deerScript.SetSpeed(deerSpeed);
            
            // make car play warning
            //cc.PlayWarning();
        }
    }
}
