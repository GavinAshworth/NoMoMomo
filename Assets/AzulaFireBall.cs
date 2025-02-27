using UnityEngine;

public class AzulaFireBall : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the fireball
    private Transform target; // Target (Momo)
    private float speed;
    private Vector2 direction;

    public void Initialize(Transform spawn, Transform end, float moveSpeed)
    {
        spawnPoint = spawn;
        target = end;
        speed = moveSpeed;
        direction = (end.position - transform.position).normalized;

        // Rotate the fireball to face Momo
        RotateTowardsTarget();
    }

    private void Update()
    {
        // Move the fireball in the specified direction
        transform.position += (Vector3)(direction*speed*Time.deltaTime);
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
        if (target != null)
        {
            // Calculate the direction to the target
            Vector2 directionToTarget = (target.position - spawnPoint.position).normalized;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Apply the rotation to the fireball
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void ResetFireball()
    {
        gameObject.SetActive(false);
        transform.position = spawnPoint.position;
    }
}