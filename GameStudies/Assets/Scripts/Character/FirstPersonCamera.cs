using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject crosshair;
    
    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks cursor to the center of the screen
        crosshair.SetActive(true); // Display crosshair
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Restrict vertical rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
