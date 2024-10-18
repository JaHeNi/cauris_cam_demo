using UnityEngine;

public class Deer : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    void Start()
    {
        // Destroy the deer after 20 seconds
        Destroy(gameObject, 20f);
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        // Ensure the deer stays on the terrain
        Vector3 position = transform.position;
        position.y = Terrain.activeTerrain.SampleHeight(position);
        transform.position = position;

        // Run forwards
        transform.position += direction * speed * Time.deltaTime;
        RotateTowardsDirection();
    }

    void RotateTowardsDirection()
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }
}
