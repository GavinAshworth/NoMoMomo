using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; //checkpoint location
    private bool isTriggered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isTriggered) return;
        // Check if the collided object is Momo 
        if (collision.CompareTag("Player")) // Making sure its only momo who triggers this
        {
            // Get Momo's AbyssCollision script and update the start position
            Momo momo = collision.GetComponent<Momo>();
            if (momo != null)
            {
                isTriggered = true;
                //Update the location at which momo respawns
                momo.UpdateRespawnPosition(checkpoint.position);
                //Level up in our game manager
                GameManager.Instance.LevelUp();
            }
        }
    }
}