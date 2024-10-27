using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    public float jumpForce = 5f;
    public float diveForce = 10f;
    public float gravityMultiplier = 2f;
    public float groundCheckDistance = 0.2f; // Adjusted for procedural terrain
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isDiving = false;
    private Vector3 playerVelocity;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //playerStats.ResetStats();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        HandleMovement();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            animator.SetTrigger("Dive");
            StartCoroutine(Dive());
        }
    }

    private void HandleMovement()
    {
        // Capture input for movement direction and apply momentum
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        // Smoothly transition movement using momentum
        Vector3 desiredVelocity = move * playerStats.currentSpeed;
        playerVelocity = Vector3.Lerp(playerVelocity, desiredVelocity, 0.1f); // Adjust for responsiveness

        // Move player using updated velocity
        rb.MovePosition(rb.position + playerVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    private IEnumerator Dive()
    {
        isDiving = true;
        rb.AddForce(transform.forward * diveForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(1.0f); // Reduced for quicker recovery
        isDiving = false;
    }

    private void GroundCheck()
    {
        // Improved ground check using capsule cast for procedural terrains
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Bounce effect for walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 bounceDirection = Vector3.Reflect(playerVelocity, collision.contacts[0].normal);
            rb.linearVelocity = bounceDirection * 0.5f; // Adjust for bounce effect
        }
        // Knockback effect for enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 knockback = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockback * 5f, ForceMode.Impulse); // Adjust force for desired knockback
        }
    }
}
