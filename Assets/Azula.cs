using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Azula : MonoBehaviour
{
    private int lives = 4;
    private bool isAlive = true;
    private Animator anim;
    private bool readyToAttack = true;
    private List<GameObject> fireballPool = new List<GameObject>();
    [SerializeField] GameObject fireballPrefab; // Prefab for the fireball
    [SerializeField] GameObject momo; // Player
    [SerializeField] int poolSize = 100; // Number of fireballs to pool
    [SerializeField] float fireballSpeed = 5f; // Speed of fireballs
    [SerializeField] float fireballResetDistance = 15f; // Distance at which fireballs reset
    [SerializeField] Tilemap pathToCrystals; //This is the path that allows momo to get to crystals
    [SerializeField] GameObject LightningSet1;
    [SerializeField] GameObject LightningSet2;
    [SerializeField] GameObject LightningSet1B;
    [SerializeField] GameObject LightningSet2B;
    private bool isPathAvailable = true;


    private bool isResetting;

    private int fireCount;
    void Start()
    {
        anim = GetComponent<Animator>();

        // Initialize the fireball pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.SetActive(false); // Deactivate fireballs initially
            fireballPool.Add(fireball);
        }

        //Set the Lightning sets to inactive
        LightningSet1.SetActive(false);
        LightningSet2.SetActive(false);
        LightningSet1B.SetActive(false);
        LightningSet2B.SetActive(false);
    }

    void Update()
    {
        if (readyToAttack && isAlive && GameManager.Instance.level ==5)
        {
            PerformRandomAttack();
        }

        // Check and reset fireballs that are too far away
        foreach (GameObject fireball in fireballPool)
        {
            if (fireball.activeInHierarchy && Vector3.Distance(fireball.transform.position, transform.position) > fireballResetDistance)
            {
                ResetFireball(fireball);
            }
        }
    }

    private void PerformRandomAttack()
    {
        anim.SetBool("IsAttack", true);
        int attackChoice = Random.Range(0, 2); // 0 = Lightning, 1,2 = Fire

        if (attackChoice == 0)
        {
            StartCoroutine(TriggerLightningAttack());
        }
        else
        {
            StartCoroutine(PerformFireBarrage());
        }
    }

    private System.Collections.IEnumerator TriggerLightningAttack()
    {
        readyToAttack = false;
        anim.SetTrigger("LightningAttack");
        yield return null; 
    }

    private System.Collections.IEnumerator PerformFireBarrage()
    {
        readyToAttack = false;
        //Up difficulty based on lives left
        fireCount = 5 + (4 - lives)*2;
        fireballSpeed = 5 + (4 - lives)*2;
        anim.SetBool("isFireAttack", true);
        yield return null;     
    }

    private void SpawnFireball(){
    if (fireCount <= 0)
    {
        return;
    }

    // Fireball aimed at momo
    SpawnSingleFireballAtTarget(momo.transform.position);

    // Calculate how many lives have been lost
    int livesLost = 4 - lives;

    // For each life lost, add two more fireballs (left & right)
    for (int i = 1; i <= livesLost; i++)
    {
        float offset = 2f * i;  // 2, 4, 6 for each lost life

        // Left and Right positions relative to momo
        Vector3 leftOffset = momo.transform.position + Vector3.left * offset;
        Vector3 rightOffset = momo.transform.position + Vector3.right * offset;

        SpawnSingleFireballAtTarget(leftOffset);
        SpawnSingleFireballAtTarget(rightOffset);
    }

    // Reduce fire count (each "burst" counts as one shot, regardless of how many fireballs it contains)
    fireCount--;

    if (fireCount <= 1 && !isResetting)
    {
        StartCoroutine(ResetAttackCooldown(1));
    }
}

private void SpawnSingleFireballAtTarget(Vector3 targetPosition)
{
    GameObject fireball = GetFireballFromPool();
    if (fireball == null) return;

    fireball.SetActive(true);
    fireball.transform.position = transform.position;  // Spawn from Azula

    // Point the fireball at the target position
    AzulaFireBall fireballScript = fireball.GetComponent<AzulaFireBall>();
    fireballScript.Initialize(transform, targetPosition, fireballSpeed);
}

    private int GetFireballCount()
    {
        // Number of fireballs increases as lives decrease
        switch (lives)
        {
            case 4: return 1;
            case 3: return 3;
            case 2: return 5;
            case 1: return 7;
            default: return 1;
        }
    }

    private GameObject GetFireballFromPool()
    {
        // Find an inactive fireball in the pool
        foreach (GameObject fireball in fireballPool)
        {
            if (!fireball.activeInHierarchy)
            {
                return fireball;
            }
        }
        return null; // No available fireballs
    }

    private void LightningIndicator(){
        //Spawn animation of lightning indicator on tiles
        int lightningChoice = Random.Range(0, 2);
        if(lightningChoice == 0){
            LightningSet1.SetActive(true);
            if(isPathAvailable){
                LightningSet1B.SetActive(true);
            }
        }else{
            LightningSet2.SetActive(true);
            if(isPathAvailable){
                LightningSet2B.SetActive(true);
            }
        } 
    }
    private void LightningDamage(){
        //Make the actual damage happen 
        LightningSet1.SetActive(false);
        LightningSet2.SetActive(false);
        LightningSet1B.SetActive(false);
        LightningSet2B.SetActive(false);
        StartCoroutine(ResetAttackCooldown(0));
    }

    private void ResetFireball(GameObject fireball)
    {
        fireball.SetActive(false);
        fireball.transform.position = transform.position;
    }

    private System.Collections.IEnumerator ResetAttackCooldown(int type)
    {
        isResetting = true;
        anim.SetBool("IsAttack", false);
        if(type == 0){
            //pause two seconds after lightning attack
            yield return new WaitForSeconds(2f);
        }else{
            //pause 1 second after fire attack
            anim.SetBool("isFireAttack", false);
            yield return new WaitForSeconds(3f);
        }
        readyToAttack = true;
        isResetting = false;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void TakeDamage()
    {
        SetLives(lives - 1);
        anim.SetTrigger("Hurt");
        readyToAttack = true; //immediatley good to attack again
        LightningSet1.SetActive(false); //Turn off the lightning attacks
        LightningSet2.SetActive(false);
        LightningSet1B.SetActive(false);
        LightningSet2B.SetActive(false);
        StartCoroutine(removePath()); //remove paths
        if (lives <= 0)
        {
            // Transition to Death animation here and disable attacks
            isAlive = false;
            anim.SetBool("IsDead", true);
        }
    }

    private System.Collections.IEnumerator removePath(){
        pathToCrystals.gameObject.SetActive(false);
        isPathAvailable = false;
        yield return new WaitForSeconds(10f); //path goes away for 10 seconds
        pathToCrystals.gameObject.SetActive(true);
        isPathAvailable = true;
    }
}