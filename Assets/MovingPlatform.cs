using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Speed of the platform
    private Transform spawnPoint; // Spawn point for the platform
    private Transform endPoint; // End point for the platform

    public void Initialize(Transform spawn, Transform end)
    {
        spawnPoint = spawn;
        endPoint = end;
        transform.position = spawnPoint.position; // Start at the spawn point
    }

    private void Update()
    {
        // Move the platform towards the end point
        transform.position = Vector2.MoveTowards(transform.position, endPoint.position, moveSpeed * Time.deltaTime);

        // If the platform reaches the end point, reset it to the spawn point
        if (Vector2.Distance(transform.position, endPoint.position) < 0.1f){
        
            Debug.Log(endPoint.position + " end");
            Debug.Log(transform.position + " platform");
            transform.position = spawnPoint.position;
        }
    }
}