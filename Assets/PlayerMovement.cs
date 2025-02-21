using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f; // Set movement step size
    [SerializeField] private float moveTime = 0.1f;  // Time to complete movement
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false; // Prevent multiple inputs before finishing move

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isMoving) return; // Prevent movement spam

        if (moveDirection != Vector2.zero)
        {
            Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveDistance;
            StartCoroutine(MoveToPosition(targetPosition));
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving) return; // Ignore input while moving

        Vector2 input = context.ReadValue<Vector2>();

        // Prevent diagonal movement: Prioritize horizontal or vertical
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            moveDirection = new Vector2(Mathf.Sign(input.x), 0);
        else
            moveDirection = new Vector2(0, Mathf.Sign(input.y));
    }

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
    }
}
