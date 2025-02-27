using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private GameObject childCrystal;
    private bool isBroken;
    private SpriteRenderer topHalfRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topHalfRenderer = childCrystal.GetComponent<SpriteRenderer>();
    }

   private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Fire Ability") && !isBroken){
            //Set the sprites to broken sprite
            topHalfRenderer.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenSprite;
            transform.position += new Vector3(0, 0.2f, 0); // Moves it up slightly as it is lowered for some reason
            Debug.Log("My name is ejfff");
            //Hurt azula

            //Set is broken to true so we cant retrigger this
        }
    }
}
