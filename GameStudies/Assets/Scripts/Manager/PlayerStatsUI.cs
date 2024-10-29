using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats playerStats;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI diveForceText;
    public TextMeshProUGUI jumpForceText;
    public TextMeshProUGUI bouncinessText;
    public TextMeshProUGUI heavyAttackDamageText;
    public TextMeshProUGUI lightAttackDamageText;

    private void OnEnable()
    {
        // Subscribe to events
        playerStats.OnHealthChanged.AddListener(UpdateHealthUI);
        playerStats.OnSpeedChanged.AddListener(UpdateSpeedUI);
        playerStats.OnDamageChanged.AddListener(UpdateDamageUI);
        playerStats.OnDiveForceChanged.AddListener(UpdateDiveForceUI);
        playerStats.OnJumpForceChanged.AddListener(UpdateJumpForceUI);
        playerStats.OnBouncinessChanged.AddListener(UpdateBouncinessUI);
        playerStats.OnHeavyAttackDamageChanged.AddListener(UpdateHeavyAttackDamageUI);
        playerStats.OnLightAttackDamageChanged.AddListener(UpdateLightAttackDamageUI);

        // Initialize UI with current values
        UpdateHealthUI(playerStats.currentHealth);
        UpdateSpeedUI(playerStats.currentSpeed);
        UpdateDamageUI(playerStats.currentDamage);
        UpdateDiveForceUI(playerStats.currentDiveForce);
        UpdateJumpForceUI(playerStats.currentJumpForce);
        UpdateBouncinessUI(playerStats.currentBounciness);
        UpdateHeavyAttackDamageUI(playerStats.currentHeavyAttackDamage);
        UpdateLightAttackDamageUI(playerStats.currentLightAttackDamage);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        playerStats.OnHealthChanged.RemoveListener(UpdateHealthUI);
        playerStats.OnSpeedChanged.RemoveListener(UpdateSpeedUI);
        playerStats.OnDamageChanged.RemoveListener(UpdateDamageUI);
        playerStats.OnDiveForceChanged.RemoveListener(UpdateDiveForceUI);
        playerStats.OnJumpForceChanged.RemoveListener(UpdateJumpForceUI);
        playerStats.OnBouncinessChanged.RemoveListener(UpdateBouncinessUI);
        playerStats.OnHeavyAttackDamageChanged.RemoveListener(UpdateHeavyAttackDamageUI);
        playerStats.OnLightAttackDamageChanged.RemoveListener(UpdateLightAttackDamageUI);
    }

    // UI update methods
    private void UpdateHealthUI(float value) { healthText.text = $"Health: {value:F0}"; }
    private void UpdateSpeedUI(float value) { speedText.text = $"Speed: {value:F1}"; }
    private void UpdateDamageUI(float value) { damageText.text = $"Damage: {value:F1}"; }
    private void UpdateDiveForceUI(float value) { diveForceText.text = $"Dive Force: {value:F1}"; }
    private void UpdateJumpForceUI(float value) { jumpForceText.text = $"Jump Force: {value:F1}"; }
    private void UpdateBouncinessUI(float value) { bouncinessText.text = $"Bounciness: {value:F1}"; }
    private void UpdateHeavyAttackDamageUI(float value) { heavyAttackDamageText.text = $"Heavy Attack Damage: {value:F1}"; }
    private void UpdateLightAttackDamageUI(float value) { lightAttackDamageText.text = $"Light Attack Damage: {value:F1}"; }
}
