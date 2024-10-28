using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        PlayerStats playerStats = (PlayerStats)target;

        // Create a button in the inspector
        if (GUILayout.Button("Print Current Stats"))
        {
            PrintCurrentStats(playerStats);
        }
    }

    private void PrintCurrentStats(PlayerStats playerStats)
    {
        // Print the current values of all stats
        Debug.Log($"Current Health: {playerStats.currentHealth}");
        Debug.Log($"Current Speed: {playerStats.currentSpeed}");
        Debug.Log($"Current Damage: {playerStats.currentDamage}");
        Debug.Log($"Current Dive Force: {playerStats.currentDiveForce}");
        Debug.Log($"Current Light Attack Cooldown Reduction: {playerStats.currentLightAttackCooldownReduction}");
        Debug.Log($"Current Heavy Attack Cooldown Reduction: {playerStats.currentHeavyAttackCooldownReduction}");
        Debug.Log($"Current Jump Force: {playerStats.currentJumpForce}");
        Debug.Log($"Current Bounciness: {playerStats.currentBounciness}");
        Debug.Log($"Current Heavy Attack Damage: {playerStats.currentHeavyAttackDamage}");
        Debug.Log($"Current Light Attack Damage: {playerStats.currentLightAttackDamage}");
    }
}
