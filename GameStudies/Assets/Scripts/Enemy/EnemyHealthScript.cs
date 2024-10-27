using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    public event System.Action OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float baseDamage, ElementType elementType)
    {
        float finalDamage = CalculateDamage(baseDamage, elementType);
        currentHealth -= finalDamage;

        Debug.Log("Enemy took damage: " + finalDamage + " from " + elementType + " element.");

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    private int CalculateDamage(float baseDamage, ElementType elementType)
    {
        float damageMultiplier = 1.0f;

        // Example of elemental weaknesses/resistances (customize as needed)
        switch (elementType)
        {
            case ElementType.Fire:
                damageMultiplier = 1.2f;  // Fire deals extra damage
                break;
            case ElementType.Ice:
                damageMultiplier = 0.8f;  // Ice deals less damage
                break;
            case ElementType.Electric:
                damageMultiplier = 1.1f;
                break;
            case ElementType.Poison:
                damageMultiplier = 1.3f;  // Poison is highly effective
                break;
            case ElementType.Wind:
                damageMultiplier = 1.0f;
                break;
            case ElementType.Earth:
                damageMultiplier = 0.9f;
                break;
            case ElementType.Neutral:
                damageMultiplier = 1.0f;
                break;
        }

        return Mathf.RoundToInt(baseDamage * damageMultiplier);
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}
