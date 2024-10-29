using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public StatType statToModify; // Dropdown to select which stat to modify
    public float modificationAmount = 20f; // Amount to modify the selected stat
    public PlayerStats statsManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (statsManager != null)
            {
                ApplyStatModification(statsManager);
            }
            Destroy(gameObject); // Remove the item from the scene
        }
    }

    private void ApplyStatModification(PlayerStats statsManager)
    {
        switch (statToModify)
        {
            case StatType.Health:
                statsManager.ModifyHealth(modificationAmount);
                break;
            case StatType.Speed:
                statsManager.ModifySpeed(modificationAmount);
                break;
            case StatType.Damage:
                statsManager.ModifyDamage(modificationAmount);
                break;
            case StatType.DiveForce:
                statsManager.ModifyDiveForce(modificationAmount);
                break;
            case StatType.LightAttackCooldownReduction:
                statsManager.ModifyLightAttackCooldownReduction(modificationAmount);
                break;
            case StatType.HeavyAttackCooldownReduction:
                statsManager.ModifyHeavyAttackCooldownReduction(modificationAmount);
                break;
            case StatType.JumpForce:
                statsManager.ModifyJumpForce(modificationAmount);
                break;
            case StatType.Bounciness:
                statsManager.ModifyBounciness(modificationAmount);
                break;
            case StatType.HeavyAttackDamage:
                statsManager.ModifyHeavyAttackDamage(modificationAmount);
                break;
            case StatType.LightAttackDamage:
                statsManager.ModifyLightAttackDamage(modificationAmount);
                break;
            default:
                Debug.LogWarning("Stat type not recognized!");
                break;
        }
    }
}
