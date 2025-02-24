using UnityEngine;
using System.Collections;
public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab; // Platform prefab to spawn
    [SerializeField] private int platformCount = 3; // Number of platforms to spawn
    [SerializeField] private Transform spawnPoint; // Spawn point for platforms
    [SerializeField] private Transform endPoint; // End point for platforms
    [SerializeField] private float spawnInterval = 2f; // Time between platform spawns
    [SerializeField] private bool isReverse;
    private GameObject[] platforms; // Array to store the platforms

    private void Start()
    {
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
            platforms[i] = Instantiate(platformPrefab, spawnPoint.position, Quaternion.identity);
            platforms[i].GetComponent<MovingPlatform>().Initialize(spawnPoint, endPoint);
            platforms[i].SetActive(false); // Start inactive
        }

        // Start spawning platforms
        StartCoroutine(SpawnPlatforms());
    }

    private IEnumerator SpawnPlatforms()
    {
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