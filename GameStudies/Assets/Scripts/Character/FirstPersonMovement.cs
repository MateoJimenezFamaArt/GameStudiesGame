using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    public float jumpForce = 5f;
    public float diveForce = 10f;
    public float gravityMultiplier = 2f;
    public float groundCheckDistance = 0.1f; // Distance for ground checking
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isDiving = false;

    private Animator  animator;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStats.ResetStats(); // Reset stats when the game starts or the player respawns
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        GroundCheck();
        MovePlayer();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetTrigger("Jump");
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDiving && isGrounded)
        {
            animator.SetTrigger("Dive");
            StartCoroutine(Dive());
        }
    }

    private void MovePlayer()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        rb.MovePosition(transform.position + move * playerStats.currentSpeed * Time.deltaTime); // Use currentSpeed
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
        yield return new WaitForSeconds(1.5f);
        isDiving = false;
    }

    private void GroundCheck()
    {
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
