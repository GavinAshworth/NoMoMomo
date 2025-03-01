using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget; // The camera's target position for the next level, (*12 up from the last one)

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure only momo can trigger just in case
        {
            MoveCamera(); //right now this is instant, might try to add in an animation later
            
            //Also move momo up one tile after the transition so hes not half in half out
            collision.transform.position += Vector3.up * 1f;

            //This ensures that when momo enters a new map he is centered again
            Vector3 pos = collision.transform.position;
            pos.x = Mathf.Round(pos.x) + 0.5f;
            collision.transform.position = pos;
        }
    }

    private void MoveCamera()
    {
        // Move the camera to the target position 
        mainCamera.transform.position = cameraTarget.position;
    }
}