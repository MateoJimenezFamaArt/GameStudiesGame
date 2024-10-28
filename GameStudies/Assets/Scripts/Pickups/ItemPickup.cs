using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public StatType statToModify; // Dropdown to select which stat to modify
    public float modificationAmount = 20f; // Amount to modify the selected stat

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatsManager statsManager = other.GetComponent<PlayerStatsManager>();
            if (statsManager != null)
            {
                ApplyStatModification(statsManager);
            }
            Destroy(gameObject); // Remove the item from the scene
        }
    }

    private void ApplyStatModification(PlayerStatsManager statsManager)
    {
        switch (statToModify)
        {
            case StatType.Health:
                statsManager.ApplyHealthUpgrade(modificationAmount);
                break;
            case StatType.Speed:
                statsManager.ApplySpeedUpgrade(modificationAmount);
                break;
            case StatType.Damage:
                statsManager.ApplyDamageUpgrade(modificationAmount);
                break;
            case StatType.DiveForce:
                statsManager.ApplyDiveForceUpgrade(modificationAmount);
                break;
            case StatType.LightAttackCooldownReduction:
                statsManager.ApplyLightAttackCooldownReduction(modificationAmount);
                break;
            case StatType.HeavyAttackCooldownReduction:
                statsManager.ApplyHeavyAttackCooldownReduction(modificationAmount);
                break;
            case StatType.JumpForce:
                statsManager.ApplyJumpForceUpgrade(modificationAmount);
                break;
            case StatType.Bounciness:
                statsManager.ApplyBouncinessUpgrade(modificationAmount);
                break;
            case StatType.HeavyAttackDamage:
                statsManager.ApplyHeavyAttackDamageUpgrade(modificationAmount);
                break;
            case StatType.LightAttackDamage:
                statsManager.ApplyLightAttackDamageUpgrade(modificationAmount);
                break;
            default:
                Debug.LogWarning("Stat type not recognized!");
                break;
        }
    }
}
