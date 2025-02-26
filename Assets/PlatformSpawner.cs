using UnityEngine;
using System.Collections;
public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // Spawn point for platforms
    [SerializeField] private Transform endPoint; // End point for platforms
    [SerializeField] private float spawnInterval = 2f; // Time between platform spawns
    [SerializeField] private float moveSpeed = 2f; // Speed of the platform
    [SerializeField] private int platformCount = 3; // Number of platforms to spawn
    [SerializeField] private bool isReverse;
    [SerializeField] bool isLong;

    private ParentPlatformSpawners parentScript;
    private GameObject shortPrefab; // Platform prefab to spawn
    private GameObject longPrefab; // Platform prefab to spawn
    private Sprite shortSprite;
    private Sprite longSprite;
    private int level;

    private bool isSpawning;
    private GameObject[] platforms; // Array to store the platforms

    private void Start()
    {
        if (transform.parent != null)
        {
            parentScript = transform.parent.GetComponent<ParentPlatformSpawners>();

            if (parentScript != null)
            {
                //Here we are getting all the sprites and platforms for our possible spawners. This I believe will help making new levels easier.
                longSprite = parentScript.GetSpriteLong();
                shortSprite = parentScript.GetSpriteShort();
                shortPrefab = parentScript.GetPlatformShort();
                longPrefab = parentScript.GetPlatformLong();
                level = parentScript.GetSpawnerLevel();
            }
            else
            {
                Debug.LogError("No ParentPlatformSpawners script found on parent", this);
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
        platforms = new GameObject[platformCount];
        for (int i = 0; i < platformCount; i++)
        {
            if(isLong){
                platforms[i] = Instantiate(longPrefab, spawnPoint.position, Quaternion.identity);
                platforms[i].GetComponent<MovingObject>().Initialize(spawnPoint, endPoint, moveSpeed, longSprite,level, isReverse, false);
            }
            else{
                platforms[i] = Instantiate(shortPrefab, spawnPoint.position, Quaternion.identity);
                platforms[i].GetComponent<MovingObject>().Initialize(spawnPoint, endPoint, moveSpeed, shortSprite, level, isReverse, false);
            }
            platforms[i].SetActive(false); // Start inactive
        }
    }

    private void Update(){
        //Here we check what level we are on. If we are on the level for these spawners than we spawn the platforms
        if(GameManager.Instance.level == level && !isSpawning){
            // Start spawning platforms
            StartCoroutine(SpawnPlatforms());
        } 
    }

    private IEnumerator SpawnPlatforms()
    {
        isSpawning = true;
        int index = 0;
        while (index < platformCount)
        {
            // Activate the next platform in the array
            platforms[index].SetActive(true);
            platforms[index].transform.position = spawnPoint.position; // Reset position

            // Move to the next platform 
            index = (index + 1);

            // Wait for the specified spawn interval. We add in a little randomness to make level different every time
            yield return new WaitForSeconds(spawnInterval + Random.Range(-0.5f, 0.5f));
        }
    }
}