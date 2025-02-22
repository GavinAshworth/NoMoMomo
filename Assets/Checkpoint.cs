using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; //checkpoint location
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is Momo (e.g., by tag or layer)
        if (collision.CompareTag("Player")) // Assuming Momo has the "Player" tag
        {
            // Get Momo's AbyssCollision script and update the start position
            AbyssCollision momo = collision.GetComponent<AbyssCollision>();
            if (momo != null)
            {
                momo.UpdateStartPosition(checkpoint.position);
                Debug.Log("Checkpoint updated to: " + transform.position);
            }
        }
    }
}