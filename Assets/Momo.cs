using UnityEngine;
using UnityEngine.InputSystem;

public class Momo : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f; // Movement step size for momo. This should be 1 tile worth
    [SerializeField] private float moveTime = 0.5f;  // Time to complete movement, change based on animation

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false; // Prevents multiple inputs before finishing move, like arcade version
    private Animator animator; // Our animator used for sprite animations
    private SpriteRenderer spriteRenderer; // Using this to flip the sprite when moving to the left as I didnt make a move left animation
    private Camera mainCamera;

    //Momos respawn point. Gets updated based on checkpoint
    [SerializeField] private Vector3 respawnPoint; // Momo's current reset position
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        animator.SetFloat("LastInputY", 1); // Just making momo start facing forward
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

            // Momo dies when he lands in the abyss (loses a life and gets reset)
            if (abyss != null && platform == null && ground == null)
            {
                StopAllCoroutines();
                moveDirection = Vector2.zero;
                animator.SetBool("isJumping", false);
                //here we will call death function once game manager is set up. right now we will just reset momo
                transform.position = respawnPoint;
                //Death();
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
}