using TMPro;
using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public StatType statToModify; // Dropdown to select which stat to modify
    public float modificationAmount = 20f; // Amount to modify the selected stat
    public PlayerStats statsManager;

    private TextMeshProUGUI PickupDisplay;
    private float displayDuration = 1.3f; // Time in seconds to display the text

    private void Start()
    {
        PickupDisplay = GameObject.Find("Pickup").GetComponent<TextMeshProUGUI>();
        PickupDisplay.text = " ";
    }

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
        string statName = ""; // Variable to hold the name of the modified stat

        switch (statToModify)
        {
            case StatType.Health:
                statsManager.ModifyHealth(modificationAmount);
                statName = "Health";
                break;
            case StatType.Speed:
                statsManager.ModifySpeed(modificationAmount);
                statName = "Speed";
                break;
            case StatType.Damage:
                statsManager.ModifyDamage(modificationAmount);
                statName = "Damage";
                break;
            case StatType.DiveForce:
                statsManager.ModifyDiveForce(modificationAmount);
                statName = "Dive Force";
                break;
            case StatType.LightAttackCooldownReduction:
                statsManager.ModifyLightAttackCooldownReduction(modificationAmount);
                statName = "Light Attack Cooldown";
                break;
            case StatType.HeavyAttackCooldownReduction:
                statsManager.ModifyHeavyAttackCooldownReduction(modificationAmount);
                statName = "Heavy Attack Cooldown";
                break;
            case StatType.JumpForce:
                statsManager.ModifyJumpForce(modificationAmount);
                statName = "Jump Force";
                break;
            case StatType.Bounciness:
                statsManager.ModifyBounciness(modificationAmount);
                statName = "Bounciness";
                break;
            case StatType.HeavyAttackDamage:
                statsManager.ModifyHeavyAttackDamage(modificationAmount);
                statName = "Heavy Attack Damage";
                break;
            case StatType.LightAttackDamage:
                statsManager.ModifyLightAttackDamage(modificationAmount);
                statName = "Light Attack Damage";
                break;
            default:
                Debug.LogWarning("Stat type not recognized!");
                break;
        }

        // Update the display text with the stat modification information
        PickupDisplay.text = $"{statName} modified by {modificationAmount}";
        
        // Start the coroutine to clear the text after a delay
        StartCoroutine(ClearPickupDisplayAfterDelay());
    }

    private IEnumerator ClearPickupDisplayAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        PickupDisplay.text = "";
    }
}
