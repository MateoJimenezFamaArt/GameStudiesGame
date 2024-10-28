using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float defaultHealth = 100f;
    public float defaultSpeed = 5f;
    public float defaultDamage = 20f;

    // New stats
    public float defaultDiveForce = 10f; 
    public float defaultLightAttackCooldownReduction = 1f; // Time in seconds
    public float defaultHeavyAttackCooldownReduction = 1f; // Time in seconds
    public float defaultJumpForce = 10f;
    public float defaultBounciness = 1f; // Factor of bounciness
    public float defaultHeavyAttackDamage = 3f;
    public float defaultLightAttackDamage = 1f;

    [HideInInspector] public float currentHealth; //YA
    [HideInInspector] public float currentSpeed; //YA
    [HideInInspector] public float currentDamage; //YA

    // New current stats
    [HideInInspector] public float currentDiveForce; //YA
    [HideInInspector] public float currentLightAttackCooldownReduction; //YA
    [HideInInspector] public float currentHeavyAttackCooldownReduction; //YA
    [HideInInspector] public float currentJumpForce; // YA
    [HideInInspector] public float currentBounciness; //YA
    [HideInInspector] public float currentHeavyAttackDamage;//YA
    [HideInInspector] public float currentLightAttackDamage;//YA

    // Method to reset stats to default values


    public void ResetStats()
    {
        currentHealth = defaultHealth;
        currentSpeed = defaultSpeed;
        currentDamage = defaultDamage;

        // Reset new stats
        currentDiveForce = defaultDiveForce;
        currentLightAttackCooldownReduction = defaultLightAttackCooldownReduction;
        currentHeavyAttackCooldownReduction = defaultHeavyAttackCooldownReduction;
        currentJumpForce = defaultJumpForce;
        currentBounciness = defaultBounciness;
        currentHeavyAttackDamage = defaultHeavyAttackDamage;
        currentLightAttackDamage = defaultLightAttackDamage;
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
    
    // New methods to modify stats
    public void ModifyDiveForce(float amount) { currentDiveForce += amount; }
    public void ModifyLightAttackCooldownReduction(float amount) { currentLightAttackCooldownReduction += amount; }
    public void ModifyHeavyAttackCooldownReduction(float amount) { currentHeavyAttackCooldownReduction += amount; }
    public void ModifyJumpForce(float amount) { currentJumpForce += amount; }
    public void ModifyBounciness(float amount) { currentBounciness += amount; }
    public void ModifyHeavyAttackDamage(float amount) { currentHeavyAttackDamage += amount; }
    public void ModifyLightAttackDamage(float amount) { currentLightAttackDamage += amount; }
}
