using UnityEngine;

public class Azula : MonoBehaviour
{
    private int lives = 4;
    private bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetLives(int lives){
        this.lives = lives;
    }
    public void TakeDamage(){
        SetLives(lives -1);
        Debug.Log("Taking Damage");
        if(lives <=0){
            //Transition to Death animation here and disable attacks
            Debug.Log("Azula has died :(");
            isAlive = false;
        }
    }
}
