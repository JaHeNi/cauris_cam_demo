using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public GameObject car;
    private Vector3 lastPosition;
    private float speed;
    private float timer;
    public float updateInterval = 0.1f; // Update every 0.1 seconds

    void Start()
    {
        lastPosition = car.transform.position;
        timer = updateInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector3 currentPosition = car.transform.position;
            float distance = Vector3.Distance(currentPosition, lastPosition);
            speed = (distance / updateInterval) * 3.6f; // Convert to km/h
            speedText.text = Mathf.RoundToInt(speed).ToString() + " km/h"; // Round speed to integer and add unit
            lastPosition = currentPosition;
            timer = updateInterval; // Reset the timer
        }
    }
}
