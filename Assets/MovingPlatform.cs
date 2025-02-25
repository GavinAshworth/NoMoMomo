using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the platform
    private Transform endPoint; // End point for the platform
    private float speed;
    private SpriteRenderer spriteRenderer;
    private int level;

    public void Initialize(Transform spawn, Transform end, float moveSpeed, Sprite platformSprite, int platformLevel)
    {
        spawnPoint = spawn;
        endPoint = end;
        transform.position = spawnPoint.position; // Start at the spawn point
        speed = moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = platformSprite;
        level = platformLevel;
    }

    private void Update()
    {
        //If we are no longer on the level associated with this platform we destory it
        if(GameManager.Instance.level != level){
            Destroy(gameObject);
        }
        // Move the platform towards the end point
        transform.position = Vector2.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

        // If the platform reaches the end point, reset it to the spawn point
        if (Vector2.Distance(transform.position, endPoint.position) < 0.1f){
        
            Debug.Log(endPoint.position + " end");
            Debug.Log(transform.position + " platform");
            transform.position = spawnPoint.position;
        }
    }
}