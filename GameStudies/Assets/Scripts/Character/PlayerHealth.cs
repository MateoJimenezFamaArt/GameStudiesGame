using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float invulnerabilityDuration = 0.5f; // Duration of invulnerability after taking damage
    public Renderer playerRenderer; // Reference to the player renderer for visual feedback (optional)
    public Color invulnerableColor = Color.red; // Color to show when invulnerable (optional)

    private float currentHealth;
    private bool isInvulnerable = false;
    private Color originalColor;

    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        // If the player is invulnerable, ignore the damage
        if (isInvulnerable) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        // Trigger invulnerability and visual feedback
        StartCoroutine(InvulnerabilityRoutine());

        //Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        // Set player as invulnerable
        isInvulnerable = true;

        // Optional: Visual feedback (change player color or material to indicate invulnerability)
        if (playerRenderer != null)
        {
            playerRenderer.material.color = invulnerableColor;
        }

        // Wait for the duration of invulnerability
        yield return new WaitForSeconds(invulnerabilityDuration);

        // Reset invulnerability
        isInvulnerable = false;

        // Optional: Reset visual feedback to original color
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        //Debug.Log("Player died!");
        // Add death logic here (e.g., trigger death animation, restart level, etc.)
    }
}
