using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    // Methods for applying upgrades to existing stats
    public void ApplyHealthUpgrade(float amount)
    {
        playerStats.ModifyHealth(amount);
    }

    public void ApplySpeedUpgrade(float amount)
    {
        playerStats.ModifySpeed(amount);
    }

    public void ApplyDamageUpgrade(float amount)
    {
        playerStats.ModifyDamage(amount);
    }

    // New methods for the additional stats
    public void ApplyDiveForceUpgrade(float amount)
    {
        playerStats.ModifyDiveForce(amount);
    }

    public void ApplyLightAttackCooldownReduction(float amount)
    {
        playerStats.ModifyLightAttackCooldownReduction(amount);
    }

    public void ApplyHeavyAttackCooldownReduction(float amount)
    {
        playerStats.ModifyHeavyAttackCooldownReduction(amount);
    }

    public void ApplyJumpForceUpgrade(float amount)
    {
        playerStats.ModifyJumpForce(amount);
    }

    public void ApplyBouncinessUpgrade(float amount)
    {
        playerStats.ModifyBounciness(amount);
    }

    public void ApplyHeavyAttackDamageUpgrade(float amount)
    {
        playerStats.ModifyHeavyAttackDamage(amount);
    }

    public void ApplyLightAttackDamageUpgrade(float amount)
    {
        playerStats.ModifyLightAttackDamage(amount);
    }

    // Optionally, you could add a method to reset stats
    public void ResetPlayerStats()
    {
        playerStats.ResetStats();
    }
}
