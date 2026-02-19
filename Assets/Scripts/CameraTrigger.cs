using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraController cameraController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraController.AtivarCamera3();
        }
    }
}
