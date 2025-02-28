using UnityEngine;
using UnityEngine.InputSystem;

public class Momo : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f; // Movement step size for momo. This should be 1 tile worth
    [SerializeField] private float moveTime = 0.5f;  // Time to complete movement, change based on animation
    [SerializeField] private GameObject deathSprite;
    [SerializeField] private bool isGodMode;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false; // Prevents multiple inputs before finishing move, like arcade version
    private Animator animator; // Our animator used for sprite animations
    private SpriteRenderer spriteRenderer; // Using this to flip the sprite when moving to the left as I didnt make a move left animation
    private Camera mainCamera;
    private Abilities abilities;
    [SerializeField] bool startAtAzula;

    //Momos respawn point. Gets updated based on checkpoint
    [SerializeField] private Vector3 respawnPoint; // Momo's current reset position
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        abilities = GetComponent<Abilities>();
        animator.SetFloat("LastInputY", 1); // Just making momo start facing forward


        //for testing purposes, goes to level 5 and brings momo to the boss
        if(startAtAzula){
            transform.position = new Vector3(0.5f, 41.8f, 0f);
            for(int i = 0; i<3; i++){
                GameManager.Instance.LevelUp();
            }
        }
    }

    private void Update()
    {
        if (isMoving) return; // Prevent movement spam

        if (moveDirection != Vector2.zero)
        {
            Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveDistance; // Calc our next position

            //We check if momo wants to jump outside the frame
            if(!IsPositionInCameraView(targetPosition)){
                //we stop the jump animation
                animator.SetBool("isJumping", false); // Reset the jumping animation
                moveDirection = Vector2.zero; // Reset input
                Debug.Log("Cannot move outside the camera!");
                return;
            }
           Debug.Log(abilities.GetIsFlying() + "AAAAAAAAAAAAA");
            //We check the tile that we are going to move to for collisions
            Collider2D platform = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
            Collider2D abyss = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Abyss"));
            Collider2D ground = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Ground")); //non moving ground that momo is safe on
            // If momo lands on a platfrom we attach him to it
            if (platform != null) {
                transform.SetParent(platform.transform);
            } else {
                transform.SetParent(null);
            }
            // Momo dies when he lands in the abyss (loses a life and gets reset). If he is flying (using air ability we dont call this)
            if (abyss != null && platform == null && ground == null && !abilities.GetIsFlying() && !isGodMode)
            {
                //Call our death function. Currently everything just does 1 damage for now
                Death(targetPosition, 1);
            }else{
                StopAllCoroutines();
                StartCoroutine(MoveToPosition(targetPosition)); // This allows us to move smoothly instead of just teleporting, looks nicer
            }
           
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving) return; // Ignore input while moving
        animator.SetBool("isJumping", true);

        Vector2 input = context.ReadValue<Vector2>();

        // Prevent diagonal movement: Prioritize horizontal or vertical, for example stops a user from pressing up and right at the same time and moving diagonally
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            moveDirection = new Vector2(Mathf.Sign(input.x), 0);
        else
            moveDirection = new Vector2(0, Mathf.Sign(input.y));

        // Flip the sprite if moving left
        if (moveDirection.x < 0)
            spriteRenderer.flipX = true; // Flip the sprite to face left
        else if (moveDirection.x > 0)
            spriteRenderer.flipX = false; // Reset the sprite to face right

        animator.SetFloat("InputX", moveDirection.x);
        animator.SetFloat("InputY", moveDirection.y);
        animator.SetFloat("LastInputX", moveDirection.x);
        animator.SetFloat("LastInputY", moveDirection.y);
    }

    // How we generate smooth movement despite our snappy frogger move style
    private System.Collections.IEnumerator MoveToPosition(Vector2 target)
    {
        isMoving = true;
        Vector2 startPosition = rb.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            rb.linearVelocity = (target - startPosition) / moveTime; // Smooth transition
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = target; // Snap to final position
        rb.linearVelocity = Vector2.zero; // Stop movement
        isMoving = false;
        moveDirection = Vector2.zero; // Reset input

        //We snap Y to the nearest whole number to prevent some stupid bugs
        Vector3 pos = transform.position;
        pos.y = Mathf.Round(pos.y);
        transform.position = pos;

        animator.SetBool("isJumping", false); // Stops the jumping animation and returns to idle
    }

    private bool IsPositionInCameraView(Vector2 position)
    {
        // Convert the target position to viewport space
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(position);

        // Check if the position is within the camera's viewport (0 to 1 in both X and Y)
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

     //this is called by our checkpoint system to update the reset point for momo upon sucessful reach of a checkpointa
    public void UpdateRespawnPosition(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Death(Vector2 target, int damage){
        if(isGodMode) return;// for testing/debugging
        StopAllCoroutines();    
        moveDirection = Vector2.zero;
        isMoving = false;
        animator.SetBool("isJumping", false);
        transform.SetParent(null);
        //Show hurt sprite at the destination tile
        if(deathSprite!=null){
            GameObject deathFeedback = Instantiate(deathSprite, target, Quaternion.identity);
            //We will have it last for a couple seconds and then destroy it. Maybe later we will have it fade out
            Destroy(deathFeedback, 2f);
        }else{
            Debug.Log("No death sprite attached to momo script");
        }

        //Reset Momo's abilities
        abilities.StopAbility();
        //Disable control of this script so momo cant move during death
        enabled = false;

        //Disable momo so he cannot do anything while dead

        gameObject.SetActive(false);

        //Call our death function in the game manager
        GameManager.Instance.HasDied(damage);
    }

    public void Respawn(){
        //This is called by our game manager after death, if we have more lives left
        StopAllCoroutines(); 

        //Re enable momo
        gameObject.SetActive(true);

        //re-enable control
        enabled = true;

        //spawn momo at respawn point
        transform.position = respawnPoint;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        //This is for when momo gets hit by a projectile
        bool isProjectile = collision.gameObject.layer == LayerMask.NameToLayer("Projectile");
        if(isProjectile && !abilities.GetIsShielded()) {
            Death(transform.position,1);
        }
    }
}