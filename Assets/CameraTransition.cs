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
            MoveCameraInstantly(); //right now this is instant, might try to add in an animation later
        }
    }

    private void MoveCameraInstantly()
    {
        // Move the camera to the target position 
        mainCamera.transform.position = cameraTarget.position;
    }
}