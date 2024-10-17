using UnityEngine;

public class DeerController : MonoBehaviour
{
    public GameObject car;
    public float speed;
    public float distanceToBumper;
    public float activationDistance;
    private bool carHasPassed = false;
    private bool deerStartedMoving = false;

    void Update()
    {
        if (carHasPassed) return; // Stop the deer from moving if the car has already passed

        Vector3 pointInFront = car.transform.position + car.transform.forward * distanceToBumper;
        float distanceToTarget = Vector3.Distance(transform.position, pointInFront);

        if (distanceToTarget <= activationDistance)
        {
            // Mark that the deer has started moving
            deerStartedMoving = true;

            // Move towards the point
            transform.position = Vector3.MoveTowards(transform.position, pointInFront, speed * Time.deltaTime);
            // Rotate to face the point
            transform.LookAt(pointInFront);
        }

        // Check if the deer is running in the same direction as the car AFTER it has started moving. Tämä estää sen, että peura lopettaa auton jahtaamisen.
        if (deerStartedMoving)
        {
            Vector3 deerDirection = transform.forward;
            Vector3 carDirection = car.transform.forward;

            if ((Vector3.Dot(deerDirection, carDirection) > 0.95f)) // Stop if facing same direction or close enough
            {
                carHasPassed = true;
            }
        }
    }
}
