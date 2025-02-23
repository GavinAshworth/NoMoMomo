using UnityEngine;
using System.Collections;
public class AbyssCollision : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition; // Momo's current reset position
    [SerializeField] private LayerMask groundLayer; // Layer for floating platforms and ground
    [SerializeField] private float dissapearTime;
    private bool isOnGround;
    private Renderer momoRenderer; //So we can make him dissapear for a second after falling off the map
    private Rigidbody2D momoRigidbody; // Reference to Momo's Rigidbody2D so we can stop his movement while he is being reset

    private void Start(){
        //Get Momo's sprite renderer
        momoRenderer = GetComponent<SpriteRenderer>();
        if (momoRenderer == null)
        {
            Debug.LogError("Momo is missing a Renderer component!");
        }
        // Get Momo's Rigidbody2D component
        momoRigidbody = GetComponent<Rigidbody2D>();
        if (momoRigidbody == null)
        {
            Debug.LogError("Momo is missing a Rigidbody2D component!");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Heaso");
       if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Momo has left the ground!");
            StartCoroutine(ResetMomo());
        }
    }

    //this is called by our checkpoint system to update the reset point for momo upon sucessful reach of a checkpointa
    public void UpdateStartPosition(Vector3 newPosition)
    {
        startPosition = newPosition;
    }


    private IEnumerator ResetMomo()
    {
        // Freeze Momo's Rigidbody2D
        if (momoRigidbody != null)
        {
            momoRigidbody.linearVelocity = Vector2.zero; // Stop any existing movement
            momoRigidbody.bodyType = RigidbodyType2D.Kinematic; // Freeze the Rigidbody
        }
        // Disable Momo's renderer to make him disappear
        if (momoRenderer != null)
        {
            momoRenderer.enabled = false;
        }
        // Reset Momo's position
        transform.position = startPosition;
        // Wait for 0.1 seconds
        yield return new WaitForSeconds(dissapearTime);
        // Re-enable Momo's renderer to make him reappear
        momoRenderer.enabled = true;

        momoRigidbody.bodyType = RigidbodyType2D.Dynamic; // Unfreeze the Rigidbody so he can move again once rendered back
        

        Debug.Log("Momo reset to start position: " + startPosition);
    }
}