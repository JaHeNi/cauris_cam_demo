using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public Rigidbody carRigidbody;
    private float speed;
    private float timer;
    public float updateInterval = 0.1f; // Update every 0.1 seconds

    void Start()
    {
        // Initialize the timer
        timer = updateInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // Get the speed from the Rigidbody velocity and convert to km/h
            speed = carRigidbody.velocity.magnitude * 3.6f;

            // Display the speed
            speedText.text = Mathf.RoundToInt(speed).ToString() + " km/h"; // Round speed to integer and add unit

            // Reset the timer
            timer = updateInterval;
        }
    }
}
