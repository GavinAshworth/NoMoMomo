using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the platform
    private Transform endPoint; // End point for the platform
    private float speed;
    private SpriteRenderer spriteRenderer;
    private int level;
    private bool isReverse;

    public void Initialize(Transform spawn, Transform end, float moveSpeed, Sprite platformSprite, int platformLevel, bool reverse)
    {
        spawnPoint = spawn;
        endPoint = end;
        transform.position = spawnPoint.position; // Start at the spawn point
        speed = moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = platformSprite;
        level = platformLevel;
        isReverse = reverse;

        if(isReverse){
            spriteRenderer.flipX = true;
        }
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
            transform.position = spawnPoint.position;
        }
    }
}