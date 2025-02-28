using UnityEngine;

public class AzulaFireBall : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the fireball
    private Vector3 targetPosition; // Target position (could be momo or an offset tile)
    private float speed;
    private Vector2 direction;

    public void Initialize(Transform spawn, Vector3 endPosition, float moveSpeed)
    {
        spawnPoint = spawn;
        targetPosition = endPosition;
        speed = moveSpeed;
        direction = (endPosition - transform.position).normalized;

        // Rotate the fireball to face target position
        RotateTowardsTarget();
    }

    private void Update()
    {
        // Move the fireball in the specified direction
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Earth Ability"))
        {
            ResetFireball();
        }
    }

    private void RotateTowardsTarget()
    {
        // Calculate the direction to the target position
        Vector2 directionToTarget = (targetPosition - spawnPoint.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Apply the rotation to the fireball
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetFireball()
    {
        gameObject.SetActive(false);
        transform.position = spawnPoint.position;
    }
}
