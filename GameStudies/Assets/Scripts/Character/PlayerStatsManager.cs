using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

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
}
