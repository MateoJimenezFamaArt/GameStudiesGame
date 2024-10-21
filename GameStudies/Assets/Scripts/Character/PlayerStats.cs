using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float defaultHealth = 100f;
    public float defaultSpeed = 5f;
    public float defaultDamage = 20f;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentDamage;

    // Method to reset stats to default values
    public void ResetStats()
    {
        currentHealth = defaultHealth;
        currentSpeed = defaultSpeed;
        currentDamage = defaultDamage;
    }

    // Method to apply upgrades (or downgrades)
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, defaultHealth); // Prevent negative health
    }

    public void ModifySpeed(float amount)
    {
        currentSpeed += amount;
    }

    public void ModifyDamage(float amount)
    {
        currentDamage += amount;
    }
}
