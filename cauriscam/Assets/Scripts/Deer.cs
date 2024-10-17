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
