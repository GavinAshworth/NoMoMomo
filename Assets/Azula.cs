using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] float fireballSpeed = 10f; // Speed of fireballs
    [SerializeField] float fireballResetDistance = 50f; // Distance at which fireballs reset


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
        int attackChoice = Random.Range(0, 5); // 0 = Lightning, 1 = Fire

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
        //attack here

        float lightningLength = anim.GetCurrentAnimatorStateInfo(0).length;
        //pause for duration of attack
        yield return new WaitForSeconds(lightningLength);
        StartCoroutine(ResetAttackCooldown(0)); 
    }

    private System.Collections.IEnumerator PerformFireBarrage()
    {
        readyToAttack = false;
        fireCount = 5 + (4 - lives)*2;
        while(fireCount>0){
            anim.SetTrigger("FireAttack");
            yield return null;
        }     
    }

    private void SpawnFireball(){
        GameObject fireball = GetFireballFromPool();
        fireball.SetActive(true);
        fireball.transform.position = transform.position;
        // Initialize the fireball
        AzulaFireBall fireballScript = fireball.GetComponent<AzulaFireBall>();
        fireballScript.Initialize(transform, momo.transform, fireballSpeed);
        //reduce fireCount
        fireCount--;
        if(fireCount==0){
            StartCoroutine(ResetAttackCooldown(1));
        }
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

    private void ResetFireball(GameObject fireball)
    {
        fireball.SetActive(false);
        fireball.transform.position = transform.position;
    }

    private System.Collections.IEnumerator ResetAttackCooldown(int type)
    {
        anim.SetBool("IsAttack", false);
        if(type == 0){
            //pause two seconds after lightning attack
            yield return new WaitForSeconds(2.5f);
        }else{
            //pause 1 second after fire attack
            yield return new WaitForSeconds(1.5f);
        }
        readyToAttack = true;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void TakeDamage()
    {
        SetLives(lives - 1);
        anim.SetTrigger("Hurt");
        if (lives <= 0)
        {
            // Transition to Death animation here and disable attacks
            isAlive = false;
            anim.SetBool("IsDead", true);
        }
    }
}