using UnityEngine;
using System.Collections;
public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // Spawn point 
    [SerializeField] private Transform endPoint; // End point 
    [SerializeField] private float spawnInterval = 2f; // Time between spawns
    [SerializeField] private float moveSpeed = 2f; // Speed of the projectile
    [SerializeField] private int projectileCount = 3; // Number of projectiles to spawn
    [SerializeField] private bool isReverse;

    private ParentProjectileSpawner parentScript;
    private GameObject projectilePrefab; // Projectile prefab to spawn
    private Sprite projectileSprite;
    private int level;

    private bool isSpawning;
    private GameObject[] projectiles; // Array to store the projectiles

    private void Start()
    {
        if (transform.parent != null)
        {
            parentScript = transform.parent.GetComponent<ParentProjectileSpawner>();

            if (parentScript != null)
            {
                projectileSprite = parentScript.GetSprite();
                projectilePrefab = parentScript.GetProjectileObject();
                level = parentScript.GetSpawnerLevel();
            }
            else
            {
                Debug.LogError("No script found on parent", this);
            }
        }
        else
        {
            Debug.LogError("This object has no parent", this);
        }
        //If reverse is true we spawn in the other direction and reverse them 
        if(isReverse){
            Transform temp = spawnPoint;
            spawnPoint = endPoint;
            endPoint = temp;

        }

        // Initialize the platforms
        projectiles = new GameObject[projectileCount];
        for (int i = 0; i < projectileCount; i++)
        {
        
            projectiles[i] = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            projectiles[i].GetComponent<MovingObject>().Initialize(spawnPoint, endPoint, moveSpeed, projectileSprite, level, isReverse);
            projectiles[i].SetActive(false); // Start inactive
        }
    }

    private void Update(){
        //Here we check what level we are on. If we are on the level for these spawners than we spawn the platforms
        if(GameManager.Instance.level == level && !isSpawning){
            // Start spawning platforms
            StartCoroutine(SpawnProjectiles());
        } 
    }

    private IEnumerator SpawnProjectiles()
    {
        isSpawning = true;
        int index = 0;
        while (index < projectileCount)
        {
            // Activate the next platform in the array
            projectiles[index].SetActive(true);
            projectiles[index].transform.position = spawnPoint.position; // Reset position

            // Move to the next platform 
            index = (index + 1);

            // Wait for the specified spawn interval. We add in a little randomness to make level different every time
            yield return new WaitForSeconds(spawnInterval + Random.Range(-0.5f, 0.5f));
        }
    }
}