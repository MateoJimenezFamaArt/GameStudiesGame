using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;

    [SerializeField]
    private float currentHealth;

    private EnemyElement enemyElement;
    private EnemyAudioManager audioManager;
    private EnemyStateMachine stateMachine;

    public event System.Action OnDeath;

    private bool isDead = false; // Flag to check if the enemy is already dead

    private void Start()
    {
        currentHealth = maxHealth;
        enemyElement = GetComponent<EnemyElement>();
        audioManager = GetComponent<EnemyAudioManager>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    public void TakeDamage(float baseDamage, ElementType attackerElement)
    {
        // Check if the enemy is already dead; if so, ignore further damage
        if (isDead) return;

        audioManager.PlayRandomHitSound();
        float finalDamage = CalculateDamage(baseDamage, attackerElement);
        currentHealth -= finalDamage;

        /*Debug.Log($"Enemy took damage: {finalDamage} from {attackerElement} element.");
        Debug.Log($"Base Damage: {baseDamage} | Final Damage: {finalDamage}");*/

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    private float CalculateDamage(float baseDamage, ElementType attackerElement)
    {
        ElementType enemyElementType = enemyElement.enemyElement;
        float damageMultiplier = GetDamageMultiplier(enemyElementType, attackerElement);

        string effectiveness = GetEffectiveness(enemyElementType, attackerElement);
        //Debug.Log($"Attacker Element: {attackerElement}, Enemy Element: {enemyElementType}, Effectiveness: {effectiveness}");

        return Mathf.RoundToInt(baseDamage * damageMultiplier);
    }

    private float GetDamageMultiplier(ElementType enemyElementType, ElementType attackerElement)
    {
        float[,] multiplierTable = new float[,]
        {
            { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f },
            { 1f, 2f, 0.8f, 1f, 1f, 1.3f },
            { 0.8f, 1f, 1f, 1f, 2f, 1.3f },
            { 1f, 1f, 1f, 0.8f, 2f, 1.3f },
            { 1f, 1.3f, 1f, 1f, 0.8f, 1f },
            { 1.3f, 0.8f, 1f, 2f, 1f, 1f },
            { 0.8f, 1f, 1.3f, 1f, 2f, 1f }
        };

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
        isDead = true; // Set the enemy as dead to prevent further damage

        //Debug.Log("Enemy has died.");

        stateMachine.SetState(EnemyStateMachine.EnemyState.Dying);
        // Play the death sound
        audioManager.PlayRandomDeathSound();

        // Start the coroutine to destroy the enemy after a delay
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(6f); // Wait for 5 seconds (adjust as needed)
        Destroy(gameObject);
    }
}
