using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private GameObject childCrystal;
    private bool isBroken;
    private SpriteRenderer topHalfRenderer;
    private ParticleSystem ps;
    [SerializeField] private Azula azula;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topHalfRenderer = childCrystal.GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
    }

   private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Fire Ability") && !isBroken){
            //Set the sprites to broken sprite
            topHalfRenderer.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenSprite;
            transform.position += new Vector3(0, 0.2f, 0); // Moves it up slightly as it is lowered for some reason
            ps.Stop(); //Stops the particle system
            //Hurt azula
            azula.TakeDamage();
            //Set is broken to true so we cant retrigger this
            isBroken = true;
        }
    }
}
