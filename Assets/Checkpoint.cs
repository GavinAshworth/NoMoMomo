using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; //checkpoint location
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is Momo 
        if (collision.CompareTag("Player")) // Making sure its only momo who triggers this
        {
            // Get Momo's AbyssCollision script and update the start position
            Momo momo = collision.GetComponent<Momo>();
            if (momo != null)
            {
                momo.UpdateRespawnPosition(checkpoint.position);
                Debug.Log("Checkpoint updated to: " + transform.position);
            }
        }
    }
}