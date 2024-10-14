using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float diveForce = 10f;
    public float gravityMultiplier = 2f;
    public float groundCheckDistance = 0.1f; // Distance for ground checking
    public LayerMask groundLayer; // Set the layer of the ground in the inspector
    
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isDiving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents character from falling over
    }

    private void Update()
    {
        GroundCheck(); // Check if the player is on the ground
        MovePlayer();
        Debug.Log("IsGrounded state: " + isGrounded);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Dive with the Control key (left control)
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDiving && isGrounded)
        {
            StartCoroutine(Dive());
        }
    }

    private void MovePlayer()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        // Toggle sprinting with the Shift key (left shift)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        rb.MovePosition(transform.position + move * currentSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private IEnumerator Dive()
    {
        isDiving = true;
        rb.AddForce(transform.forward * diveForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1.5f); // Duration of the dive
        isDiving = false;
    }

    private void GroundCheck()
    {
        // Perform a raycast from the player's position downwards to check for ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
