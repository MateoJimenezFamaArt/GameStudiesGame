using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public bool canJump = true;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public bool showCrosshair = true;

    private float xRotation = 0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Crosshair Settings")]
    public Texture2D crosshairTexture;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        RotateCamera();
        HandleJump();
        CheckForClick();
    }

    void MovePlayer()
    {
        isGrounded = characterController.isGrounded;

        // Reset downward velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Check for sprint
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        characterController.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }

    void CheckForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player clicked!");
        }
    }

    void OnGUI()
    {
        if (showCrosshair && crosshairTexture != null)
        {
            float crosshairSize = 20f;
            Rect rect = new Rect(
                (Screen.width - crosshairSize) / 2,
                (Screen.height - crosshairSize) / 2,
                crosshairSize,
                crosshairSize
            );
            GUI.DrawTexture(rect, crosshairTexture);
        }
    }
}
