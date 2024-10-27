using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    private EnemyElement enemyElement; // Cache reference to EnemyElement

    public event System.Action OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyElement = GetComponent<EnemyElement>(); // Cache the EnemyElement reference
    }

    public void TakeDamage(float baseDamage, ElementType attackerElement)
    {
        float finalDamage = CalculateDamage(baseDamage, attackerElement);
        currentHealth -= finalDamage;

        // Log details about the damage received
        Debug.Log($"Enemy took damage: {finalDamage} from {attackerElement} element.");
        Debug.Log($"Base Damage: {baseDamage} | Final Damage: {finalDamage}");

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    private float CalculateDamage(float baseDamage, ElementType attackerElement)
    {
        ElementType enemyElementType = enemyElement.enemyElement; // Get enemy's element type
        float damageMultiplier = GetDamageMultiplier(enemyElementType, attackerElement);

        // Log the damage type and multiplier details
        string effectiveness = GetEffectiveness(enemyElementType, attackerElement);
        Debug.Log($"Attacker Element: {attackerElement}, Enemy Element: {enemyElementType}, Effectiveness: {effectiveness}");

        return Mathf.RoundToInt(baseDamage * damageMultiplier);
    }

    private float GetDamageMultiplier(ElementType enemyElementType, ElementType attackerElement)
    {
        // Damage multipliers based on your balance table
        float[,] multiplierTable = new float[,]
        {
            { 1f, 2f, 0.8f, 1f, 1f, 1.3f }, // Light
            { 0.8f, 1f, 1f, 1f, 2f, 1.3f }, // Dark
            { 1f, 1f, 1f, 0.8f, 2f, 1.3f }, // Fire
            { 1f, 1.3f, 1f, 1f, 0.8f, 1f }, // Water
            { 1.3f, 0.8f, 1f, 2f, 1f, 1f }, // Grass
            { 0.8f, 1f, 1.3f, 1f, 2f, 1f }  // Poison
        };

        // Map ElementType to array indices
        int enemyIndex = (int)enemyElementType;
        int attackerIndex = (int)attackerElement;

        return multiplierTable[enemyIndex, attackerIndex];
    }

    private string GetEffectiveness(ElementType enemyElementType, ElementType attackerElement)
    {
        float multiplier = GetDamageMultiplier(enemyElementType, attackerElement);
        
        if (multiplier > 1)
            return "Strong";
        else if (multiplier < 1)
            return "Weak";
        else
            return "Neutral";
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}
