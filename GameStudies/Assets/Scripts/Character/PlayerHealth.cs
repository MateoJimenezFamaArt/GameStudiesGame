using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    public float invulnerabilityDuration = 0.5f; // Duration of invulnerability after taking damage
    public Renderer playerRenderer; // Reference to the player renderer for visual feedback (optional)
    public Color invulnerableColor = Color.red; // Color to show when invulnerable (optional)
    public Camera mainCamera; // Reference to the main camera to turn the screen red
    public float deathDelay = 2f; // Delay before transitioning to death scene

    public float currentHealth;
    private bool isInvulnerable = false;
    private Color originalColor;
    private Color originalCameraColor;

    private Animator animator;

    private TextMeshProUGUI currentlife;

    private void Start()
    {
        if (RunsManager.Instance.runsCompleted == 0)
        {
            currentHealth = playerStats.currentHealth;
        } else 
        {
            currentHealth = RunsManager.Instance.PreservedHealth;
        }
        //currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        // Store original colors for resetting
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
        if (mainCamera != null)
        {
            originalCameraColor = mainCamera.backgroundColor;
        }
        currentlife = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();

        currentlife.text = "Player Health: " + currentHealth.ToString();
    }

    public void TakeDamage(float damage)
    {
        // If the player is invulnerable, ignore the damage
        if (isInvulnerable) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        RunsManager.Instance.PreservedHealth = currentHealth;

        currentlife.text = "Player Health: " + currentHealth.ToString();

        Debug.Log ("La vida guardada en el manager es de : " + RunsManager.Instance.PreservedHealth);

        // Trigger invulnerability and visual feedback
        StartCoroutine(InvulnerabilityRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        // Visual feedback for invulnerability
        if (playerRenderer != null)
        {
            playerRenderer.material.color = invulnerableColor;
        }

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;

        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        RunsManager.Instance.runsCompleted = 0;
        playerStats.ResetStats();
        Debug.Log("Player died!");
        SceneManager.LoadScene("You Are Dead");
    }
}
