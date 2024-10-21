using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public event System.Action OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage: " + damage);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}
