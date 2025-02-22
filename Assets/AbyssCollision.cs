using UnityEngine;
using System.Collections;
public class AbyssCollision : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition; // Momo's current reset position
    [SerializeField] private LayerMask groundLayer; // Layer for floating platforms and ground
    [SerializeField] private float dissapearTime;
    private bool isOnGround;
    private Renderer momoRenderer; //So we can make him dissapear for a second after falling off the map

    private void Start(){
        momoRenderer = GetComponent<SpriteRenderer>();
        if (momoRenderer == null)
        {
            Debug.LogError("Momo is missing a Renderer component!");
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

    //this is called by our checkpoint system to update the reset point for momo upon sucessful reach of
    public void UpdateStartPosition(Vector3 newPosition)
    {
        startPosition = newPosition;
    }


    private IEnumerator ResetMomo()
    {
        // Disable Momo's renderer to make him disappear
        if (momoRenderer != null)
        {
            momoRenderer.enabled = false;
        }
        // Wait for 0.1 seconds
        yield return new WaitForSeconds(dissapearTime);
        // Reset Momo's position
        transform.position = startPosition;
        // Re-enable Momo's renderer to make him reappear
        momoRenderer.enabled = true;
        

        Debug.Log("Momo reset to start position: " + startPosition);
    }
}